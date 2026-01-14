namespace TournamentManager.Models
{
    public class Participant
    {
        public int IdEquipe { get; set; }
        public int IdMatch { get; set; }
        public int NumeroEquipe { get; set; } // 1 ou 2
        public int Score { get; set; }
        public bool EstVainqueur { get; set; }
    }
}