using TournamentManager.DataAccess;
using TournamentManager.Models;

namespace TournamentManager.Services
{
    public class JoueurService
    {
        private readonly JoueurRepository _joueurRepo;
        private readonly EquipeRepository _equipeRepo;

        public JoueurService(DatabaseContext context)
        {
            _joueurRepo = new JoueurRepository(context);
            _equipeRepo = new EquipeRepository(context);
        }

        public int AjouterJoueur(string pseudo, string nomReel, string prenom, DateTime dateNaissance, 
                                  string role, bool estTitulaire, int idEquipe)
        {
            var joueur = new Joueur
            {
                Pseudo = pseudo,
                NomReel = nomReel,
                Prenom = prenom,
                DateNaissance = dateNaissance,
                RoleJeu = role,
                EstTitulaire = estTitulaire,
                IdEquipe = idEquipe
            };

            return _joueurRepo.Add(joueur);
        }

        public void AfficherJoueurs()
        {
            var joueurs = _joueurRepo.GetAll();

            if (joueurs.Count == 0)
            {
                Console.WriteLine("Aucun joueur disponible.");
                return;
            }

            Console.WriteLine("\n=== LISTE DES JOUEURS ===");
            foreach (var j in joueurs)
            {
                var equipe = _equipeRepo.GetById(j.IdEquipe);
                string equipeNom = equipe != null ? equipe.NomEquipe : "Équipe inconnue";
                string statut = j.EstTitulaire ? "⭐ Titulaire" : "🔄 Remplaçant";

                Console.WriteLine($"[{j.IdJoueur}] {j.Pseudo} - {j.RoleJeu} ({statut})");
                Console.WriteLine($"    {j.Prenom} {j.NomReel}");
                Console.WriteLine($"    🏆 {equipeNom}");
                Console.WriteLine($"    📅 Né le {j.DateNaissance:dd/MM/yyyy} ({CalculerAge(j.DateNaissance)} ans)");
                Console.WriteLine();
            }
        }

        public void AfficherJoueursParEquipe(int idEquipe)
        {
            var equipe = _equipeRepo.GetById(idEquipe);
            if (equipe == null)
            {
                Console.WriteLine("❌ Équipe introuvable.");
                return;
            }

            var joueurs = _joueurRepo.GetByEquipe(idEquipe);

            Console.WriteLine($"\n=== JOUEURS DE {equipe.NomEquipe} ===");
            if (joueurs.Count == 0)
            {
                Console.WriteLine("Aucun joueur dans cette équipe.");
                return;
            }

            foreach (var j in joueurs)
            {
                string statut = j.EstTitulaire ? "⭐ Titulaire" : "🔄 Remplaçant";
                Console.WriteLine($"[{j.IdJoueur}] {j.Pseudo} - {j.RoleJeu} ({statut})");
                Console.WriteLine($"    {j.Prenom} {j.NomReel} - {CalculerAge(j.DateNaissance)} ans");
                Console.WriteLine();
            }
        }

        public void SupprimerJoueur(int idJoueur)
        {
            _joueurRepo.Delete(idJoueur);
            Console.WriteLine("✅ Joueur supprimé !");
        }

        private int CalculerAge(DateTime dateNaissance)
        {
            var aujourdhui = DateTime.Today;
            int age = aujourdhui.Year - dateNaissance.Year;
            if (dateNaissance.Date > aujourdhui.AddYears(-age)) age--;
            return age;
        }
    }
}