using TournamentManager.DataAccess;
using TournamentManager.UI;

namespace TournamentManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║   🎮 TOURNAMENT MANAGER - E-SPORT 🎮   ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();

            // Test de connexion à la base de données
            var dbContext = new DatabaseContext();
            
            Console.Write("🔄 Connexion à la base de données... ");
            if (dbContext.TestConnection())
            {
                Console.WriteLine("\n");
                
                // Lancer le menu principal
                var menu = new MenuPrincipal(dbContext);
                menu.Afficher();
            }
            else
            {
                Console.WriteLine("\n❌ Impossible de se connecter à la base de données.");
                Console.WriteLine("Vérifiez votre fichier appsettings.json");
            }
        }
    }
}