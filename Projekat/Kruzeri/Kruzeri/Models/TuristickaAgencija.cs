using System.Text.Json.Serialization;

namespace Kruzeri.Models
{
    public class TuristickaAgencija
    {
        public required string Id { get; set; }
        public required string Naziv { get; set; }
        public required string Adresa { get; set; }
        public required long Telefon { get; set; }
        public required string Email { get; set; }
        public required string Sifra { get; set; }
        public int ProsecnaOcena { get; set; }
        public int BrojKorisnikaKojiSuOcenili { get; set; }
        [JsonIgnore]
        public List<int> ListaOcenaKorisnika { get; set; } = new List<int>();



    }
}
