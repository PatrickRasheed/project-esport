using TournamentManager.DataAccess;
using TournamentManager.Models;

namespace TournamentManager.Services
{
    public class MatchService
    {
        private readonly MatchRepository _matchRepo;
        private readonly TournoiRepository _tournoiRepo;
        private readonly EquipeRepository _equipeRepo;

        public MatchService(DatabaseContext context)
        {
            _matchRepo = new MatchRepository(context);
            _tournoiRepo = new TournoiRepository(context);
            _equipeRepo = new EquipeRepository(context);
        }

        public int CreerMatch(int idTournoi, DateTime dateMatch, string phase, string format)
        {
            var match = new Match
            {
                IdTournoi = idTournoi,
                DateMatch = dateMatch,
                PhaseTournoi = phase,
                FormatMatch = format,
                Statut = "Planifié"
            };

            return _matchRepo.Add(match);
        }

        public void AfficherMatchsParTournoi(int idTournoi)
        {
            var tournoi = _tournoiRepo.GetById(idTournoi);
            if (tournoi == null)
            {
                Console.WriteLine("❌ Tournoi introuvable.");
                return;
            }

            var matchs = _matchRepo.GetByTournoi(idTournoi);

            Console.WriteLine($"\n=== MATCHS DE {tournoi.NomTournoi} ===");
            if (matchs.Count == 0)
            {
                Console.WriteLine("Aucun match planifié pour ce tournoi.");
                return;
            }

            foreach (var m in matchs)
            {
                string iconeStatut = m.Statut switch
                {
                    "Terminé" => "✅",
                    "En cours" => "🔴",
                    "Planifié" => "📅",
                    _ => "❓"
                };

                Console.WriteLine($"{iconeStatut} [{m.IdMatch}] {m.PhaseTournoi} - {m.FormatMatch}");
                Console.WriteLine($"    📅 {m.DateMatch:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"    Statut: {m.Statut}");
                Console.WriteLine();
            }
        }

        public void DemarrerMatch(int idMatch)
        {
            var match = _matchRepo.GetById(idMatch);
            if (match == null)
            {
                Console.WriteLine("❌ Match introuvable.");
                return;
            }

            match.Statut = "En cours";
            _matchRepo.Update(match);
            Console.WriteLine("✅ Match démarré !");
        }

        public void TerminerMatch(int idMatch)
        {
            var match = _matchRepo.GetById(idMatch);
            if (match == null)
            {
                Console.WriteLine("❌ Match introuvable.");
                return;
            }

            match.Statut = "Terminé";
            _matchRepo.Update(match);
            Console.WriteLine("✅ Match terminé !");
        }
    }
}