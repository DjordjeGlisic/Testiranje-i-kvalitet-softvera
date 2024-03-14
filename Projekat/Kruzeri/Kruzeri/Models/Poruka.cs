namespace Kruzeri.Models
{
    public class Poruka
    {
        public required string  Id { get; set; }
        public required string IdKorisnika { get; set; }
        public required string IdAgencije { get; set; }
        public string Datum { get; set; }
        public string Sadrzaj { get; set; }
        public bool PoslataOdStraneAgencije { get; set; }
        public bool PoslataOdStraneKorisnika { get; set; }
    }
}
