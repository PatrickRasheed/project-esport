namespace TournamentManager.Models
{
    public class Stream
    {
        public int Id { get; set; }
        public int IdMatch { get; set; }
        public string Url { get; set; }
        public string Plateforme { get; set; } // "Twitch", "YouTube"
        public int NbViewers { get; set; }
        public string Langue { get; set; }
        public bool EstOfficiel { get; set; }
    }
}