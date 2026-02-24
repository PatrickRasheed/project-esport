using Npgsql;
using TournamentManager.Models;

namespace TournamentManager.DataAccess
{
    public class InscriptionRepository
    {
        private readonly DatabaseContext _context;

        public InscriptionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(Inscription inscription)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO INSCRIPTION (id_equipe, id_tournoi, date_inscription, seed, est_valide)
                VALUES (@idEquipe, @idTournoi, @dateInscription, @seed, @estValide)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idEquipe", inscription.IdEquipe);
            cmd.Parameters.AddWithValue("@idTournoi", inscription.IdTournoi);
            cmd.Parameters.AddWithValue("@dateInscription", inscription.DateInscription);
            cmd.Parameters.AddWithValue("@seed", inscription.Seed);
            cmd.Parameters.AddWithValue("@estValide", inscription.EstValide);

            cmd.ExecuteNonQuery();
        }

        public List<Inscription> GetByTournoi(int idTournoi)
        {
            var inscriptions = new List<Inscription>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT * FROM INSCRIPTION WHERE id_tournoi = @idTournoi";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idTournoi", idTournoi);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                inscriptions.Add(new Inscription
                {
                    IdEquipe = reader.GetInt32(0),
                    IdTournoi = reader.GetInt32(1),
                    DateInscription = reader.GetDateTime(2),
                    Seed = reader.GetInt32(3),
                    EstValide = reader.GetBoolean(4)
                });
            }

            return inscriptions;
        }

        public int CountByTournoi(int idTournoi)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM INSCRIPTION WHERE id_tournoi = @idTournoi";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idTournoi", idTournoi);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Delete(int idEquipe, int idTournoi)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "DELETE FROM INSCRIPTION WHERE id_equipe = @idEquipe AND id_tournoi = @idTournoi";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
            cmd.Parameters.AddWithValue("@idTournoi", idTournoi);

            cmd.ExecuteNonQuery();
        }
    }
}