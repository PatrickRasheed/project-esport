namespace TournamentManager.Models
{
    public class Jeu
    {
        public int IdJeux { get; set; } 
        public string NomJeu { get; set; }  
        public string Editeur { get; set; }
        public string Genre { get; set; }
        public int AnneeSortie { get; set; }
        public string Description { get; set; }
        public int NbJoueursMinEquipe { get; set; }
        public int NbJoueursMaxEquipe { get; set; }
    }
}