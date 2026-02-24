using Npgsql;
using TournamentManager.Models;

namespace TournamentManager.DataAccess
{
    public class JoueurRepository
    {
        private readonly DatabaseContext _context;

        public JoueurRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Add(Joueur joueur)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO joueur (pseudo, nom_reel, prenom, date_naissance, role_jeu, est_titulaire, id_equipe)
                VALUES (@pseudo, @nomReel, @prenom, @dateNaissance, @role, @estTitulaire, @idEquipe)
                RETURNING id_joueur";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pseudo", joueur.Pseudo);
            cmd.Parameters.AddWithValue("@nomReel", joueur.NomReel ?? "");
            cmd.Parameters.AddWithValue("@prenom", joueur.Prenom ?? "");
            cmd.Parameters.AddWithValue("@dateNaissance", joueur.DateNaissance);
            cmd.Parameters.AddWithValue("@role", joueur.RoleJeu ?? "");
            cmd.Parameters.AddWithValue("@estTitulaire", joueur.EstTitulaire);
            cmd.Parameters.AddWithValue("@idEquipe", joueur.IdEquipe);

            return (int)cmd.ExecuteScalar()!;
        }

        public List<Joueur> GetAll()
        {
            var joueurs = new List<Joueur>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_joueur, pseudo, nom_reel, prenom, date_naissance, role_jeu, est_titulaire, id_equipe FROM joueur";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                joueurs.Add(new Joueur
                {
                    IdJoueur = reader.GetInt32(0),
                    Pseudo = reader.GetString(1),
                    NomReel = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Prenom = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    DateNaissance = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                    RoleJeu = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    EstTitulaire = reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                    IdEquipe = reader.GetInt32(7)
                });
            }

            return joueurs;
        }

        public List<Joueur> GetByEquipe(int idEquipe)
        {
            var joueurs = new List<Joueur>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_joueur, pseudo, nom_reel, prenom, date_naissance, role_jeu, est_titulaire, id_equipe FROM joueur WHERE id_equipe = @idEquipe";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idEquipe", idEquipe);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                joueurs.Add(new Joueur
                {
                    IdJoueur = reader.GetInt32(0),
                    Pseudo = reader.GetString(1),
                    NomReel = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Prenom = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    DateNaissance = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                    RoleJeu = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    EstTitulaire = reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                    IdEquipe = reader.GetInt32(7)
                });
            }

            return joueurs;
        }

        public void Update(Joueur joueur)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                UPDATE joueur 
                SET pseudo = @pseudo, nom_reel = @nomReel, prenom = @prenom, 
                    date_naissance = @dateNaissance, role_jeu = @role, 
                    est_titulaire = @estTitulaire, id_equipe = @idEquipe
                WHERE id_joueur = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", joueur.IdJoueur);
            cmd.Parameters.AddWithValue("@pseudo", joueur.Pseudo);
            cmd.Parameters.AddWithValue("@nomReel", joueur.NomReel ?? "");
            cmd.Parameters.AddWithValue("@prenom", joueur.Prenom ?? "");
            cmd.Parameters.AddWithValue("@dateNaissance", joueur.DateNaissance);
            cmd.Parameters.AddWithValue("@role", joueur.RoleJeu ?? "");
            cmd.Parameters.AddWithValue("@estTitulaire", joueur.EstTitulaire);
            cmd.Parameters.AddWithValue("@idEquipe", joueur.IdEquipe);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "DELETE FROM joueur WHERE id_joueur = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}