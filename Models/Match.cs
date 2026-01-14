namespace TournamentManager.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int IdTournoi { get; set; }
        public DateTime DateHeure { get; set; }
        public string Phase { get; set; } // "Poules", "Quarts", "Demi", "Finale"
        public string Format { get; set; } // "BO1", "BO3", "BO5"
        public string Statut { get; set; } // "Planifié", "En cours", "Terminé"
        public string Map { get; set; }
    }
}