namespace TournamentManager.Models
{
    public class Joueur
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string NomReel { get; set; }
        public string Email { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Role { get; set; }
        public bool EstTitulaire { get; set; }
        
        // Clé étrangère
        public int IdEquipe { get; set; }
    }
}