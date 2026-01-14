namespace TournamentManager.Models
{
    public class Tournoi
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int IdJeu { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NbEquipesMax { get; set; }
        public string Format { get; set; } // "Simple élimination", "Poules + Bracket"
        public decimal PrizePool { get; set; }
        public string Statut { get; set; } // "Ouvert", "En cours", "Terminé"
    }
}