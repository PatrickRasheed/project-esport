namespace TournamentManager.Models
{
    public class Stream
    {
        public int IdStream { get; set; }  
        public int IdMatch { get; set; }
        public string UrlStream { get; set; }  // Changé de Url
        public string Plateform { get; set; }  // Twitch, YouTube, etc.
        public int NbViewersPic { get; set; }  
        public string Langue { get; set; }
        public bool EstOfficiel { get; set; }
    }
}