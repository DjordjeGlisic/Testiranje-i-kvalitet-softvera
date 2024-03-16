using Kruzeri.Controllers;
using Kruzeri.Hubs;
using Kruzeri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kruzeri.Controllers.AdministratorController;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Moq;
namespace KruzeriNunit
{

    [TestFixture]
    public class PorukaTests
    {
        private PorukaController _porukaController;
        private string ID1=null, ID2=null, ID3=null;
        private string IDKorisnika=null;
        private string IDAgencije=null;
        private List<string> IDKreiranih;
        [OneTimeSetUp]
        public async Task Setup()
        {
            // Pravljenje stvarnih implementacija servisa i okoline
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var logger = loggerFactory.CreateLogger<KorisnikController>();
            var neo4jService = Neo4jService.CreateInMemoryService();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var mockHubContext=new Mock<IHubContext<ChatHub>>();

        // Instanciranje kontrolera sa stvarnim implementacijama servisa i okoline
        _porukaController = new PorukaController(logger, neo4jService, configuration,mockHubContext.Object);
            IDKreiranih = new List<string>();
            if(IDKorisnika==null)
            {
                var korisnik1 = await neo4jService.RegisterAsync(new Korisnik { Id = "", Ime = "Marko", Prezime = "Rakic", Email = "markorakic@gmail.com", Sifra = "marko123", Telefon = 3213213210, DatumRodjenja = "7.7.2001", Grad = "Leskovac", Adresa = "RajkovicRajka10" });
                IDKorisnika = korisnik1.Id;
            }
            if(IDAgencije==null)
            {
                var agencija1 = await neo4jService.DodajAgencijuAsync(new TuristickaAgencija { Id = "", Naziv = "AGENCIJA NOVOSADSKA", Adresa = "NOVOSADSKA,40", Email = "agencijanovosadska@gmail.com", Sifra = "agencija123", Telefon = 3213213210, ProsecnaOcena = 0, BrojKorisnikaKojiSuOcenili = 0 });
                IDAgencije = agencija1.Id;
            }
            if(ID1==null)
            {
                var poruka1 = await neo4jService.PosaljiPorukuAsync(new Poruka { Id = "", IdKorisnika = IDKorisnika, IdAgencije = IDAgencije, Sadrzaj = "Poruka 1", Datum = "" });
                ID1 = poruka1.Id;
            }
            if (ID2 == null)
            {
                var poruka2 = await neo4jService.PosaljiPorukuAsync(new Poruka { Id = "", IdKorisnika = IDKorisnika, IdAgencije = IDAgencije, Sadrzaj = "Poruka 2", Datum = "" });
                ID2 = poruka2.Id;
            }
            if (ID3 == null)
            {
                var poruka3 = await neo4jService.PosaljiPorukuAsync(new Poruka { Id = "", IdKorisnika = IDKorisnika, IdAgencije = IDAgencije, Sadrzaj = "Poruka 3", Datum = "" });
                ID3 = poruka3.Id;
            }

        }
        //    //TESTIRA SE DODAVANJE PORUKE
        [TestCase("string", "svidja mi se vasa ponuda", true, false)]
        [TestCase("string", "svidja mi se vasa ponuda veoma", false, true)]
        [TestCase("string", "svidja mi se vasa ponuda bas veoma", true, false)]
        public async Task TestDodajPorukuReturnObjectOkResult(string datum, string sadrzaj, bool poslataOdStraneKorisnika, bool poslataOdStraneAgencije)
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = IDKorisnika,
                IdAgencije = IDAgencije,
                Datum = datum,
                Sadrzaj = sadrzaj,
                PoslataOdStraneKorisnika = poslataOdStraneKorisnika,
                PoslataOdStraneAgencije = poslataOdStraneAgencije

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var dodataPoruka = okResult.Value as Poruka;
            IDKreiranih.Add(dodataPoruka.Id);
            Assert.That(dodataPoruka, Is.Not.Null);
            Assert.That(poruka.Id, Is.EqualTo(dodataPoruka.Id));
            Assert.That(IDKorisnika, Is.EqualTo(dodataPoruka.IdKorisnika));
            Assert.That(IDAgencije, Is.EqualTo(dodataPoruka.IdAgencije));
            Assert.That(sadrzaj, Is.EqualTo(dodataPoruka.Sadrzaj));
            Assert.That(datum, Is.EqualTo(dodataPoruka.Datum));
            Assert.That(poslataOdStraneAgencije, Is.EqualTo(dodataPoruka.PoslataOdStraneAgencije));
            Assert.That(poslataOdStraneKorisnika, Is.EqualTo(dodataPoruka.PoslataOdStraneKorisnika));

        }
        [Test]
        public async Task TestDodajPorukuReturnsBadRequestIDKorisnika()
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = "1498765432",
                IdAgencije = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzaj",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator korisnika koji salje ili prima poruku mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestDodajPorukuReturnsBadRequestIDAgencije()
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                IdAgencije = "1109824307",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzaj",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator agencije koja salje ili prima poruku mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestDodajPorukuReturnsBadRequestIDKorisnikaIDAgencije()
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                IdAgencije = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzaj",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikatori korisnika i agencije ne mogu biti ista vrednost"));

        }
        [Test]
        public async Task TestDodajPorukuReturnsBadRequestSadrzaj()
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                IdAgencije = "c71703b1-aa51-460b-bb95-b7204d1fe18e",
                Datum = "1.1.2001",
                Sadrzaj = "",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruka koja se salje izmedju agencije i korisnika mora sadrzati makar 1 karakter"));

        }
        [Test]
        public async Task TestDodajPorukuReturnsBadRequestPoslata()
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                IdAgencije = "c71703b1-aa51-460b-bb95-b7204d1fe18e",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzaj",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = false

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruku je poslao agencija ili korisnik"));

        }
        [Test]
        public async Task TestDodajPorukuReturnsBadRequestKorisnikNull()
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                IdAgencije = IDAgencije,
                Datum = "1.1.2001",
                Sadrzaj = "sadrzaj",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Korisnik sa prosledjenim IDem nije pronadjen u bazi"));

        }
        [Test]
        public async Task TestDodajPorukuReturnsBadRequestAgencijaNull()
        {
            var poruka = new Poruka
            {
                Id = "",
                IdKorisnika = IDKorisnika,
                IdAgencije = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzaj",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.PosaljiPoruku(poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Agencija sa prosledjenim IDem nije pronadjena u bazi"));

        }
        //    //TESTIRAJU SE CITANJA VEZANA ZA PORUKE
      
     
       
        [Test]
        public async Task TestProcitajKorisnikeSaKojimaKonuniciraAgencijaReturnsOkObjectResult()
        {

            var result = await _porukaController.PribaviKorisnikeAgencije(IDAgencije);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var ucitaniKorisnici = okResult.Value as List<Korisnik>;
            Assert.That(ucitaniKorisnici, Is.Not.Null);
            foreach (var ucitanKorisnik in ucitaniKorisnici)
            {
                Assert.That(ucitanKorisnik, Is.Not.Null);
                Assert.IsNotNull(ucitanKorisnik.Ime);
                Assert.IsNotNull(ucitanKorisnik.Prezime);
                Assert.IsNotNull(ucitanKorisnik.Email);
                Assert.IsTrue(ucitanKorisnik.Email.Contains("@gmail.com"));
                Assert.IsNotNull(ucitanKorisnik.Sifra);
                Assert.That(ucitanKorisnik.Sifra.Length, Is.GreaterThanOrEqualTo(8));
                Assert.That(ucitanKorisnik.Telefon.ToString().Length, Is.EqualTo(10));
                Assert.IsNotNull(ucitanKorisnik.DatumRodjenja);
                Assert.IsNotNull(ucitanKorisnik.Grad);
                Assert.IsNotNull(ucitanKorisnik.Adresa);


            }


            }
        [Test]
        public async Task TestProcitajKorisnikeSaKojimaKomuniciraAgencijaReturnsBadRequestID()
        {

            var result = await _porukaController.PribaviKorisnikeAgencije("1134408132");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator agencije za koju se pribavljaju korisnici sa kojima ta agencija komunicira mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestProcitajKorisikeSaKojimaKomuniciraAgencijaReturnsBadRequestNull()
        {

            var result = await _porukaController.PribaviKorisnikeAgencije("c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Agencija sa zadatim IDem nije pronadjena u bazi"));

        }
        [Test]
        public async Task TestProcitajCetReturnsOkObjectResult()
        {

            var result = await _porukaController.PribaviCet(IDKorisnika, IDAgencije);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var ucitanePoruke = okResult.Value as List<Poruka>;
            Assert.That(ucitanePoruke, Is.Not.Null);
            foreach (var ucitanaPoruka in ucitanePoruke)
            {
                Assert.That(ucitanaPoruka, Is.Not.Null);
                Assert.That(ucitanaPoruka.Id.Length, Is.GreaterThan(35));
                Assert.That(ucitanaPoruka.IdKorisnika, Is.EqualTo(IDKorisnika));
                Assert.That(ucitanaPoruka.IdAgencije, Is.EqualTo(IDAgencije));
                Assert.IsNotNull(ucitanaPoruka.Datum);
                Assert.IsNotNull(ucitanaPoruka.Sadrzaj);


            }


        }
        [Test]
        public async Task TestProcitajCetReturnsBadRequestIDKorisnika()
        {

            var result = await _porukaController.PribaviCet("1134408132", "c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator korisnika mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestProcitajCetReturnsBadRequestIDAgencije()
        {

            var result = await _porukaController.PribaviCet("c61604a7-aa51-460b-bb95-b7204d1fe18e", "1109824307");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator agencije mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestProcitajCetReturnsBadRequestIDKorisnikaIDAgencije()
        {

            var result = await _porukaController.PribaviCet("c61604a7-aa51-460b-bb95-b7204d1fe18e", "c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikatori agencije i korisnika morau biti razlicite vrednosti"));

        }
        [Test]
        public async Task TestProcitajCetReturnsBadRequestNullKorisnik()
        {

            var result = await _porukaController.PribaviCet("c61604a7-aa51-460b-bb95-b7204d1fe18e", IDAgencije);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Korisnik nije pronadjen u bazi"));

        }
        [Test]
        public async Task TestProcitajCetReturnsBadRequestNullAgencija()
        {

            var result = await _porukaController.PribaviCet(IDKorisnika, "c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Agencija nije pronadjena u bazi"));

        }
        [Test]
        public async Task TestProcitajPorukuReturnsOkObjectResult()
        {

            var result = await _porukaController.PribaviPoruku(ID1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var ucitanaPoruka = okResult.Value as Poruka;

            Assert.That(ucitanaPoruka, Is.Not.Null);
            Assert.That(ucitanaPoruka.Id.Length, Is.GreaterThan(30));
            Assert.That(ID1, Is.EqualTo(ucitanaPoruka.Id));
            Assert.That(ucitanaPoruka.IdKorisnika.Length, Is.GreaterThan(30));
            Assert.That(ucitanaPoruka.IdAgencije.Length, Is.GreaterThan(30));
            Assert.IsNotNull(ucitanaPoruka.Datum);
            Assert.IsNotNull(ucitanaPoruka.Sadrzaj);




        }
        [Test]
        public async Task TestProcitajPorukuReturnsBadRequestID()
        {

            var result = await _porukaController.PribaviPoruku("1109824236");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator poruke mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestProcitajPorukuReturnsBadRequestNull()
        {

            var result = await _porukaController.PribaviPoruku("c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruka nije pronadjena u bazi"));

        }
        //    //TESTIRA SE AZURIRANJE PORUKE
        [TestCase("string", "Poruka1 azurirana", true, false)]
        [TestCase("string", "Poruka 2 azurirana", false, true)]
        [TestCase("string", "Poruka 3 azurirana", true, false)]
        public async Task TestAzurirajPorukuReturnObjectOkResult(string datum, string sadrzaj, bool poslataOdStraneKorisnika, bool poslataOdStraneAgencije)
        {
            var poruka = new Poruka
            {
                Id = ID2,
                IdKorisnika = IDKorisnika,
                IdAgencije = IDAgencije,
                Datum = datum,
                Sadrzaj = sadrzaj,
                PoslataOdStraneKorisnika = poslataOdStraneKorisnika,
                PoslataOdStraneAgencije = poslataOdStraneAgencije

            };
            var result = await _porukaController.AzurirajPoruku(ID2, poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var azuriranaPoruka = okResult.Value as Poruka;
            Assert.That(azuriranaPoruka, Is.Not.Null);
            Assert.That(ID2, Is.EqualTo(azuriranaPoruka.Id));
            Assert.That(IDKorisnika, Is.EqualTo(azuriranaPoruka.IdKorisnika));
            Assert.That(IDAgencije, Is.EqualTo(azuriranaPoruka.IdAgencije));
            Assert.That(sadrzaj, Is.EqualTo(azuriranaPoruka.Sadrzaj));
            Assert.That(datum, Is.EqualTo(azuriranaPoruka.Datum));
            Assert.That(poslataOdStraneAgencije, Is.EqualTo(azuriranaPoruka.PoslataOdStraneAgencije));
            Assert.That(poslataOdStraneKorisnika, Is.EqualTo(azuriranaPoruka.PoslataOdStraneKorisnika));

        }
        [Test]
        public async Task TestAzurirajPorukuReturnsBadRequestIDKorisnika()
        {
            var poruka = new Poruka
            {
                Id = ID2,
                IdKorisnika = "1498765432",
                IdAgencije = IDAgencije,
                Datum = "1.1.2001",
                Sadrzaj = "sadrzajNov",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.AzurirajPoruku(ID2, poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator korisnika koji salje ili prima poruku mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestAzurirajPorukuReturnsBadRequestIDPoruke()
        {
            var poruka = new Poruka
            {
                Id = "c61604a7",
                IdKorisnika = IDKorisnika,
                IdAgencije = IDAgencije,
                Datum = "1.1.2001",
                Sadrzaj = "sadrzajNov",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.AzurirajPoruku("c61604a7", poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Idetifikator poruke koja se azurira mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestAzurirajReturnsBadRequestIDAgencije()
        {
            var poruka = new Poruka
            {
                Id = ID2,
                IdKorisnika = IDKorisnika,
                IdAgencije = "c61604a7",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzajNov",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.AzurirajPoruku(ID2, poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator agencije koja salje ili prima poruku mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestAzurirajPorukuReturnsBadRequestIDKorisnikaIDAgencije()
        {
            var poruka = new Poruka
            {
                Id = ID2,
                IdKorisnika = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                IdAgencije = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzajNov",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.AzurirajPoruku(ID2, poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikatori korisnika i agencije ne mogu biti ista vrednost"));

        }
        [Test]
        public async Task TestAzurirajPorukuReturnsBadRequestSadrzaj()
        {
            var poruka = new Poruka
            {
                Id = ID2,
                IdKorisnika = IDKorisnika,
                IdAgencije = IDAgencije,
                Datum = "1.1.2001",
                Sadrzaj = "",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.AzurirajPoruku(ID2, poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruka koja se salje izmedju agencije i korisnika mora sadrzati makar 1 karakter"));

        }
        [Test]
        public async Task TestAzurirajPorukuReturnsBadRequestProcitana()
        {
            var poruka = new Poruka
            {
                Id = ID2,
                IdKorisnika = IDKorisnika,
                IdAgencije = IDAgencije,
                Datum = "1.1.2001",
                Sadrzaj = "sadrzajNov",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = false

            };
            var result = await _porukaController.AzurirajPoruku(ID2, poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruku je poslao agencija ili korisnik"));

        }


        [Test]
        public async Task TestAzurirajPorukuReturnsBadRequestPorukaNull()
        {
            var poruka = new Poruka
            {
                Id = "c61604a7-aa51-460b-bb95-b7204d1fe18e",
                IdKorisnika = IDKorisnika,
                IdAgencije = IDAgencije,
                Datum = "1.1.2001",
                Sadrzaj = "sadrzajNov",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.AzurirajPoruku("c61604a7-aa51-460b-bb95-b7204d1fe18e", poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruka nije pronadjena u bazi"));

        }
          [Test]
        public async Task TestAzurirajPorukuReturnsBadRequestKorisnikIliAgencijaRazliciti()
        {
            var poruka = new Poruka
            {
                Id = ID2,
                IdKorisnika = "c61604a7-aa51-460b-bb95-b7204d1fe28e",
                IdAgencije = "c71704a7-aa51-460b-bb95-b7204d1fe18e",
                Datum = "1.1.2001",
                Sadrzaj = "sadrzaj",
                PoslataOdStraneKorisnika = false,
                PoslataOdStraneAgencije = true

            };
            var result = await _porukaController.AzurirajPoruku(ID2, poruka);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruka mora ostati izmedju iste agencije i istog korisnika"));

        }
        //    //TESTIRA SE BRISANJE PORUKE
       [Test]
        public async Task TestObrisiPorukuReturnsOkObjectResult()
        {
            foreach(var PorukaID in IDKreiranih)
            {
                var result = await _porukaController.ObrisiPoruku(PorukaID);

                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<OkObjectResult>());

                var okResult = result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);

                var poruka = okResult.Value as string;
                Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno obrisana poruka."));

            }
        }
        [Test]
        public async Task TestObrisiPorukuReturnsBadRequestNull()
        {

            var result = await _porukaController.ObrisiPoruku("c61604a7-aa51-460b-bb95-b7204d1fe18e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Poruka sa datim IDem nije pronadjena u bazi"));


        }
        [Test]
        public async Task TestObrisiPorukuReturnsBadRequestID()
        {

            var result = await _porukaController.ObrisiPoruku("1198765432");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID poruke mora biti veci od 30 karaktera"));


        }
        [OneTimeTearDown]
        public async Task OneTimeTearDown() 
        {
            var neo4jService = Neo4jService.CreateInMemoryService();
            await neo4jService.ObrisiPorukuAsync(ID1);
            await neo4jService.ObrisiPorukuAsync(ID2);
            await neo4jService.ObrisiPorukuAsync(ID3);
            await neo4jService.DeleteUserAsync(IDKorisnika);
            await neo4jService.ObrisiAgencijuAsync(IDAgencije);
        }
        ~PorukaTests() { OneTimeTearDown(); }
    }
}
