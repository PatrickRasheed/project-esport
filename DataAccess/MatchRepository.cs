using Npgsql;
using TournamentManager.Models;

namespace TournamentManager.DataAccess
{
    public class MatchRepository
    {
        private readonly DatabaseContext _context;

        public MatchRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int Add(Match match)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO match_ (id_tournoi, date_match, phase_tournoi, format_match, statut)
                VALUES (@idTournoi, @dateMatch, @phase, @format, @statut)
                RETURNING id_match";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idTournoi", match.IdTournoi);
            cmd.Parameters.AddWithValue("@dateMatch", match.DateMatch);
            cmd.Parameters.AddWithValue("@phase", match.PhaseTournoi ?? "");
            cmd.Parameters.AddWithValue("@format", match.FormatMatch ?? "");
            cmd.Parameters.AddWithValue("@statut", match.Statut ?? "");

            return (int)cmd.ExecuteScalar()!;
        }

        public List<Match> GetByTournoi(int idTournoi)
        {
            var matchs = new List<Match>();

            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_match, id_tournoi, date_match, phase_tournoi, format_match, statut FROM match_ WHERE id_tournoi = @idTournoi ORDER BY date_match";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idTournoi", idTournoi);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                matchs.Add(new Match
                {
                    IdMatch = reader.GetInt32(0),
                    IdTournoi = reader.GetInt32(1),
                    DateMatch = reader.GetDateTime(2),
                    PhaseTournoi = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    FormatMatch = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Statut = reader.IsDBNull(5) ? "" : reader.GetString(5)
                });
            }

            return matchs;
        }

        public Match? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = "SELECT id_match, id_tournoi, date_match, phase_tournoi, format_match, statut FROM match_ WHERE id_match = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Match
                {
                    IdMatch = reader.GetInt32(0),
                    IdTournoi = reader.GetInt32(1),
                    DateMatch = reader.GetDateTime(2),
                    PhaseTournoi = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    FormatMatch = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Statut = reader.IsDBNull(5) ? "" : reader.GetString(5)
                };
            }

            return null;
        }

        public void Update(Match match)
        {
            using var conn = _context.GetConnection();
            conn.Open();

            string query = @"
                UPDATE match_ 
                SET statut = @statut, date_match = @dateMatch, phase_tournoi = @phase, format_match = @format
                WHERE id_match = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", match.IdMatch);
            cmd.Parameters.AddWithValue("@statut", match.Statut ?? "");
            cmd.Parameters.AddWithValue("@dateMatch", match.DateMatch);
            cmd.Parameters.AddWithValue("@phase", match.PhaseTournoi ?? "");
            cmd.Parameters.AddWithValue("@format", match.FormatMatch ?? "");

            cmd.ExecuteNonQuery();
        }
    }
}