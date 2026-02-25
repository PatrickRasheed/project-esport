using TournamentManager.Services;
using TournamentManager.DataAccess;

namespace TournamentManager.UI
{
    public class MenuPrincipal
    {
        private readonly DatabaseContext _context;
        private readonly TournoiService _tournoiService;
        private readonly EquipeService _equipeService;
        private readonly JoueurService _joueurService;
        private readonly MatchService _matchService;
        private readonly ClassementService _classementService;
        private readonly JeuRepository _jeuRepo;

        public MenuPrincipal(DatabaseContext context)
        {
            _context = context;
            _tournoiService = new TournoiService(context);
            _equipeService = new EquipeService(context);
            _joueurService = new JoueurService(context);
            _matchService = new MatchService(context);
            _classementService = new ClassementService(context);
            _jeuRepo = new JeuRepository(context);
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

        // ========================================
        // MENU TOURNOIS
        // ========================================
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
                MenuTournois();
        }

        private void CreerTournoi()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║       ➕ CRÉER UN TOURNOI ➕           ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            // Afficher les jeux disponibles
            var jeux = _jeuRepo.GetAll();
            Console.WriteLine("Jeux disponibles :");
            foreach (var j in jeux)
            {
                Console.WriteLine($"  [{j.IdJeux}] {j.NomJeu}");
            }
            Console.WriteLine();

            Console.Write("Nom du tournoi : ");
            string nom = Console.ReadLine() ?? "Tournoi Test";

            Console.Write($"ID du jeu : ");
            string inputJeu = Console.ReadLine() ?? "";
            int idJeu = string.IsNullOrEmpty(inputJeu) ? 1 : int.Parse(inputJeu);

            Console.Write("Date de début (yyyy-MM-dd) : ");
            string inputDebut = Console.ReadLine() ?? "";
            DateTime dateDebut = string.IsNullOrEmpty(inputDebut) ? DateTime.Now : DateTime.Parse(inputDebut);

            Console.Write("Date de fin (yyyy-MM-dd) : ");
            string inputFin = Console.ReadLine() ?? "";
            DateTime dateFin = string.IsNullOrEmpty(inputFin) ? DateTime.Now.AddDays(7) : DateTime.Parse(inputFin);

            Console.Write("Nombre max d'équipes : ");
            string inputNb = Console.ReadLine() ?? "";
            int nbEquipes = string.IsNullOrEmpty(inputNb) ? 16 : int.Parse(inputNb);

            Console.Write("Format (ex: Simple élimination, Poules + Bracket) : ");
            string format = Console.ReadLine() ?? "Simple élimination";

            Console.Write("Prize Pool ($) : ");
            string inputPrize = Console.ReadLine() ?? "";
            decimal prizePool = string.IsNullOrEmpty(inputPrize) ? 10000 : decimal.Parse(inputPrize);

            try
            {
                int id = _tournoiService.CreerTournoi(nom, idJeu, dateDebut, dateFin, nbEquipes, format, prizePool);
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
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _tournoiService.DemarrerTournoi(id);
            Pause();
        }

        private void TerminerTournoi()
        {
            _tournoiService.AfficherTournois();
            Console.Write("\nID du tournoi à terminer : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _tournoiService.TerminerTournoi(id);
            Pause();
        }

        private void SupprimerTournoi()
        {
            _tournoiService.AfficherTournois();
            Console.Write("\nID du tournoi à supprimer : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

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

        // ========================================
        // MENU ÉQUIPES
        // ========================================
        private void MenuEquipes()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║       👥 GESTION DES ÉQUIPES 👥        ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. ➕ Créer une équipe");
            Console.WriteLine("2. 📋 Afficher toutes les équipes");
            Console.WriteLine("3. 🔍 Voir détails d'une équipe");
            Console.WriteLine("4. 🗑️  Supprimer une équipe");
            Console.WriteLine("0. ↩️  Retour");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Votre choix : ");

            string? choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    CreerEquipe();
                    break;
                case "2":
                    _equipeService.AfficherEquipes();
                    Pause();
                    break;
                case "3":
                    VoirDetailsEquipe();
                    break;
                case "4":
                    SupprimerEquipe();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Choix invalide !");
                    Pause();
                    break;
            }

            if (choix != "0")
                MenuEquipes();
        }

        private void CreerEquipe()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║        ➕ CRÉER UNE ÉQUIPE ➕          ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            Console.Write("Nom de l'équipe : ");
            string nom = Console.ReadLine() ?? "Équipe Test";

            Console.Write("Tag (ex: G2, T1, FNC) : ");
            string tag = Console.ReadLine() ?? "TST";

            Console.Write("Pays : ");
            string pays = Console.ReadLine() ?? "France";

            Console.Write("Date de création (yyyy-MM-dd) : ");
            string inputDate = Console.ReadLine() ?? "";
            DateTime dateCreation = string.IsNullOrEmpty(inputDate) ? DateTime.Now : DateTime.Parse(inputDate);

            try
            {
                int id = _equipeService.CreerEquipe(nom, tag, dateCreation, pays);
                Console.WriteLine($"\n✅ Équipe créée avec succès ! (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Erreur : {ex.Message}");
            }

            Pause();
        }

        private void VoirDetailsEquipe()
        {
            _equipeService.AfficherEquipes();
            Console.Write("\nID de l'équipe : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _equipeService.AfficherDetailsEquipe(id);
            Pause();
        }

        private void SupprimerEquipe()
        {
            _equipeService.AfficherEquipes();
            Console.Write("\nID de l'équipe à supprimer : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            Console.Write("⚠️  Êtes-vous sûr ? (o/n) : ");
            string? confirm = Console.ReadLine();

            if (confirm?.ToLower() == "o")
            {
                try
                {
                    _equipeService.SupprimerEquipe(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erreur : {ex.Message}");
                }
            }

            Pause();
        }

        // ========================================
        // MENU JOUEURS
        // ========================================
        private void MenuJoueurs()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║       🎯 GESTION DES JOUEURS 🎯        ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. ➕ Ajouter un joueur");
            Console.WriteLine("2. 📋 Afficher tous les joueurs");
            Console.WriteLine("3. 🔍 Voir joueurs d'une équipe");
            Console.WriteLine("4. 🗑️  Supprimer un joueur");
            Console.WriteLine("0. ↩️  Retour");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Votre choix : ");

            string? choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AjouterJoueur();
                    break;
                case "2":
                    _joueurService.AfficherJoueurs();
                    Pause();
                    break;
                case "3":
                    VoirJoueursEquipe();
                    break;
                case "4":
                    SupprimerJoueur();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Choix invalide !");
                    Pause();
                    break;
            }

            if (choix != "0")
                MenuJoueurs();
        }

        private void AjouterJoueur()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║       ➕ AJOUTER UN JOUEUR ➕          ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            _equipeService.AfficherEquipes();

            Console.Write("\nID de l'équipe : ");
            string inputEquipe = Console.ReadLine() ?? "0";
            int idEquipe = string.IsNullOrEmpty(inputEquipe) ? 0 : int.Parse(inputEquipe);

            Console.Write("Pseudo : ");
            string pseudo = Console.ReadLine() ?? "Player";

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine() ?? "";

            Console.Write("Nom : ");
            string nom = Console.ReadLine() ?? "";

            Console.Write("Date de naissance (yyyy-MM-dd) : ");
            string inputDate = Console.ReadLine() ?? "";
            DateTime dateNaissance = string.IsNullOrEmpty(inputDate) ? DateTime.Now.AddYears(-20) : DateTime.Parse(inputDate);

            Console.Write("Rôle (ex: Mid Laner, Duelist, etc.) : ");
            string role = Console.ReadLine() ?? "Player";

            Console.Write("Titulaire ? (o/n) : ");
            bool titulaire = Console.ReadLine()?.ToLower() == "o";

            try
            {
                int id = _joueurService.AjouterJoueur(pseudo, nom, prenom, dateNaissance, role, titulaire, idEquipe);
                Console.WriteLine($"\n✅ Joueur ajouté avec succès ! (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Erreur : {ex.Message}");
            }

            Pause();
        }

        private void VoirJoueursEquipe()
        {
            _equipeService.AfficherEquipes();
            Console.Write("\nID de l'équipe : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _joueurService.AfficherJoueursParEquipe(id);
            Pause();
        }

        private void SupprimerJoueur()
        {
            _joueurService.AfficherJoueurs();
            Console.Write("\nID du joueur à supprimer : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            Console.Write("⚠️  Êtes-vous sûr ? (o/n) : ");
            string? confirm = Console.ReadLine();

            if (confirm?.ToLower() == "o")
            {
                try
                {
                    _joueurService.SupprimerJoueur(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erreur : {ex.Message}");
                }
            }

            Pause();
        }

        // ========================================
        // MENU MATCHS
        // ========================================
        private void MenuMatchs()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║        ⚔️  GESTION DES MATCHS ⚔️        ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. ➕ Créer un match");
            Console.WriteLine("2. 📋 Voir matchs d'un tournoi");
            Console.WriteLine("3. ▶️  Démarrer un match");
            Console.WriteLine("4. ✅ Terminer un match");
            Console.WriteLine("0. ↩️  Retour");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Votre choix : ");

            string? choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    CreerMatch();
                    break;
                case "2":
                    VoirMatchsTournoi();
                    break;
                case "3":
                    DemarrerMatch();
                    break;
                case "4":
                    TerminerMatch();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Choix invalide !");
                    Pause();
                    break;
            }

            if (choix != "0")
                MenuMatchs();
        }

        private void CreerMatch()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║         ➕ CRÉER UN MATCH ➕           ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            _tournoiService.AfficherTournois();

            Console.Write("\nID du tournoi : ");
            string inputTournoi = Console.ReadLine() ?? "0";
            int idTournoi = string.IsNullOrEmpty(inputTournoi) ? 0 : int.Parse(inputTournoi);

            Console.Write("Date et heure (yyyy-MM-dd HH:mm) : ");
            string inputDate = Console.ReadLine() ?? "";
            DateTime dateMatch = string.IsNullOrEmpty(inputDate) ? DateTime.Now : DateTime.Parse(inputDate);

            Console.Write("Phase (Poules, Quarts, Demi-finales, Finale) : ");
            string phase = Console.ReadLine() ?? "Poules";

            Console.Write("Format (BO1, BO3, BO5) : ");
            string format = Console.ReadLine() ?? "BO1";

            try
            {
                int id = _matchService.CreerMatch(idTournoi, dateMatch, phase, format);
                Console.WriteLine($"\n✅ Match créé avec succès ! (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Erreur : {ex.Message}");
            }

            Pause();
        }

        private void VoirMatchsTournoi()
        {
            _tournoiService.AfficherTournois();
            Console.Write("\nID du tournoi : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _matchService.AfficherMatchsParTournoi(id);
            Pause();
        }

        private void DemarrerMatch()
        {
            Console.Write("ID du match à démarrer : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _matchService.DemarrerMatch(id);
            Pause();
        }

        private void TerminerMatch()
        {
            Console.Write("ID du match à terminer : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _matchService.TerminerMatch(id);
            Pause();
        }

        // ========================================
        // MENU CLASSEMENTS
        // ========================================
        private void MenuClassements()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║    📊 CLASSEMENTS & STATISTIQUES 📊    ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. 📊 Voir classement d'un tournoi");
            Console.WriteLine("0. ↩️  Retour");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Votre choix : ");

            string? choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    VoirClassement();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Choix invalide !");
                    Pause();
                    break;
            }

            if (choix != "0")
                MenuClassements();
        }

        private void VoirClassement()
        {
            _tournoiService.AfficherTournois();
            Console.Write("\nID du tournoi : ");
            string input = Console.ReadLine() ?? "0";
            int id = string.IsNullOrEmpty(input) ? 0 : int.Parse(input);

            _classementService.AfficherClassement(id);
            Pause();
        }

        // ========================================
        // MENU JEUX
        // ========================================
        private void MenuJeux()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║        🎮 GESTION DES JEUX 🎮          ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. 📋 Afficher tous les jeux");
            Console.WriteLine("0. ↩️  Retour");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Votre choix : ");

            string? choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AfficherJeux();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Choix invalide !");
                    Pause();
                    break;
            }

            if (choix != "0")
                MenuJeux();
        }

        private void AfficherJeux()
        {
            var jeux = _jeuRepo.GetAll();

            Console.WriteLine("\n=== LISTE DES JEUX ===");
            foreach (var j in jeux)
            {
                Console.WriteLine($"[{j.IdJeux}] {j.NomJeu} ({j.AnneeSortie})");
                Console.WriteLine($"    🏢 {j.Editeur} - {j.Genre}");
                Console.WriteLine($"    👥 {j.NbJoueursMinEquipe}-{j.NbJoueursMaxEquipe} joueurs");
                Console.WriteLine($"    📝 {j.Description}");
                Console.WriteLine();
            }
            Pause();
        }

        // ========================================
        // UTILITAIRES
        // ========================================
        private void Pause()
        {
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}