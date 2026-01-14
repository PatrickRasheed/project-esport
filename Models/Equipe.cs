namespace TournamentManager.Models
{
    public class Equipe
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Tag { get; set; }
        public DateTime DateCreation { get; set; }
        public string Pays { get; set; }
        
        // Navigation (liste des joueurs de cette équipe)
        public List<Joueur> Joueurs { get; set; } = new List<Joueur>();
    }
}