namespace TournamentManager.Models
{
    public class Inscription
    {
        public int IdInscription { get; set; }  
        public int IdEquipe { get; set; }
        public int IdTournoi { get; set; }
        public DateTime DateInscription { get; set; }
        public int Seed { get; set; } // Position dans le tirage au sort
        public bool EstValide { get; set; }
    }
}