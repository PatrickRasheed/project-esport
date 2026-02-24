namespace TournamentManager.Models
{
    public class Tournoi
    {
        public int IdTournoi { get; set; }  // Changé de Id
        public string NomTournoi { get; set; }  // Changé de Nom
        public int IdJeux { get; set; }  // Changé de IdJeu
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NombreEquipesMax { get; set; }  // Changé de NbEquipesMax
        public string Format { get; set; }
        public decimal PrizePool { get; set; }
        public string Statut { get; set; }
    }
}