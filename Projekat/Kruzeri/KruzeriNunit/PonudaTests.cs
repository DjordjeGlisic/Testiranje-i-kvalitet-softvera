using Kruzeri.Controllers;
using Kruzeri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kruzeri.Controllers.AdministratorController;

namespace KruzeriNunit
{
    [TestFixture]
    public class PonudaTests
    {
        private PonudaController _ponudaController;
        private List<string> gradovi1;
        private List<string> gradovi2;
        private List<string> gradovi3;
        private string ID1=null, ID2=null, ID3=null;
        private string IDAgencije=null;
        private string IDKorisnika = null;
       
        [OneTimeSetUp]
        public async Task Setup()
        {
            // Pravljenje stvarnih implementacija servisa i okoline
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            gradovi1 = new List<string>() { "Venice", "Pisa", "Rome", "Naples", "Bari", "Geona" };
            gradovi2 = new List<string>() { "Barcelona", "Valencia", "Sevilla" };
            gradovi3 = new List<string>() { "Lille", "Touluse", "Bordeaux", "Nice", "Monaco" };
           
            var logger = loggerFactory.CreateLogger<KorisnikController>();
            var neo4jService = Neo4jService.CreateInMemoryService();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // Instanciranje kontrolera sa stvarnim implementacijama servisa i okoline
            _ponudaController = new PonudaController(logger, neo4jService, configuration);
            if (IDAgencije == null)
            {

                var agencija = await neo4jService.DodajAgencijuAsync(new TuristickaAgencija { Naziv = "AgencijaProba", Id = "", Telefon = 1234567890, Adresa = "Agencijska1", BrojKorisnikaKojiSuOcenili = 0, ProsecnaOcena = 0, Email = "agencija@gmail.com", Sifra = "agencija123" });
                IDAgencije = agencija.Id;
            }
            if(ID1==null)
            {
                var ponuda1 = await neo4jService.DodajPonuduAsync(IDAgencije, new Ponuda { NazivPonude="Ponuda1",NazivAerodroma="Aerodrom Nikola Tesla",GradPolaskaBroda="Zadar",DatumPolaska="30.9.2025",DatumDolaska="4.10.2025",CenaSmestajaBezHrane=400,CenaSmestajaSaHranom=800,Id="",ListaGradova=gradovi1,OpisPutovanja="Putovanje jadran"});
                ID1 = ponuda1.Id;
            }
            if (ID2 == null)
            {
                var ponuda2 = await neo4jService.DodajPonuduAsync(IDAgencije, new Ponuda { NazivPonude = "Ponuda2", NazivAerodroma = "Aerodrom Nikola Tesla", GradPolaskaBroda = "Split", DatumPolaska = "11.9.2025", DatumDolaska = "24.10.2025", CenaSmestajaBezHrane = 400, CenaSmestajaSaHranom = 800, Id = "", ListaGradova = gradovi2, OpisPutovanja = "Putovanje Rome" });
                ID2 = ponuda2.Id;
            }
            if (ID3 == null)
            {
                var ponuda3 = await neo4jService.DodajPonuduAsync(IDAgencije, new Ponuda { NazivPonude = "Ponuda3", NazivAerodroma = "Aerodrom Nikola Tesla", GradPolaskaBroda = "Dubrovnik", DatumPolaska = "20.9.2025", DatumDolaska = "27.10.2025", CenaSmestajaBezHrane = 400, CenaSmestajaSaHranom = 800, Id = "", ListaGradova = gradovi3, OpisPutovanja = "Putovanje Croatia" });
                ID3 = ponuda3.Id;
            }
            if (IDKorisnika == null)
            {
                var korisnik = await neo4jService.RegisterAsync(new Korisnik { Ime = "KorinsikProba", Prezime = "Korisnikovic", Email = "korisnikemail@gmail.com", Sifra = "korisnik123", Telefon =1234567890, DatumRodjenja="1.1.2001",Id = "",Grad="Sombor",Adresa="Sopotska,10" });
                IDKorisnika = korisnik.Id;
                await neo4jService.RezervacijaAsync(IDKorisnika, ID1);
            }


        }
        //TESTIRA SE DODAVANJE PONUDE

        [TestCase("Rimska", "Venice", "Aerodrom Marko Polo", "4.2.2024", "7.1.2025", 900, 4500, new string[] { "Venice", "Pisa", "Rome", "Naples", "Bari", "Geona" }, "Veoma lepo putovanje")]
        [TestCase( "Spanska", "Barcelona", "Aerodrom Carlos Puyol", "7.9.2024", "11.9.2024", 250, 700, new string[] { "Barcelona", "Valencia", "Sevilla" }, "Putovanje za sva cula")]
        [TestCase("Francuska", "Lille", "Aerodrom Stade France", "31.8.2024", "20.9.2024", 800, 1800, new string[] { "Lille", "Touluse", "Bordeaux", "Nice", "Monaco" }, "Atlantik i mediteran")]
        public async Task TestDodavanjePonudeReturnsOkObjectResult( string nazivPonude, string gradPolaskaBroda, string nazivAerodreoma, string datumPolaska, string datumDolaska, int cenaSmestajaBezHrane, int cenaSmestajaSaHranom, string[] gradovi, string opisPutovanja)
        {
            List<string> lista = new List<string>();
            foreach (string grad in gradovi)
            {
                lista.Add(grad);
            }
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = nazivPonude,
                NazivAerodroma = nazivAerodreoma,
                GradPolaskaBroda = gradPolaskaBroda,
                DatumPolaska = datumPolaska,
                DatumDolaska = datumDolaska,
                CenaSmestajaBezHrane = cenaSmestajaBezHrane,
                CenaSmestajaSaHranom = cenaSmestajaSaHranom,
                OpisPutovanja = opisPutovanja,
                ListaGradova = lista,
                
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var dodataPonuda = okResult.Value as Ponuda;
           
            Assert.That(dodataPonuda, Is.Not.Null);

            //id
            Assert.That(ponuda.Id, Is.EqualTo(dodataPonuda.Id));
            //naziv
            Assert.That(nazivPonude, Is.EqualTo(dodataPonuda.NazivPonude));
            //polazak
            Assert.That(gradPolaskaBroda, Is.EqualTo(dodataPonuda.GradPolaskaBroda));
            //aerodrom
            Assert.That(nazivAerodreoma, Is.EqualTo(dodataPonuda.NazivAerodroma));
            Assert.IsTrue(nazivAerodreoma.Contains("Aerodrom"));
            //datum polaska
            Assert.That(dodataPonuda.DatumPolaska, Is.Not.EqualTo(dodataPonuda.DatumDolaska));
            Assert.That(datumPolaska, Is.EqualTo(dodataPonuda.DatumPolaska));
            //datum dolaska
            Assert.That(datumDolaska, Is.EqualTo(dodataPonuda.DatumDolaska));
            //cena bez hrane
            Assert.That(cenaSmestajaBezHrane, Is.EqualTo(dodataPonuda.CenaSmestajaBezHrane));
            Assert.That(cenaSmestajaBezHrane, Is.Not.EqualTo(dodataPonuda.CenaSmestajaSaHranom));
            Assert.That(cenaSmestajaBezHrane, Is.GreaterThanOrEqualTo(100));
            //cena sa hranom
            Assert.That(cenaSmestajaSaHranom, Is.EqualTo(dodataPonuda.CenaSmestajaSaHranom));
            Assert.That(dodataPonuda.CenaSmestajaSaHranom, Is.GreaterThanOrEqualTo(200));
            //gradovi
            Assert.IsNotEmpty(dodataPonuda.ListaGradova);
            Assert.That(lista, Is.EqualTo(dodataPonuda.ListaGradova));
            //opis
            Assert.That(opisPutovanja, Is.EqualTo(dodataPonuda.OpisPutovanja));
            Assert.That(dodataPonuda.OpisPutovanja.Length, Is.GreaterThanOrEqualTo(10));
            await _ponudaController.ObrisiPonudu(dodataPonuda.Id);

        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestID()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "2.3.2022",
                DatumDolaska = "4.7.2022",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" },
                
            };
            var result = await _ponudaController.DodajPonudu("1198765432", ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator agencije kojoj se dodaje ponuda mora biti veci od 30 karaktera"));


        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestNull()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "2.3.2022",
                DatumDolaska = "4.7.2022",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" },
            
            };
            var result = await _ponudaController.DodajPonudu("c61604a7-aa51-460b-bb95-b7204d1fe18e", ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Agencija sa zadatim identifikatorom nije pronadjena u bazi"));


        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestAerodrom()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "2.3.2022",
                DatumDolaska = "4.7.2022",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" },
      
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Naziv aerodroma sa koga se polazi na putovanje mora posedovati rec Aerodrom"));


        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestDatumi()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "1.1.2001",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" },
           
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Datum polaska na putovanje mora biti razlicit od datuma dolaska sa putovanja"));


             }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestCene()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 400,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" },
              
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Cena smestaja sa uracunatom hranom mora biti veca od cene smestaja bez uracunate hrane"));


        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestCenaBezHrane()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 70,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" },
                
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Minimalna dozvoljena cena putovanja bez uracunate hrane jeste 100 evra"));


        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestCenaSaHranom()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 170,
                CenaSmestajaSaHranom = 190,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" },
            
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Minimalna dozvoljena cena putovanja sa uracunatom hranom jeste 200 evra"));


        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestGradovi()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 170,
                CenaSmestajaSaHranom = 390,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string>(),
              
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Lista gradova koji se obilaze mora sadrzati makar jedan grad"));


        }
        [Test]
        public async Task TestDodajPonuduReturnsBadRequestOpis()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 170,
                CenaSmestajaSaHranom = 390,
                OpisPutovanja = "opis",
                ListaGradova = new List<string>() { "grad1", "grad2", "grad3" },
               
            };
            var result = await _ponudaController.DodajPonudu(IDAgencije, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Opis putovanja koje nudi agencija mora se opisati upotrebom najmanje 10 karaktera"));


        }
        //TESTIRA SE CITANJE PONUDE
        [Test]
        public async Task TestProveriKorisnikPonudaReturnsOkResult()
        {
            var result = await _ponudaController.PribaviPonuduKorisnika(IDKorisnika, ID1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
           
        }
        [Test]
        public async Task TestProveriKorisnikPonudaReturnsBadReqeustIDKorisnika()
        {
            var result = await _ponudaController.PribaviPonuduKorisnika("IDKorisnika", ID1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator korisnika mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestProveriKorisnikPonudaReturnsBadReqeustIDPonude()
        {
            var result = await _ponudaController.PribaviPonuduKorisnika(IDKorisnika, "ID1");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator ponuda mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestProveriKorisnikPonudaReturnsBadReqeustNullKorisnik()
        {
            var result = await _ponudaController.PribaviPonuduKorisnika("15f82f25-ed74-4feb-9474-413c784169a2", ID1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji korisnik sa zadatim IDem u bazi"));

        }
        [Test]
        public async Task TestProveriKorisnikPonudaReturnsBadReqeustNullAgencija()
        {
            var result = await _ponudaController.PribaviPonuduKorisnika(IDKorisnika,"15f82f25-ed74-4feb-9474-413c784169a2");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji ponuda sa zadatim IDem u bazi"));

        }
        [Test]
        public async Task TestProcitajPonuduReturnsOkObjectResult()
        {

            var result = await _ponudaController.PribaviPonudePrekoID(ID1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var ucitanaPonuda = okResult.Value as Ponuda;
            Assert.That(ucitanaPonuda, Is.Not.Null);
            //id
            Assert.That(ID1, Is.EqualTo(ucitanaPonuda.Id));
            //naziv
            Assert.IsNotNull(ucitanaPonuda.NazivPonude);
            //aerodrom
            Assert.IsNotNull(ucitanaPonuda.NazivAerodroma);
            Assert.That(ucitanaPonuda.NazivAerodroma, Does.Contain("Aerodrom"));
            //grad
            Assert.That(ucitanaPonuda.GradPolaskaBroda, Is.Not.Null);
            //datum polaska
            Assert.That(ucitanaPonuda.DatumPolaska, Is.Not.Null);
            //datum dolaska
            Assert.That(ucitanaPonuda.DatumDolaska, Is.Not.Null);
            Assert.That(ucitanaPonuda.DatumDolaska, Is.Not.EqualTo(ucitanaPonuda.DatumPolaska));
            //cena sa hranom
            Assert.That(ucitanaPonuda.CenaSmestajaSaHranom, Is.GreaterThanOrEqualTo(200));
            //cena bez hrane
            Assert.That(ucitanaPonuda.CenaSmestajaBezHrane, Is.GreaterThanOrEqualTo(100));
            Assert.That(ucitanaPonuda.CenaSmestajaSaHranom, Is.GreaterThan(ucitanaPonuda.CenaSmestajaBezHrane));
            //gradovi
            Assert.That(ucitanaPonuda.ListaGradova, Is.Not.Null);
            Assert.IsNotEmpty(ucitanaPonuda.ListaGradova);
            //opis
            Assert.That(ucitanaPonuda.OpisPutovanja, Is.Not.Null);
            Assert.That(ucitanaPonuda.OpisPutovanja.Length, Is.GreaterThanOrEqualTo(10));








        }
        [Test]
        public async Task TestProcitajPonuduReturnsBadRequestNull()
        {

            var result = await _ponudaController.PribaviPonudePrekoID("c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ponuda sa identifikatorom koji ste prosledili nije pronadjena"));


        }
        [Test]
        public async Task TestProcitajPonuduReturnsBadRequestID()
        {

            var result = await _ponudaController.PribaviPonudePrekoID("1198765432");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator ponude koja se pribavlja mora biti veci od 30 karaktera"));


        }
        //    //TESTIRA SE CITANJE SVIH PONUDA AGENCIJE
       
        public async Task TestProcitajPonudeAgencijeReturnsOkObjectResult()
        {

            var result = await _ponudaController.PribaviSvePonudeAgencije(IDAgencije);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var ucitanaPonuda = okResult.Value as List<Ponuda>;
            Assert.That(ucitanaPonuda, Is.Not.Null);
            //id
            foreach (Ponuda p in ucitanaPonuda)
            {
                //naziv
                Assert.IsNotNull(p.NazivPonude);
                //aerodrom
                Assert.IsNotNull(p.NazivAerodroma);
                Assert.That(p.NazivAerodroma, Does.Contain("Aerodrom"));
                //grad
                Assert.That(p.GradPolaskaBroda, Is.Not.Null);
                //datum polaska
                Assert.That(p.DatumPolaska, Is.Not.Null);
                //datum dolaska
                Assert.That(p.DatumDolaska, Is.Not.Null);
                Assert.That(p.DatumDolaska, Is.Not.EqualTo(p.DatumPolaska));
                //cena sa hranom
                Assert.That(p.CenaSmestajaSaHranom, Is.GreaterThanOrEqualTo(200));
                //cena bez hrane
                Assert.That(p.CenaSmestajaBezHrane, Is.GreaterThanOrEqualTo(100));
                Assert.That(p.CenaSmestajaSaHranom, Is.GreaterThan(p.CenaSmestajaBezHrane));
                //gradovi
                Assert.That(p.ListaGradova, Is.Not.Null);
                Assert.IsNotEmpty(p.ListaGradova);
                //opis
                Assert.That(p.OpisPutovanja, Is.Not.Null);
                Assert.That(p.OpisPutovanja.Length, Is.GreaterThanOrEqualTo(10));
            }









               }
        [Test]
        public async Task TestProcitajPonudeAgencijeReturnsBadRequestNull()
        {

            var result = await _ponudaController.PribaviSvePonudeAgencije("c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji agencija sa zadatim IDem u bazi"));


        }
        [Test]
        public async Task TestProcitajPonudeAgencijeReturnsBadRequestID()
        {

            var result = await _ponudaController.PribaviSvePonudeAgencije("1198765432");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator agencije cije se ponude pribavljaju mora biti veci od 30 karaktera"));


        }
    
  

        [Test]
        public async Task TestProcitajSvePonudeReturnsOkObjectResult()
        {
            var result = await _ponudaController.PribaviSvePonude();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var agencije = okResult.Value as List<Ponuda>;
            Assert.That(agencije, Is.Not.Null.And.Not.Empty);

        }
        //    //TESTIRA SE AZURIRANJE PONUDE
        [TestCase("Italijanska", "Palermo", "Aerodrom Palerma", "11.11.2024", "12.12.2024", 3300, 4500, new string[] { "Palermo", "Callari", "Rome", "Naples", "Geona" }, "Malo da se vidi Italija")]
        [TestCase("Madridska", "Cadiz", "Aerodrom u Kadizu", "9.9.2024", "11.11.2024", 7700, 14700, new string[] { "Cadiz", "Bordeaux", "Nantes" }, "I Spanija i Frrancuska")]
        [TestCase("Pariska", "Maresille", "Aerodrom u Marseju", "10.10.2024", "12.12.2024", 990, 99000, new string[] { "Marseille", "Monaco", "Torino", "Venice", "Dubrovnik" }, "Vise zemalja se obilazi")]
        public async Task TestAzurirajPonuduReturnsOkObjectResult( string nazivPonude, string gradPolaskaBroda, string nazivAerodroma, string datumPolaska, string datumDolaska, int cenaBezHrane, int cenaSaHranom, string[] listaGradova, string opisPutovanja)
        {
            List<string> lista = new List<string>();
            foreach (string grad in listaGradova)
            {
                lista.Add(grad);
            }
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = nazivPonude,
                NazivAerodroma = nazivAerodroma,
                GradPolaskaBroda = gradPolaskaBroda,
                DatumPolaska = datumPolaska,
                DatumDolaska = datumDolaska,
                CenaSmestajaBezHrane = cenaBezHrane,
                CenaSmestajaSaHranom = cenaSaHranom,
                OpisPutovanja = opisPutovanja,
                ListaGradova = lista
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var azuriranaPonuda = okResult.Value as Ponuda;
            Assert.That(azuriranaPonuda, Is.Not.Null);
            //id
            Assert.That(ID2, Is.EqualTo(azuriranaPonuda.Id));
            //naziv
            Assert.That(nazivPonude, Is.EqualTo(azuriranaPonuda.NazivPonude));
            //polazak
            Assert.That(gradPolaskaBroda, Is.EqualTo(azuriranaPonuda.GradPolaskaBroda));
            //aerodrom
            Assert.That(nazivAerodroma, Is.EqualTo(azuriranaPonuda.NazivAerodroma));
            Assert.IsTrue(azuriranaPonuda.NazivAerodroma.Contains("Aerodrom"));
            //datum polaska
            Assert.That(azuriranaPonuda.DatumPolaska, Is.Not.EqualTo(azuriranaPonuda.DatumDolaska));
            Assert.That(datumPolaska, Is.EqualTo(azuriranaPonuda.DatumPolaska));
            //datum dolaska
            Assert.That(datumDolaska, Is.EqualTo(azuriranaPonuda.DatumDolaska));
            //cena bez hrane
            Assert.That(cenaBezHrane, Is.EqualTo(azuriranaPonuda.CenaSmestajaBezHrane));
            Assert.That(cenaBezHrane, Is.Not.EqualTo(azuriranaPonuda.CenaSmestajaSaHranom));
            Assert.That(cenaBezHrane, Is.GreaterThanOrEqualTo(100));
            //cena sa hranom
            Assert.That(cenaSaHranom, Is.EqualTo(azuriranaPonuda.CenaSmestajaSaHranom));
            Assert.That(azuriranaPonuda.CenaSmestajaSaHranom, Is.GreaterThanOrEqualTo(200));
            //gradovi
            Assert.IsNotEmpty(azuriranaPonuda.ListaGradova);
            Assert.That(lista, Is.EqualTo(azuriranaPonuda.ListaGradova));
            //opis
            Assert.That(opisPutovanja, Is.EqualTo(azuriranaPonuda.OpisPutovanja));
            Assert.That(azuriranaPonuda.OpisPutovanja.Length, Is.GreaterThanOrEqualTo(10));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestID()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "1198765432",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "2.3.2022",
                DatumDolaska = "4.7.2022",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu("1198765432", ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator ponude mora biti veci od 30 karaktera"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestNull()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "2.3.2022",
                DatumDolaska = "4.7.2022",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu("c61604a7-aa51-460b-bb95-b7204d1fe18e", ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ponuda sa zadatim identifikatorom nije pronadjena u bazi"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestAerodrom()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = "nazivPonude",
                NazivAerodroma = "nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "2.3.2022",
                DatumDolaska = "4.7.2022",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Naziv aerodroma sa koga se polazi na putovanje mora posedovati rec Aerodrom"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestDatumi()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "1.1.2001",
                CenaSmestajaBezHrane = 300,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Datum polaska na putovanje mora biti razlicit od datuma dolaska sa putovanja"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestCene()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 400,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Cena smestaja sa uracunatom hranom mora biti veca od cene smestaja bez uracunate hrane"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestCenaBezHrane()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 70,
                CenaSmestajaSaHranom = 400,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Minimalna dozvoljena cena putovanja bez uracunate hrane jeste 100 evra"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestCenaSaHranom()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 170,
                CenaSmestajaSaHranom = 190,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string> { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Minimalna dozvoljena cena putovanja sa uracunatom hranom jeste 200 evra"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestGradovi()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 170,
                CenaSmestajaSaHranom = 390,
                OpisPutovanja = "opisPutovanja",
                ListaGradova = new List<string>()
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Lista gradova koji se obilaze mora sadrzati makar jedan grad"));


        }
        [Test]
        public async Task TestAzurirajPonuduReturnsBadRequestOpis()
        {
            Ponuda ponuda = new Ponuda
            {
                Id = ID2,
                NazivPonude = "nazivPonude",
                NazivAerodroma = "Aerodrom nazivAerodreoma",
                GradPolaskaBroda = "gradPolaskaBroda",
                DatumPolaska = "1.1.2001",
                DatumDolaska = "2.1.2001",
                CenaSmestajaBezHrane = 170,
                CenaSmestajaSaHranom = 390,
                OpisPutovanja = "opis",
                ListaGradova = new List<string>() { "grad1", "grad2", "grad3" }
            };
            var result = await _ponudaController.AzurirajPonudu(ID2, ponuda);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Opis putovanja koje nudi agencija mora se opisati upotrebom najmanje 10 karaktera"));


        }
        //    //TESTIRA SE BRISANJE PONUDE
        [Test]
        public async Task TestObrisiPonuduReturnsOkObjectResult()
        {
             var result = await _ponudaController.ObrisiPonudu(ID3);

                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<OkObjectResult>());

                var okResult = result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);

                var poruka = okResult.Value as string;
                Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno obrisana ponuda."));

            
        }
        [Test]
        public async Task TestObrisiPonuduReturnsBadRequestNull()
        {

            var result = await _ponudaController.ObrisiPonudu("c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ponuda sa datim IDem ne postoji u bazi"));


        }
        [Test]
        public async Task TestObrisiPonuduReturnsBadRequestID()
        {

            var result = await _ponudaController.ObrisiPonudu("1198765432");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator ponude koja se brise mora biti veci od 30 karaktera"));


        }
        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _ponudaController.ObrisiPonudu(ID1);
            await _ponudaController.ObrisiPonudu(ID2);
            await _ponudaController.ObrisiPonudu(ID3);
            var neo4jService = Neo4jService.CreateInMemoryService();
            await neo4jService.ObrisiAgencijuAsync(IDAgencije);
            await neo4jService.DeleteUserAsync(IDKorisnika);
        }
        ~PonudaTests() { TearDown(); }
    }
    }
