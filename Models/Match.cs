namespace TournamentManager.Models
{
    public class Match
    {
        public int IdMatch { get; set; }  
        public int IdTournoi { get; set; }
        public DateTime DateMatch { get; set; }  
        public string PhaseTournoi { get; set; }  // Préciser la phase du tournoi (ex: "Quart de finale", "Demi-finale", "Finale")
        public string FormatMatch { get; set; }  // B01, B02, B03, B05, B07
        public string Statut { get; set; }
        
    }
}