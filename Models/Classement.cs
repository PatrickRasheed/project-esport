namespace TournamentManager.Models
{
    public class Classement
    {
        public int IdEquipe { get; set; }
        public int IdTournoi { get; set; }
        public int Points { get; set; }
        public int Victoires { get; set; }
        public int Defaites { get; set; }
        public int MatchsNuls { get; set; }
        public int Position { get; set; }
    }
}