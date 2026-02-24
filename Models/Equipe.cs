namespace TournamentManager.Models
{
    public class Equipe
    {
        public int IdEquipe { get; set; }  
        public string NomEquipe { get; set; }  
        public string TagEquipe { get; set; } 
        public DateTime DateCreation { get; set; }
        public string Pays { get; set; }
        
        public List<Joueur> Joueurs { get; set; } = new List<Joueur>();
    }
}