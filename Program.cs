using TournamentManager.DataAccess;

namespace TournamentManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Tournament Manager ===\n");

            // Test de connexion à la base de données
            var dbContext = new DatabaseContext();
            dbContext.TestConnection();

            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}