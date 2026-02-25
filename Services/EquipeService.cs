using TournamentManager.DataAccess;
using TournamentManager.Models;

namespace TournamentManager.Services
{
    public class EquipeService
    {
        private readonly EquipeRepository _equipeRepo;
        private readonly JoueurRepository _joueurRepo;

        public EquipeService(DatabaseContext context)
        {
            _equipeRepo = new EquipeRepository(context);
            _joueurRepo = new JoueurRepository(context);
        }

        public int CreerEquipe(string nom, string tag, DateTime dateCreation, string pays)
        {
            var equipe = new Equipe
            {
                NomEquipe = nom,
                TagEquipe = tag,
                DateCreation = dateCreation,
                Pays = pays
            };

            return _equipeRepo.Add(equipe);
        }

        public void AfficherEquipes()
        {
            var equipes = _equipeRepo.GetAll();

            if (equipes.Count == 0)
            {
                Console.WriteLine("Aucune équipe disponible.");
                return;
            }

            Console.WriteLine("\n=== LISTE DES ÉQUIPES ===");
            foreach (var e in equipes)
            {
                Console.WriteLine($"[{e.IdEquipe}] {e.NomEquipe} ({e.TagEquipe})");
                Console.WriteLine($"    📍 {e.Pays} | Créée le {e.DateCreation:dd/MM/yyyy}");
                
                // Afficher le nombre de joueurs
                var joueurs = _joueurRepo.GetByEquipe(e.IdEquipe);
                Console.WriteLine($"    👥 {joueurs.Count} joueur(s)");
                Console.WriteLine();
            }
        }

        public void AfficherDetailsEquipe(int idEquipe)
        {
            var equipe = _equipeRepo.GetById(idEquipe);
            if (equipe == null)
            {
                Console.WriteLine("❌ Équipe introuvable.");
                return;
            }

            Console.WriteLine($"\n╔════════════════════════════════════════╗");
            Console.WriteLine($"║  {equipe.NomEquipe} ({equipe.TagEquipe})");
            Console.WriteLine($"╚════════════════════════════════════════╝");
            Console.WriteLine($"📍 Pays: {equipe.Pays}");
            Console.WriteLine($"📅 Créée le: {equipe.DateCreation:dd/MM/yyyy}");
            Console.WriteLine($"\n👥 ROSTER:");

            var joueurs = _joueurRepo.GetByEquipe(idEquipe);
            if (joueurs.Count == 0)
            {
                Console.WriteLine("  Aucun joueur dans cette équipe.");
            }
            else
            {
                foreach (var j in joueurs)
                {
                    string statut = j.EstTitulaire ? "⭐" : "🔄";
                    Console.WriteLine($"  {statut} [{j.IdJoueur}] {j.Pseudo} - {j.RoleJeu}");
                    Console.WriteLine($"      {j.Prenom} {j.NomReel}");
                }
            }
        }

        public void SupprimerEquipe(int idEquipe)
        {
            _equipeRepo.Delete(idEquipe);
            Console.WriteLine("✅ Équipe supprimée !");
        }
    }
}