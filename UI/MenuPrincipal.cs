using TournamentManager.Services;
using TournamentManager.DataAccess;

namespace TournamentManager.UI
{
    public class MenuPrincipal
    {
        private readonly TournoiService _tournoiService;
        private readonly DatabaseContext _context;

        public MenuPrincipal(DatabaseContext context)
        {
            _context = context;
            _tournoiService = new TournoiService(context);
        }

        public void Afficher()
        {
            bool quitter = false;

            while (!quitter)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║   🎮 TOURNAMENT MANAGER - E-SPORT 🎮   ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("📋 MENU PRINCIPAL");
                Console.WriteLine("─────────────────────────────────────────");
                Console.WriteLine("1. 🏆 Gestion des Tournois");
                Console.WriteLine("2. 👥 Gestion des Équipes");
                Console.WriteLine("3. 🎯 Gestion des Joueurs");
                Console.WriteLine("4. ⚔️  Gestion des Matchs");
                Console.WriteLine("5. 📊 Classements & Statistiques");
                Console.WriteLine("6. 🎮 Gestion des Jeux");
                Console.WriteLine("0. ❌ Quitter");
                Console.WriteLine("─────────────────────────────────────────");
                Console.Write("Votre choix : ");

                string? choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        MenuTournois();
                        break;
                    case "2":
                        MenuEquipes();
                        break;
                    case "3":
                        MenuJoueurs();
                        break;
                    case "4":
                        MenuMatchs();
                        break;
                    case "5":
                        MenuClassements();
                        break;
                    case "6":
                        MenuJeux();
                        break;
                    case "0":
                        quitter = true;
                        Console.WriteLine("\n👋 À bientôt !");
                        break;
                    default:
                        Console.WriteLine("❌ Choix invalide !");
                        Pause();
                        break;
                }
            }
        }

        private void MenuTournois()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║      🏆 GESTION DES TOURNOIS 🏆        ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. ➕ Créer un tournoi");
            Console.WriteLine("2. 📋 Afficher tous les tournois");
            Console.WriteLine("3. ▶️  Démarrer un tournoi");
            Console.WriteLine("4. ✅ Terminer un tournoi");
            Console.WriteLine("5. 🗑️  Supprimer un tournoi");
            Console.WriteLine("0. ↩️  Retour");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Votre choix : ");

            string? choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    CreerTournoi();
                    break;
                case "2":
                    _tournoiService.AfficherTournois();
                    Pause();
                    break;
                case "3":
                    DemarrerTournoi();
                    break;
                case "4":
                    TerminerTournoi();
                    break;
                case "5":
                    SupprimerTournoi();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Choix invalide !");
                    Pause();
                    break;
            }

            if (choix != "0")
                MenuTournois(); // Revenir au menu tournois
        }

        private void CreerTournoi()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║       ➕ CRÉER UN TOURNOI ➕           ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            Console.Write("Nom du tournoi : ");
            string? nom = Console.ReadLine();

            Console.Write("ID du jeu (1=LoL, 2=CS:GO, 3=Valorant) : ");
            int idJeu = int.Parse(Console.ReadLine() ?? "1");

            Console.Write("Date de début (yyyy-MM-dd) : ");
            DateTime dateDebut = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

            Console.Write("Date de fin (yyyy-MM-dd) : ");
            DateTime dateFin = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"));

            Console.Write("Nombre max d'équipes : ");
            int nbEquipes = int.Parse(Console.ReadLine() ?? "16");

            Console.Write("Format (ex: Simple élimination, Poules + Bracket) : ");
            string? format = Console.ReadLine();

            Console.Write("Prize Pool ($) : ");
            decimal prizePool = decimal.Parse(Console.ReadLine() ?? "10000");

            try
            {
                int id = _tournoiService.CreerTournoi(nom!, idJeu, dateDebut, dateFin, nbEquipes, format!, prizePool);
                Console.WriteLine($"\n✅ Tournoi créé avec succès ! (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Erreur : {ex.Message}");
            }

            Pause();
        }

        private void DemarrerTournoi()
        {
            _tournoiService.AfficherTournois();
            Console.Write("\nID du tournoi à démarrer : ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            _tournoiService.DemarrerTournoi(id);
            Pause();
        }

        private void TerminerTournoi()
        {
            _tournoiService.AfficherTournois();
            Console.Write("\nID du tournoi à terminer : ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            _tournoiService.TerminerTournoi(id);
            Pause();
        }

        private void SupprimerTournoi()
        {
            _tournoiService.AfficherTournois();
            Console.Write("\nID du tournoi à supprimer : ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("⚠️  Êtes-vous sûr ? (o/n) : ");
            string? confirm = Console.ReadLine();

            if (confirm?.ToLower() == "o")
            {
                try
                {
                    var repo = new TournoiRepository(_context);
                    repo.Delete(id);
                    Console.WriteLine("✅ Tournoi supprimé !");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erreur : {ex.Message}");
                }
            }

            Pause();
        }

        private void MenuEquipes()
        {
            Console.WriteLine("\n🚧 Menu Équipes - En construction...");
            Pause();
        }

        private void MenuJoueurs()
        {
            Console.WriteLine("\n🚧 Menu Joueurs - En construction...");
            Pause();
        }

        private void MenuMatchs()
        {
            Console.WriteLine("\n🚧 Menu Matchs - En construction...");
            Pause();
        }

        private void MenuClassements()
        {
            Console.WriteLine("\n🚧 Menu Classements - En construction...");
            Pause();
        }

        private void MenuJeux()
        {
            Console.WriteLine("\n🚧 Menu Jeux - En construction...");
            Pause();
        }

        private void Pause()
        {
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}