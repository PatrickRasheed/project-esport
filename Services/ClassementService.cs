using TournamentManager.DataAccess;

namespace TournamentManager.Services
{
    public class ClassementService
    {
        private readonly ClassementRepository _classementRepo;
        private readonly TournoiRepository _tournoiRepo;
        private readonly EquipeRepository _equipeRepo;

        public ClassementService(DatabaseContext context)
        {
            _classementRepo = new ClassementRepository(context);
            _tournoiRepo = new TournoiRepository(context);
            _equipeRepo = new EquipeRepository(context);
        }

        public void AfficherClassement(int idTournoi)
        {
            var tournoi = _tournoiRepo.GetById(idTournoi);
            if (tournoi == null)
            {
                Console.WriteLine("❌ Tournoi introuvable.");
                return;
            }

            var classements = _classementRepo.GetByTournoi(idTournoi);

            Console.WriteLine($"\n╔════════════════════════════════════════╗");
            Console.WriteLine($"║  📊 CLASSEMENT - {tournoi.NomTournoi}");
            Console.WriteLine($"╚════════════════════════════════════════╝");

            if (classements.Count == 0)
            {
                Console.WriteLine("Aucun classement disponible pour ce tournoi.");
                return;
            }

            Console.WriteLine($"\n{"Pos",-4} {"Équipe",-25} {"Pts",-5} {"V",-4} {"D",-4} {"N",-4}");
            Console.WriteLine(new string('─', 60));

            foreach (var c in classements)
            {
                var equipe = _equipeRepo.GetById(c.IdEquipe);
                string nomEquipe = equipe != null ? equipe.NomEquipe : "Inconnue";

                string medaille = c.PositionActuelle switch
                {
                    1 => "🥇",
                    2 => "🥈",
                    3 => "🥉",
                    _ => $"{c.PositionActuelle}."
                };

                Console.WriteLine($"{medaille,-4} {nomEquipe,-25} {c.Points,-5} {c.Victoires,-4} {c.Defaites,-4} {c.MatchsNuls,-4}");
            }
        }
    }
}