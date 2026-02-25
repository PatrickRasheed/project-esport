using Npgsql;
using TournamentManager.Models;

namespace TournamentManager.DataAccess
{
    public class JeuRepository
    {
        private readonly DatabaseContext _context;

        public JeuRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Add(Jeu jeu)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO jeux (nom_jeu, editeur, genre, annee_sortie, description, nb_joueurs_min_equipe, nb_joueurs_max_equipe)
                VALUES (@nom, @editeur, @genre, @annee, @description, @nbMin, @nbMax)
                RETURNING id_jeux";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom", jeu.NomJeu);
            cmd.Parameters.AddWithValue("@editeur", jeu.Editeur ?? "");
            cmd.Parameters.AddWithValue("@genre", jeu.Genre ?? "");
            cmd.Parameters.AddWithValue("@annee", jeu.AnneeSortie);
            cmd.Parameters.AddWithValue("@description", jeu.Description ?? "");
            cmd.Parameters.AddWithValue("@nbMin", jeu.NbJoueursMinEquipe);
            cmd.Parameters.AddWithValue("@nbMax", jeu.NbJoueursMaxEquipe);

            return (int)cmd.ExecuteScalar()!;
        }

        public List<Jeu> GetAll()
        {
            var jeux = new List<Jeu>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_jeux, nom_jeu, editeur, genre, annee_sortie, description, nb_joueurs_min_equipe, nb_joueurs_max_equipe FROM jeux";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                jeux.Add(new Jeu
                {
                    IdJeux = reader.GetInt32(0),
                    NomJeu = reader.GetString(1),
                    Editeur = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Genre = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    AnneeSortie = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Description = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    NbJoueursMinEquipe = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    NbJoueursMaxEquipe = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                });
            }

            return jeux;
        }

        public Jeu? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_jeux, nom_jeu, editeur, genre, annee_sortie, description, nb_joueurs_min_equipe, nb_joueurs_max_equipe FROM jeux WHERE id_jeux = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Jeu
                {
                    IdJeux = reader.GetInt32(0),
                    NomJeu = reader.GetString(1),
                    Editeur = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Genre = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    AnneeSortie = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Description = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    NbJoueursMinEquipe = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    NbJoueursMaxEquipe = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                };
            }

            return null;
        }
    }
}