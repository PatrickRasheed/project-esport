using Npgsql;
using TournamentManager.Models;

namespace TournamentManager.DataAccess
{
    public class EquipeRepository
    {
        private readonly DatabaseContext _context;

        public EquipeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Add(Equipe equipe)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO equipe (nom_equipe, tag_equipe, date_creation, pays)
                VALUES (@nom, @tag, @dateCreation, @pays)
                RETURNING id_equipe";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom", equipe.NomEquipe);
            cmd.Parameters.AddWithValue("@tag", equipe.TagEquipe ?? "");
            cmd.Parameters.AddWithValue("@dateCreation", equipe.DateCreation);
            cmd.Parameters.AddWithValue("@pays", equipe.Pays ?? "");

            return (int)cmd.ExecuteScalar()!;
        }

        public List<Equipe> GetAll()
        {
            var equipes = new List<Equipe>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_equipe, nom_equipe, tag_equipe, date_creation, pays FROM equipe";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                equipes.Add(new Equipe
                {
                    IdEquipe = reader.GetInt32(0),
                    NomEquipe = reader.GetString(1),
                    TagEquipe = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    DateCreation = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                    Pays = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }

            return equipes;
        }

        public Equipe? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_equipe, nom_equipe, tag_equipe, date_creation, pays FROM equipe WHERE id_equipe = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Equipe
                {
                    IdEquipe = reader.GetInt32(0),
                    NomEquipe = reader.GetString(1),
                    TagEquipe = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    DateCreation = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                    Pays = reader.IsDBNull(4) ? "" : reader.GetString(4)
                };
            }

            return null;
        }

        public void Update(Equipe equipe)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                UPDATE equipe 
                SET nom_equipe = @nom, tag_equipe = @tag, date_creation = @dateCreation, pays = @pays
                WHERE id_equipe = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", equipe.IdEquipe);
            cmd.Parameters.AddWithValue("@nom", equipe.NomEquipe);
            cmd.Parameters.AddWithValue("@tag", equipe.TagEquipe ?? "");
            cmd.Parameters.AddWithValue("@dateCreation", equipe.DateCreation);
            cmd.Parameters.AddWithValue("@pays", equipe.Pays ?? "");

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "DELETE FROM equipe WHERE id_equipe = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}