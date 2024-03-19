using Kruzeri.Controllers;
using Kruzeri.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using System.Collections.Specialized;
using static Kruzeri.Controllers.AdministratorController;
using static Kruzeri.Controllers.KorisnikController;
using static System.Net.Mime.MediaTypeNames;

namespace KruzeriNunit
{
    [TestFixture]
    public class KorisnikTests
    {
        private KorisnikController _korisnikController;
        private string ID1, ID2, ID3;
        private string IDPonude;
        private string IDAgencije;
        

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

            // Instanciranje kontrolera sa stvarnim implementacijama servisa i okoline
            _korisnikController = new KorisnikController(logger, neo4jService, configuration);
            if(ID1==null)
            {
                var korisnik1 = await neo4jService.RegisterAsync(new Korisnik { Id = "", Ime = "Marko", Prezime = "Rakic", Email = "korisnikmarkorakic@gmail.com", Sifra = "marko123", Telefon = 3213213210, DatumRodjenja = "7.7.2001", Grad = "Leskovac", Adresa = "RajkovicRajka10" });
                ID1 = korisnik1.Id;

            }
            if(ID2==null)
            {
                var korisnik2 = await neo4jService.RegisterAsync(new Korisnik { Id = "", Ime = "Trajko", Prezime = "Trajkovic", Email = "korisniktrajkotrajkovic@gmail.com", Sifra = "trajko123", Telefon = 3213213210, DatumRodjenja = "1.4.2003", Grad = "Leskovac", Adresa = "RajkovicRajka10" });
                ID2 = korisnik2.Id;

            }
            if(ID3==null)
            {
                var korisnik3 = await neo4jService.RegisterAsync(new Korisnik { Id = "", Ime = "Vlajko", Prezime = "Vlajkovic", Email = "korisnikvlajkovlajkovic@gmail.com", Sifra = "vlajko123", Telefon = 3213213210, DatumRodjenja = "27.3.2005", Grad = "Leskovac", Adresa = "TRajkovicRajka10" });
                ID3 = korisnik3.Id;

            }
            var agencija = await neo4jService.DodajAgencijuAsync(new TuristickaAgencija
            {
                Naziv = "AgencijaProba",
                Id = "0",
                Adresa = "Agencijska1",
                Telefon = 3213213210,
                Email = "agencija1@gmail.com",
                Sifra = "agencija123",
                ProsecnaOcena = 0,
                BrojKorisnikaKojiSuOcenili = 0


            });
            IDAgencije = agencija.Id;
            var ponuda = await neo4jService.DodajPonuduAsync(IDAgencije,
                new Ponuda
                {
                    Id = "",
                    NazivPonude = "VeniceTour",
                    GradPolaskaBroda = "Venecija",
                    NazivAerodroma = "Aerodrom Marko Polo",
                    DatumPolaska = "11.4.2021",
                    DatumDolaska = "20.4.2021",
                    CenaSmestajaBezHrane = 400,
                    CenaSmestajaSaHranom = 800,
                    ListaGradova = new List<string>() { "Venice", "Geona", "Bari" },
                    OpisPutovanja = "Dobro putovanje",
                    
                 
                }
                );
            IDPonude = ponuda.Id;
        



        }
       
        //TESTIRA SE DODAVANJE KORISNIKA
        [TestCase("0", "Aleksa", "Lakic", "korisnik1@gmail.com", "aleksic123", 1231231231, "3.8.1992", "Pirot", "Adresovica30")]
        [TestCase("0", "Janko", "Jakic", "korisnik2@gmail.com", "janko123", 7111009201, "2.8.1999", "Nis", "Niska10")]
        [TestCase("0", "Marko", "Markovic", "admin3@gmail.com", "marko123", 1333009201, "1.8.2000", "Beograd", "Beogradska10")]
        public async Task TestDodajKorisnikaReturnObjectOkResult(string Idk, string Imek, string Prezimek, string Emailk, string Sifrak, long Telefonk, string DatumRodjenjak, string Gradk, string Adresak)
        {
            Korisnik korisnik = new Korisnik
            {
                Id = Idk,
                Ime = Imek,
                Prezime = Prezimek,
                Email = Emailk,
                Sifra = Sifrak,
                Telefon = Telefonk,
                DatumRodjenja = DatumRodjenjak,
                Grad = Gradk,
                Adresa = Adresak
            };
            var result = await _korisnikController.RegisterUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var dodatKorisnik = okResult.Value as Korisnik;
            
            Assert.That(dodatKorisnik, Is.Not.Null);

            Assert.That(Imek, Is.EqualTo(dodatKorisnik.Ime));
            Assert.That(Prezimek, Is.EqualTo(dodatKorisnik.Prezime));
            Assert.That(Emailk, Is.EqualTo(dodatKorisnik.Email));
            Assert.IsTrue(Emailk.Contains("@gmail"));
            Assert.That(Sifrak, Is.EqualTo(dodatKorisnik.Sifra));
            Assert.That(Sifrak, Has.Length.GreaterThanOrEqualTo(8));
            Assert.That(Telefonk, Is.EqualTo(dodatKorisnik.Telefon));
            string telefonString = Telefonk.ToString();
            Assert.That(telefonString, Has.Length.EqualTo(10));
            Assert.That(DatumRodjenjak, Is.EqualTo(dodatKorisnik.DatumRodjenja));
            Assert.That(Gradk, Is.EqualTo(dodatKorisnik.Grad));
            Assert.That(Adresak, Is.EqualTo(dodatKorisnik.Adresa));
            await _korisnikController.DeleteUser(dodatKorisnik.Id);
        }
        [Test]
        public async Task TestDodajKorisnikaReturnsBadRequestEmailKorisnika()
        {
            Korisnik korisnik = new Korisnik
            {
                Id = "",
                Ime = "Imek",
                Prezime = "Prezimek",
                Email = "usermarko@gmail.com",
                Sifra = "Sifrak123123",
                Telefon = 1234567890,
                DatumRodjenja = "11.8.2002",
                Grad = "Zajecar",
                Adresa = "Adrea neka"
            };
            var result = await _korisnikController.RegisterUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email korisnika koji se dodaje mora da sadrzi rec korisnik ili admin"));

        }
        [Test]
        public async Task TestDodajKorisnikaObjectErrorResultSifra()
        {
            var korisnik = new Korisnik
            {
                Id = "",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "korisnikimeprezime@gmail.com",
                Sifra = "320a",
                Telefon = 22108130,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.RegisterUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Sifra mora imati bar 8 karaktera"));

        }
        [Test]
        public async Task TestDodajKorisnikaObjectErrorResultEmail()
        {
            var korisnik = new Korisnik
            {
                Id = "",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "korisnikimeprezime@yahoo.com",
                Sifra = "320imeprezime",
                Telefon = 22108130,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.RegisterUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email mora sadrzati @gmail"));

        }
        [Test]
        public async Task TestDodajKorisnikaObjectErrorResultTelefon()
        {
            var korisnik = new Korisnik
            {
                Id = "",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "administratorimeprezime@gmail.com",
                Sifra = "123123ahahaah",
                Telefon = 380980280,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.RegisterUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Broj telefona treba da se sastoji od 10 cifara"));

        }
        //TESTIRA SE CITANJE KORISNIKA
        [TestCase("korisnikmarkorakic@gmail.com", "marko123")]
        
        public async Task TestProcitajKorisnikaReturnsOkObjectResult(string email, string sifra)
        {
            var korisnik = new KorisnikData
            {

                Email = email,
                Sifra = sifra
            };
            var result = await _korisnikController.LoginUser(korisnik);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var ucitanKorisnik = okResult.Value as Korisnik;
            Assert.That(ucitanKorisnik, Is.Not.Null);

            Assert.IsNotNull(ucitanKorisnik.Ime);
            Assert.IsNotNull(ucitanKorisnik.Prezime);
            Assert.That(email, Is.EqualTo(ucitanKorisnik.Email));
            Assert.That(sifra, Is.EqualTo(ucitanKorisnik.Sifra));
            Assert.IsNotNull(ucitanKorisnik.DatumRodjenja);
            Assert.IsNotNull(ucitanKorisnik.Grad);
            Assert.IsNotNull(ucitanKorisnik.Adresa);



        }
        [Test]
        public async Task TestProcitajKorisnikaReturnsBadRequestEmail()
        {
            var korisnik = new KorisnikData
            {
                Email = "korisnikaleksandaraleksandrovic@mail.com",
                Sifra = "1231231231"
            };
            var result = await _korisnikController.LoginUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email mora sadrzati @gmail"));

        }

        [Test]
        public async Task TestProcitajKorisnikaReturnsBadRequestSifra()
        {
            var korisnik = new KorisnikData
            {
                Email = "korisnikmarkomarkovic@gmail.com",
                Sifra = "m123"
            };
            var result = await _korisnikController.LoginUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Sifra mora imati bar 8 karaktera"));

        }
        [Test]
        public async Task TestProcitajKorisnikaReturnsBadRequestNull()
        {
            var korisnik = new KorisnikData
            {
                Email = "korisnikrajkorajkovic@gmail.com",
                Sifra = "rajko123"
            };
            var result = await _korisnikController.LoginUser(korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji korisnik u bazi!"));
        }
        [Test]
        public async Task TestProcitajKorisnikaPrekoIDReturnOkObjectResult()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            var result1 = await _korisnikController.ReadUser(ID1);

            Assert.That(result1, Is.Not.Null);
            Assert.That(result1.Result, Is.TypeOf<OkObjectResult>());

            var okResult1 = result1.Result as OkObjectResult;
            Assert.That(okResult1, Is.Not.Null);

            var ucitanKorisnik1 = okResult1.Value as Korisnik;
            korisnici.Add(ucitanKorisnik1);
            var result2 = await _korisnikController.ReadUser(ID2);

            Assert.That(result2, Is.Not.Null);
            Assert.That(result2.Result, Is.TypeOf<OkObjectResult>());

            var okResult2 = result2.Result as OkObjectResult;
            Assert.That(okResult2, Is.Not.Null);

            var ucitanKorisnik2 = okResult2.Value as Korisnik;
            korisnici.Add(ucitanKorisnik2);
           
            foreach (Korisnik ucitanKorisnik in korisnici)
            {
                Assert.That(ucitanKorisnik, Is.Not.Null);

                Assert.IsNotNull(ucitanKorisnik.Ime);
                Assert.IsNotNull(ucitanKorisnik.Prezime);
                Assert.IsNotNull(ucitanKorisnik.DatumRodjenja);
                Assert.IsNotNull(ucitanKorisnik.Grad);
                Assert.IsNotNull(ucitanKorisnik.Adresa);

            }

        }
        [Test]
        public async Task TestProcitajKorisnikaPrekoIDReturnsBadRequestID()
        {

            var result = await _korisnikController.ReadUser("123");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestProcitajKorisnikaPrekoIDReturnsBadRequestNull()
        {

            var result = await _korisnikController.ReadUser("12345678912345678912345678912331");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji korisnik u bazi!"));

        }
        //TESTIRA SE AZURIRANJE KORISNIKA
       
        [TestCase("Zlatko", "Zlatkovic", "korisnikzlatkozlatkovic@gmail.com", "zlatko123", 4312210810, "23.11.1970", "Zajecar", "Zajecarska10")]
        public async Task TestAzurirajKorisnikaReturnsOkObjectResult(string ime, string prezime, string email, string sifra, long telefon, string datum, string grad, string adresa)
        {
            Korisnik korisnik = new Korisnik
            {
                Id = ID2,
                Ime = ime,
                Prezime = prezime,
                Email = email,
                Sifra = sifra,
                Telefon = telefon,
                DatumRodjenja = datum,
                Grad = grad,
                Adresa = adresa
            };
            var result = await _korisnikController.UpdateUser(ID2, korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var azuriranKorisnik = okResult.Value as Korisnik;
            Assert.That(azuriranKorisnik, Is.Not.Null);

            Assert.That(ime, Is.EqualTo(azuriranKorisnik.Ime));
            Assert.That(prezime, Is.EqualTo(azuriranKorisnik.Prezime));
            Assert.That(email, Is.EqualTo(azuriranKorisnik.Email));
            Assert.IsTrue(email.Contains("@gmail"));
            Assert.That(sifra, Is.EqualTo(azuriranKorisnik.Sifra));
            Assert.That(sifra, Has.Length.GreaterThanOrEqualTo(8));
            Assert.That(telefon, Is.EqualTo(azuriranKorisnik.Telefon));
            string telefonString = telefon.ToString();
            Assert.That(telefonString, Has.Length.EqualTo(10));
            Assert.That(datum, Is.EqualTo(azuriranKorisnik.DatumRodjenja));
            Assert.That(grad, Is.EqualTo(azuriranKorisnik.Grad));
            Assert.That(adresa, Is.EqualTo(azuriranKorisnik.Adresa));


        }

        [Test]
        public async Task TestAzurirajKorisnikaReturnsBadRequestEmailKorisnika()
        {
            Korisnik korisnik = new Korisnik
            {
                Id = "",
                Ime = "Imek",
                Prezime = "Prezimek",
                Email = "usermarko@gmail.com",
                Sifra = "Sifrak123123",
                Telefon = 1234567890,
                DatumRodjenja = "11.8.2002",
                Grad = "Zajecar",
                Adresa = "Adrea neka"
            };
            var result = await _korisnikController.UpdateUser(ID2,korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email korisnika koji se azurira mora da sadrzi rec korisnik ili admin"));

        }
        [Test]
        public async Task TestAzurirajKorisnikaObjectErrorResultSifra()
        {
            var korisnik = new Korisnik
            {
                Id = "170956528517095652851709565285",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "korisnikimeprezime@gmail.com",
                Sifra = "320a",
                Telefon = 22108130,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.UpdateUser("170956528517095652851709565285", korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Sifra mora imati bar 8 karaktera"));

        }
        [Test]
        public async Task TestAzurirajKorisnikaObjectErrorResultEmail()
        {
            var korisnik = new Korisnik
            {
                Id = "170956528517095652851709565285",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "korisnikimeprezime@yahoo.com",
                Sifra = "320imeprezime",
                Telefon = 22108130,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.UpdateUser("170956528517095652851709565285", korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email mora sadrzati @gmail"));

        }
        [Test]
        public async Task TestAzurirajKorisnikaObjectErrorResultTelefon()
        {
            var korisnik = new Korisnik
            {
                Id = "170956528517095652851709565285",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "korisnikimeprezime@gmail.com",
                Sifra = "123123ahahaah",
                Telefon = 380980280,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.UpdateUser("170956528517095652851709565285", korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Broj telefona treba da se sastoji od 10 cifara"));

        }
        [Test]
        public async Task TestAzurirajKorisnikaObjectErrorResultNull()
        {
            var korisnik = new Korisnik
            {
                Id = "170956528517095652851709565285",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "korisnikimeprezime@gmail.com",
                Sifra = "123123ahahaah",
                Telefon = 1234567891,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.UpdateUser("170956528517095652851709565285", korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Korisnik nije pronadjen u bazi"));

        }
        [Test]
        public async Task TestAzurirajKorisnikaObjectErrorResultID()
        {
            var korisnik = new Korisnik
            {
                Id = "1798765432",
                Ime = "Ime",
                Prezime = "Prezime",
                Email = "korisnikimeprezime@gmail.com",
                Sifra = "123123ahahaah",
                Telefon = 1234567891,
                DatumRodjenja = "2.1.1981",
                Grad = "Nis",
                Adresa = "Niska"
            };
            var result = await _korisnikController.UpdateUser("1798765432", korisnik);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID korisnika mora biti veci od 30 karaktera"));

        }

        [Test]
        public async Task TestRezervisiPonuduReturnsOkObjectResult()
        {

            var result = await _korisnikController.RezervisiPonudu(ID1, IDPonude);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var poruka = okResult.Value as string;
            Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno rezervisana ponuda"));
            result = await _korisnikController.RezervisiPonudu(ID2, IDPonude);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            poruka = okResult.Value as string;
            Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno rezervisana ponuda"));
          

        }
        [Test]
        public async Task TestRezervisiPonuduReturnsBadRequestIDKorisnika()
        {
            var result = await _korisnikController.RezervisiPonudu("-1", "12345678911234567891123456789000");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID korisnika mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestRezervisiPonuduReturnsBadRequestIDPonude()
        {
            var result = await _korisnikController.RezervisiPonudu("12345678911234567891123456789000", "-1");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID ponude mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestRezervisiPonuduReturnsBadRequestIDKorisnikaIDPonude()
        {
            var result = await _korisnikController.RezervisiPonudu("12345678911234567891123456789000", "12345678911234567891123456789000");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID korisnika ne moze biti jednak ID ponude"));

        }

        [Test]
        public async Task TestOtkaziRezervacijuReturnsOkObjectResult()
        {

            var result = await _korisnikController.OtkaziRezervaciu(ID1, IDPonude);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var poruka = okResult.Value as string;
            Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno otkazana rezervacija"));
            result = await _korisnikController.OtkaziRezervaciu(ID2, IDPonude);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            poruka = okResult.Value as string;
            Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno otkazana rezervacija"));


        }
        [Test]
        public async Task TestOtkaziRezervacijuReturnsBadRequestIDKorisnika()
        {
            var result = await _korisnikController.OtkaziRezervaciu("f2f260fc-9003-4917-80dc", IDPonude);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID korisnika mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestOtkaziRezervacijuReturnsBadRequestIDPonude()
        {
            var result = await _korisnikController.OtkaziRezervaciu(ID1, "f2f260fc-9003-4917-80dc");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID ponude mora biti veci od 30 karaktera"));

        }
        [Test]
        public async Task TestOtkaziRezervacijuReturnsBadRequestIDKorisnikaIDPonude()
        {
            var result = await _korisnikController.OtkaziRezervaciu("c3528b82 - f33e - 4601 - 8f57 - e50accfac83e", "c3528b82 - f33e - 4601 - 8f57 - e50accfac83e");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID korisnika ne moze biti jednak ID ponude"));

        }
        //    TESTIRA SE BRISANJE KORISNIKA
        [Test]
        public async Task TestObrisiKorisnikaReturnsOkObjectResult()
        {
          
                var result = await _korisnikController.DeleteUser(ID3);

                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<OkObjectResult>());

                var okResult = result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);

                var poruka = okResult.Value as string;
                Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno obrisan korisnik."));

            

        }
        [Test]
        public async Task TestObrisiKorisnikaReturnsBadRequestNull()
        {

            var result = await _korisnikController.DeleteUser("02f260fc - 9003 - 4917 - 80dc - 6d317e556548");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji korisnik u bazi"));


        }
        [Test]
        public async Task TestObrisiKorisnikaReturnsBadRequestID()
        {

            var result = await _korisnikController.DeleteUser("f2f260fc - 9003 - 4917 - 80dc");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID mora biti veci od 30 karaktera"));


        }
        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _korisnikController.OtkaziRezervaciu(ID1, IDPonude);
            await _korisnikController.OtkaziRezervaciu(ID2, IDPonude);
            await _korisnikController.OtkaziRezervaciu(ID3, IDPonude);
            var neo4jService = Neo4jService.CreateInMemoryService();
            await neo4jService.ObrisiPonuduAsync(IDPonude);
            await neo4jService.ObrisiAgencijuAsync(IDAgencije);
            await _korisnikController.DeleteUser(ID1);
            await _korisnikController.DeleteUser(ID2);
            await _korisnikController.DeleteUser(ID3);





        }
        ~KorisnikTests() { TearDown(); }
    }
}