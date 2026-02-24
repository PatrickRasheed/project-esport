using TournamentManager.DataAccess;
using TournamentManager.Models;

namespace TournamentManager.Services
{
    public class TournoiService
    {
        private readonly TournoiRepository _tournoiRepo;

        public TournoiService(DatabaseContext context)
        {
            _tournoiRepo = new TournoiRepository(context);
        }

        public int CreerTournoi(string nom, int idJeux, DateTime dateDebut, DateTime dateFin, 
                                int nbEquipesMax, string format, decimal prizePool)
        {
            var tournoi = new Tournoi
            {
                NomTournoi = nom,
                IdJeux = idJeux,
                DateDebut = dateDebut,
                DateFin = dateFin,
                NombreEquipesMax = nbEquipesMax,
                Format = format,
                PrizePool = prizePool,
                Statut = "Ouvert"
            };

            return _tournoiRepo.Add(tournoi);
        }

        public void AfficherTournois()
        {
            var tournois = _tournoiRepo.GetAll();

            if (tournois.Count == 0)
            {
                Console.WriteLine("Aucun tournoi disponible.");
                return;
            }

            Console.WriteLine("\n=== LISTE DES TOURNOIS ===");
            foreach (var t in tournois)
            {
                Console.WriteLine($"[{t.IdTournoi}] {t.NomTournoi} - {t.Statut}");
                Console.WriteLine($"    Prize Pool: {t.PrizePool:C} | {t.DateDebut:dd/MM/yyyy} → {t.DateFin:dd/MM/yyyy}");
                Console.WriteLine($"    Format: {t.Format} | Max équipes: {t.NombreEquipesMax}");
                Console.WriteLine();
            }
        }

        public void DemarrerTournoi(int idTournoi)
        {
            var tournoi = _tournoiRepo.GetById(idTournoi);
            if (tournoi == null)
            {
                Console.WriteLine("❌ Tournoi introuvable.");
                return;
            }

            if (tournoi.Statut != "Ouvert")
            {
                Console.WriteLine($"❌ Le tournoi est déjà {tournoi.Statut}.");
                return;
            }

            tournoi.Statut = "En cours";
            _tournoiRepo.Update(tournoi);

            Console.WriteLine($"✅ Le tournoi '{tournoi.NomTournoi}' a démarré !");
        }

        public void TerminerTournoi(int idTournoi)
        {
            var tournoi = _tournoiRepo.GetById(idTournoi);
            if (tournoi == null)
            {
                Console.WriteLine("❌ Tournoi introuvable.");
                return;
            }

            tournoi.Statut = "Terminé";
            _tournoiRepo.Update(tournoi);

            Console.WriteLine($"✅ Le tournoi '{tournoi.NomTournoi}' est terminé !");
        }
    }
}