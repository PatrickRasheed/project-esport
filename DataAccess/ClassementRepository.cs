using Npgsql;
using TournamentManager.Models;

namespace TournamentManager.DataAccess
{
    public class ClassementRepository
    {
        private readonly DatabaseContext _context;

        public ClassementRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<Classement> GetByTournoi(int idTournoi)
        {
            var classements = new List<Classement>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                SELECT id_classement, id_equipe, id_tournoi, points, victoires, defaites, matchs_nuls, position_actuelle 
                FROM classement 
                WHERE id_tournoi = @idTournoi 
                ORDER BY position_actuelle";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idTournoi", idTournoi);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                classements.Add(new Classement
                {
                    IdClassement = reader.GetInt32(0),
                    IdEquipe = reader.GetInt32(1),
                    IdTournoi = reader.GetInt32(2),
                    Points = reader.GetInt32(3),
                    Victoires = reader.GetInt32(4),
                    Defaites = reader.GetInt32(5),
                    MatchsNuls = reader.GetInt32(6),
                    PositionActuelle = reader.GetInt32(7)
                });
            }

            return classements;
        }
    }
}