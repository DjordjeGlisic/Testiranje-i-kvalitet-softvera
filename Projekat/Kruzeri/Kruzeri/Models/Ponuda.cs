namespace Kruzeri.Models
{
    public class Ponuda
    {
        public required string Id { get; set; }
        public required string NazivPonude { get; set; }
        public required string GradPolaskaBroda { get; set; }
        public required string NazivAerodroma { get; set; }
        public required string DatumPolaska { get; set; }
        public required string DatumDolaska { get; set; }
        public required int CenaSmestajaBezHrane { get; set; }
        public required int CenaSmestajaSaHranom { get; set; }
      
        public List<string> ListaGradova { get; set; } = new List<string>();
        public  required string  OpisPutovanja { get; set; }
   
        


    }
}
