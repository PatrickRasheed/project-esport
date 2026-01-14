namespace TournamentManager.Models
{
    public class Inscription
    {
        public int IdEquipe { get; set; }
        public int IdTournoi { get; set; }
        public DateTime DateInscription { get; set; }
        public int Seed { get; set; } // Position de départ
        public bool EstValide { get; set; }
    }
}