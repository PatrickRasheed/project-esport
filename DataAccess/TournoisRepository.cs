using Npgsql;
using TournamentManager.Models;

namespace TournamentManager.DataAccess
{
    public class TournoiRepository
    {
        private readonly DatabaseContext _context;

        public TournoiRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Add(Tournoi tournoi)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO tournoi (nom_tournoi, id_jeux, date_debut, date_fin, nombre_equipes_max, format, prize_pool, statut)
                VALUES (@nom, @idJeux, @dateDebut, @dateFin, @nbEquipesMax, @format, @prizePool, @statut)
                RETURNING id_tournoi";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom", tournoi.NomTournoi);
            cmd.Parameters.AddWithValue("@idJeux", tournoi.IdJeux);
            cmd.Parameters.AddWithValue("@dateDebut", tournoi.DateDebut);
            cmd.Parameters.AddWithValue("@dateFin", tournoi.DateFin);
            cmd.Parameters.AddWithValue("@nbEquipesMax", tournoi.NombreEquipesMax);
            cmd.Parameters.AddWithValue("@format", tournoi.Format);
            cmd.Parameters.AddWithValue("@prizePool", tournoi.PrizePool);
            cmd.Parameters.AddWithValue("@statut", tournoi.Statut);

            return (int)cmd.ExecuteScalar()!;
        }

        public List<Tournoi> GetAll()
        {
            var tournois = new List<Tournoi>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                SELECT id_tournoi, nom_tournoi, id_jeux, date_debut, date_fin, 
                       nombre_equipes_max, format, prize_pool, statut 
                FROM tournoi";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tournois.Add(new Tournoi
                {
                    IdTournoi = reader.GetInt32(0),
                    NomTournoi = reader.GetString(1),
                    IdJeux = reader.GetInt32(2),
                    DateDebut = reader.GetDateTime(3),
                    DateFin = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                    NombreEquipesMax = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    Format = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    PrizePool = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                    Statut = reader.IsDBNull(8) ? "" : reader.GetString(8)
                });
            }

            return tournois;
        }

        public Tournoi? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                SELECT id_tournoi, nom_tournoi, id_jeux, date_debut, date_fin, 
                       nombre_equipes_max, format, prize_pool, statut 
                FROM tournoi 
                WHERE id_tournoi = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Tournoi
                {
                    IdTournoi = reader.GetInt32(0),
                    NomTournoi = reader.GetString(1),
                    IdJeux = reader.GetInt32(2),
                    DateDebut = reader.GetDateTime(3),
                    DateFin = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                    NombreEquipesMax = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    Format = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    PrizePool = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                    Statut = reader.IsDBNull(8) ? "" : reader.GetString(8)
                };
            }

            return null;
        }

        public void Update(Tournoi tournoi)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                UPDATE tournoi 
                SET nom_tournoi = @nom, id_jeux = @idJeux, date_debut = @dateDebut, 
                    date_fin = @dateFin, nombre_equipes_max = @nbEquipesMax, 
                    format = @format, prize_pool = @prizePool, statut = @statut
                WHERE id_tournoi = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", tournoi.IdTournoi);
            cmd.Parameters.AddWithValue("@nom", tournoi.NomTournoi);
            cmd.Parameters.AddWithValue("@idJeux", tournoi.IdJeux);
            cmd.Parameters.AddWithValue("@dateDebut", tournoi.DateDebut);
            cmd.Parameters.AddWithValue("@dateFin", tournoi.DateFin);
            cmd.Parameters.AddWithValue("@nbEquipesMax", tournoi.NombreEquipesMax);
            cmd.Parameters.AddWithValue("@format", tournoi.Format);
            cmd.Parameters.AddWithValue("@prizePool", tournoi.PrizePool);
            cmd.Parameters.AddWithValue("@statut", tournoi.Statut);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "DELETE FROM tournoi WHERE id_tournoi = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}