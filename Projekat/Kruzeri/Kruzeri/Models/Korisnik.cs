using System.Globalization;
using System.Text.Json.Serialization;

namespace Kruzeri.Models
{
    public class Korisnik
    {
        public string Id { get; set; }
        public required string Ime { get; set; }
        public required string Prezime { get; set;}
        public required string Email { get; set; }
        public required string Sifra { get; set; }
        public required long Telefon { get; set; }
        public required string DatumRodjenja { get; set; }
        public required string Grad { get; set; }
        public required string Adresa { get; set; }
      
      

    }
}
