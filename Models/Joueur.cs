namespace TournamentManager.Models
{
    public class Joueur
    {
        public int IdJoueur { get; set; } 
        public string Pseudo { get; set; }
        public string NomReel { get; set; }
        public string Prenom { get; set; } 
        public DateTime DateNaissance { get; set; }
        public string RoleJeu { get; set; } 
        public bool EstTitulaire { get; set; }
        public int IdEquipe { get; set; }
    }
}