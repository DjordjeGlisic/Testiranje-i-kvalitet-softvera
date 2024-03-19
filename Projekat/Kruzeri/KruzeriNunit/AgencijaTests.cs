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
using static Kruzeri.Controllers.KorisnikController;

namespace KruzeriNunit
{
    
    [TestFixture]
    public class AgencijaTests
    {
        private AdministratorController _administratorController;

        private string ID1 = null, ID2 = null, ID3 = null;
       

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
                _administratorController = new AdministratorController(logger, neo4jService, configuration);
                if(ID1==null)
                {
                    var agencija1 = await neo4jService.DodajAgencijuAsync(new TuristickaAgencija { Id = "", Naziv = "AGENCIJA NOVOSADSKA", Adresa = "NOVOSADSKA,40", Email = "agencijanovosadska@gmail.com", Sifra = "agencija123", Telefon = 3213213210, ProsecnaOcena = 0, BrojKorisnikaKojiSuOcenili = 0 });
                    ID1 = agencija1.Id;

                }
                if(ID2==null)
                {
                    var agencija2 = await neo4jService.DodajAgencijuAsync(new TuristickaAgencija { Id = "", Naziv = "AGENCIJA KIKINDSKA", Adresa = "KIKINDSKA,40", Email = "agencijakikindksa@gmail.com", Sifra = "agencija123", Telefon = 3213213210, ProsecnaOcena = 0, BrojKorisnikaKojiSuOcenili = 0 });
                    ID2 = agencija2.Id;

                }
               if(ID3==null)
                {
                    var agencija3 = await neo4jService.DodajAgencijuAsync(new TuristickaAgencija { Id = "", Naziv = "AGENCIJA SUBOTICKA ", Adresa = "SUBOTICKA,40", Email = "agencijasubotika@gmail.com", Sifra = "agencija123", Telefon = 3213213210, ProsecnaOcena = 0, BrojKorisnikaKojiSuOcenili = 0 });
                    ID3 = agencija3.Id;
                }

              
                
            }

            
        
        // public AgencijaTests() { Setup(); }
        //TESTIRA SE DODAVANJE AGENCIJE
        [TestCase("0", "BigStarTravell", "Niska18", 3120212102, "agencijabigstartravell@gmail.com", "bigstar123", 7),Order(1)]
        [TestCase("0", "BeogradAgency", "Beogradska18", 9419449390, "agencijabeogradska@gmail.com", "boegradksa123", 10)]
        [TestCase("0", "NoviSadAgency", "Novosadska18", 4332331330, "agencijanovosadska@gmail.com", "novisad123", 7)]
        public async Task TestDodajAgencijuReturnObjectOkResult(string id, string naziv, string adresa, long telefon, string email, string pass, int rating)
        {
            TuristickaAgencija agencija = new TuristickaAgencija
            {
                Id = id,
                Naziv = naziv,
                Adresa = adresa,
                Telefon = telefon,
                Email = email,
                Sifra = pass,
                ProsecnaOcena = rating,
                BrojKorisnikaKojiSuOcenili = 10,

            };
            var result = await _administratorController.DodajAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var dodataAgencija = okResult.Value as TuristickaAgencija;
            string IDKreirane=dodataAgencija.Id;
            Assert.That(dodataAgencija, Is.Not.Null);
            Assert.That(id, Is.Not.EqualTo(dodataAgencija.Id));
            Assert.That(naziv, Is.EqualTo(dodataAgencija.Naziv));
            Assert.That(adresa, Is.EqualTo(dodataAgencija.Adresa));
            Assert.That(telefon, Is.EqualTo(dodataAgencija.Telefon));
            string telefonString = telefon.ToString();
            Assert.IsTrue(email.Contains("@gmail.com"));
            Assert.That(pass, Is.EqualTo(dodataAgencija.Sifra));
            Assert.That(pass, Has.Length.GreaterThanOrEqualTo(8));
            Assert.That(telefonString, Has.Length.EqualTo(10));
            Assert.That(dodataAgencija.ProsecnaOcena, Is.EqualTo(0));
            Assert.That(dodataAgencija.BrojKorisnikaKojiSuOcenili, Is.EqualTo(0));
            Assert.That(dodataAgencija.ListaOcenaKorisnika, Has.Count.EqualTo(0));
            await _administratorController.ObrisiAgenciju(IDKreirane);
            
        }
        [Test]
        public async Task TestDodajAgencijuReturnsBadRequestEmailAgencija()
        {
            var agencija = new TuristickaAgencija
            {
                Id = "",
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 3213213210,
                Email = "imeprezime@gmail.com",
                Sifra = "320adasdasdas",
                ProsecnaOcena = 10,
                BrojKorisnikaKojiSuOcenili = 10
            };
            var result = await _administratorController.DodajAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email agencije koja se dodaje mora da sadrzi rec agencija"));

        }
        [Test]
        public async Task TestDodajAgencijuReturnsBadRequestSifra()
        {
            var agencija = new TuristickaAgencija
            {
                Id = "",
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 3213213210,
                Email = "imeprezime@gmail.com",
                Sifra = "320a",
                ProsecnaOcena = 10,
                BrojKorisnikaKojiSuOcenili = 10
            };
            var result = await _administratorController.DodajAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Sifra mora imati najmanje 8 karaktera"));

        }
        [Test]
        public async Task TestDodajAgencijuReturnsBadRequestEmail()
        {
            var agencija = new TuristickaAgencija
            {
                Id = "",
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 3213213210,
                Email = "imeprezime@g.com",
                Sifra = "naziv123",
                ProsecnaOcena = 10,
                BrojKorisnikaKojiSuOcenili = 10
            };
            var result = await _administratorController.DodajAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email mora sadrzati karaktere @gmail.com"));

        }
        [Test]
        public async Task TestDodajAgencijuReturnsBadRequestTelefon()
        {
            var agencija = new TuristickaAgencija
            {
                Id = "",
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 32132132,
                Email = "imeprezime@gmail.com",
                Sifra = "naziv123",
                ProsecnaOcena = 10,
                BrojKorisnikaKojiSuOcenili = 10
            };
            var result = await _administratorController.DodajAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Telefon mora sadrzati tacno 10 cifre"));

        }
        //TESTIRA SE CITANJE AGENCIJE
        [TestCase("agencijanovosadska@gmail.com", "agencija123")]
      
        public async Task TestProcitajAgencijuReturnsOkObjectResult(string email, string sifra)
        {
            var agencija = new AgencijaData
            {

                Email = email,
                Sifra = sifra
            };
            var result = await _administratorController.PribaviAgenciju(agencija);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var ucitanaAgencija = okResult.Value as TuristickaAgencija;
            Assert.That(ucitanaAgencija, Is.Not.Null);

            Assert.IsNotNull(ucitanaAgencija.Naziv);
            Assert.IsNotNull(ucitanaAgencija.Adresa);
            Assert.That(email, Is.EqualTo(ucitanaAgencija.Email));
            Assert.That(sifra, Is.EqualTo(ucitanaAgencija.Sifra));
            Assert.IsNotNull(ucitanaAgencija.ListaOcenaKorisnika);
            Assert.That(ucitanaAgencija.Id.Length, Is.GreaterThan(30));



        }
        [Test]
        public async Task TestProcitajAgencijuReturnsBadRequestEmail()
        {
            var agencija = new AgencijaData
            {
                Email = "novosadska@gmail",
                Sifra = "novisad123"
            };
            var result = await _administratorController.PribaviAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email mora sadrzati karaktere @gmail.com"));

        }

        [Test]
        public async Task TestProcitajAgencijuReturnsBadRequestSifra()
        {
            var agencija = new AgencijaData
            {
                Email = "novosadska@gmail",
                Sifra = "novisad"
            };
            var result = await _administratorController.PribaviAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Sifra mora imati najmanje 8 karaktera"));

        }
        [Test]
        public async Task TestProcitajAgencijuReturnsBadRequestNull()
        {
            var agencija = new AgencijaData
            {
                Email = "rajkorajkovic@gmail.com",
                Sifra = "rajko123"
            };
            var result = await _administratorController.PribaviAgenciju(agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji agencija u bazi!"));
        }
        [Test]
        public async Task TestProcitajSveAgencijeReturnsOkObjectResult()
        {
            var result = await _administratorController.PribaviAgencije();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var agencije = okResult.Value as List<TuristickaAgencija>;
            Assert.That(agencije, Is.Not.Null.And.Not.Empty);

        }
        [Test]
        public async Task TestProcitajAgencijuPoIDReturnsOkObjectResult()
        {
          
            


                var result = await _administratorController.PribaviAgencijuPoID(ID1);

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.TypeOf<OkObjectResult> ());

                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);

                var ucitanaAgencija = okResult.Value as TuristickaAgencija;
                Assert.That(ucitanaAgencija, Is.Not.Null);

                Assert.IsNotNull(ucitanaAgencija.Naziv);
                Assert.IsNotNull(ucitanaAgencija.Adresa);
                Assert.IsNotNull(ucitanaAgencija.ListaOcenaKorisnika);
                Assert.That(ucitanaAgencija.Id, Is.EqualTo(ID1));
               
                result = await _administratorController.PribaviAgencijuPoID(ID2);

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

                okResult = result.Result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);

                ucitanaAgencija = okResult.Value as TuristickaAgencija;
                Assert.That(ucitanaAgencija, Is.Not.Null);

                Assert.IsNotNull(ucitanaAgencija.Naziv);
                Assert.IsNotNull(ucitanaAgencija.Adresa);
                Assert.IsNotNull(ucitanaAgencija.ListaOcenaKorisnika);
                Assert.That(ucitanaAgencija.Id, Is.EqualTo(ID2));
               
              
            

            }
        [Test]
        public async Task TestProciatajAgencijuPoIDReturnsBadRequestID()
        {

            var result = await _administratorController.PribaviAgencijuPoID("dc0a-4cef-be57-8904daedcc81");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator mora biti veci od 30 karaktera"));
        }
        [Test, Order(9)]
        public async Task TestProciatajAgencijuPoIDReturnsBadRequestNull()
        {

            var result = await _administratorController.PribaviAgencijuPoID("dc0a-4cef-be57-8904daedcc81-123456789");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ne postoji agencija sa prosledjenim IDem u bazi!"));
        }
        //   TESTIRA SE AZURIRANJE AGENCIJE
        
        [TestCase("Agencija1", "Vojvode Putnika3", 3412218201, "agencija1@gmail.com", "agencija123")]
     
        public async Task TestAzurirajAgencijuReturnsOkObjectResult(string naziv, string adresa, long telefon, string email, string sifra)
        {
            TuristickaAgencijaAzur agencija = new TuristickaAgencijaAzur
            {
                Id = ID2,
                Naziv = naziv,
                Adresa = adresa,
                Email = email,
                Sifra = sifra,
                Telefon = telefon,

            };
            var result = await _administratorController.AzurirajAgenciju(ID2, agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var azuriranaAgencija = okResult.Value as TuristickaAgencija;
            Assert.That(azuriranaAgencija, Is.Not.Null);

            Assert.That(naziv, Is.EqualTo(azuriranaAgencija.Naziv));
            Assert.That(adresa, Is.EqualTo(azuriranaAgencija.Adresa));
            Assert.That(email, Is.EqualTo(azuriranaAgencija.Email));
            Assert.IsTrue(email.Contains("@gmail"));
            Assert.That(sifra, Is.EqualTo(azuriranaAgencija.Sifra));
            Assert.That(sifra, Has.Length.GreaterThanOrEqualTo(8));
            Assert.That(telefon, Is.EqualTo(azuriranaAgencija.Telefon));
            string telefonString = telefon.ToString();
            Assert.That(telefonString, Has.Length.EqualTo(10));
            Assert.That(ID2, Is.EqualTo(azuriranaAgencija.Id));
            Assert.That(azuriranaAgencija.ListaOcenaKorisnika, Is.Not.Null);

         

           


        }
        [Test]
        public async Task TestAzurirajAgencijuReturnsBadRequestEmailAgencija()
        {
            var agencija = new TuristickaAgencijaAzur
            {
                Id =ID2,
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 1234567890,
                Email = "imeprezime@gmail.com",
                Sifra = "320aasdasdsa",
              
            };
            var result = await _administratorController.AzurirajAgenciju(ID2,agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Email agencije koja se azurira mora da sadrzi rec agencija"));

        }

        [Test]
        public async Task TestAzurirajAgencijuReturnsBadRequestSifra()
        {
            var agencija = new TuristickaAgencijaAzur
            {
                Id = ID1,
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 3213213210,
                Email = "agencijaagencija@gmail.com",
                Sifra = "agencij"
            };
            var result = await _administratorController.AzurirajAgenciju(ID1, agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Nova sifra agencije mora sadrzati makar 8 karaktera"));

        }
        [Test]
        public async Task TestAzurirajAgencijuReturnsBadRequestEmail()
        {
            var agencija = new TuristickaAgencijaAzur
            {
                Id = ID1,
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 3213213210,
                Email = "agencijaagencija@gmail",
                Sifra = "agencija123"
            };
            var result = await _administratorController.AzurirajAgenciju(ID1, agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Novi email agencije mora sadrzati karaktere @gmail.com"));


        }
        [Test]
        public async Task TestAzurirajAgencijuReturnsBadRequestTelefon()
        {
            var agencija = new TuristickaAgencijaAzur
            {
                Id = ID1,
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 32132,
                Email = "agencijaagencija@gmail.com",
                Sifra = "agencija123"
            };
            var result = await _administratorController.AzurirajAgenciju(ID1, agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Novi broj telefona agencije mora sadrzati tacno 10 cifara"));



        }
        [Test]
        public async Task TestAzurirajAgencijuReturnsBadRequestID()
        {
            var agencija = new TuristickaAgencijaAzur
            {
                Id = "dc0a-4cef-be57-8904daedcc81",
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 3213213210,
                Email = "agencijaagencija@gmail",
                Sifra = "agencija123"
            };
            var result = await _administratorController.AzurirajAgenciju("dc0a-4cef-be57-8904daedcc81", agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID agencije mora biti veci od 30 karaktera"));



        }
        
        [Test]
        public async Task TestAzurirajAgencijuObjectErrorResultNull()
        {
            var agencija = new TuristickaAgencijaAzur
            {
                Id = "12345678-dc0a-4cef-be57-8904daedcc81",
                Naziv = "Naziv",
                Adresa = "Adresa",
                Telefon = 3213213210,
                Email = "agencijaagencija@gmail.com",
                Sifra = "agencija123"
            };
            var result = await _administratorController.AzurirajAgenciju("12345678-dc0a-4cef-be57-8904daedcc81", agencija);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Agencija sa zadatim ID ne postoji u bazi"));

        }

        //TESTIRA SE DODAVANJE OCENE AGENCIJI
        [TestCase( 3)]
        [TestCase(4)]
        [TestCase(5)]
        public async Task TestDodajOcenuAgencijiReturnsOkObjectResult(int ocena)
        {
            var provera = await _administratorController.PribaviAgencijuPoID(ID1);
            var result = await _administratorController.KorisnikDajeOcenuAgenciji(ID1, ocena);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(provera, Is.Not.Null);
            Assert.That(provera.Result, Is.TypeOf<OkObjectResult>());

            var okResultProvera = provera.Result as OkObjectResult;
            Assert.That(okResultProvera, Is.Not.Null);
            var prethodnaVerzija = okResultProvera.Value as TuristickaAgencija;
            var preIposle = okResult.Value as OcenaPosle;
            Assert.That(preIposle, Is.Not.Null);
            Assert.That(ID1, Is.EqualTo(prethodnaVerzija.Id));
            Assert.That(ID1, Is.EqualTo(preIposle.AgencijaID));
            Assert.That(preIposle.ProsecnaOcenaPre, Is.EqualTo(prethodnaVerzija.ProsecnaOcena));
            Assert.That(preIposle.OcenePre, Is.Not.EqualTo(preIposle.OcenePosle));
            Assert.That(preIposle.KorisniciPosle, Is.EqualTo(preIposle.KorisniciPre + 1));



             provera = await _administratorController.PribaviAgencijuPoID(ID2);
             result = await _administratorController.KorisnikDajeOcenuAgenciji(ID2, ocena);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(provera, Is.Not.Null);
            Assert.That(provera.Result, Is.TypeOf<OkObjectResult>());

            okResultProvera = provera.Result as OkObjectResult;
            Assert.That(okResultProvera, Is.Not.Null);
            prethodnaVerzija = okResultProvera.Value as TuristickaAgencija;
            preIposle = okResult.Value as OcenaPosle;
            Assert.That(preIposle, Is.Not.Null);
            Assert.That(ID2, Is.EqualTo(prethodnaVerzija.Id));
            Assert.That(ID2, Is.EqualTo(preIposle.AgencijaID));
            Assert.That(preIposle.ProsecnaOcenaPre, Is.EqualTo(prethodnaVerzija.ProsecnaOcena));
            Assert.That(preIposle.OcenePre, Is.Not.EqualTo(preIposle.OcenePosle));
            Assert.That(preIposle.KorisniciPosle, Is.EqualTo(preIposle.KorisniciPre + 1));

          

        }
        [Test]
        public async Task TestDodajOcenuAgencijiReturnsBadRequestNull()
        {

            var result = await _administratorController.KorisnikDajeOcenuAgenciji("123455789-ba56-49bc-ac11-664047438548", 4);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Nije pronadjena agencija u bazi"));


        }
        [Test]
        public async Task TestDodajOcenuAgencijiReturnsBadRequestID()
        {

            var result = await _administratorController.KorisnikDajeOcenuAgenciji("1198765432", 4);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("ID agencije mora biti veci od 30 karaktera"));


        }
        [Test]
        public async Task TestDodajOcenuAgencijiReturnsBadRequestOcena()
        {

            var result = await _administratorController.KorisnikDajeOcenuAgenciji(ID1, 0);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result.Result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Ocena koju korisnik daje agenciji mora biti izmedju 1 i 5"));


        }
        //TESTIRA SE BRISANJE AGENCIJE

        [Test]
        public async Task TestObrisiAgencijuReturnsOkObjectResult()
        {
           
          
               var result = await _administratorController.ObrisiAgenciju(ID3);

                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf<OkObjectResult>());

               var okResult = result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);

              var  poruka = okResult.Value as string;
                Assert.That(poruka, Is.Not.Null.And.Contains("Uspesno obrisana agencija."));
            
        }

        [Test]
        public async Task TestObrisiAgencijuReturnsBadRequestNull()
        {

            var result = await _administratorController.ObrisiAgenciju("123456789-dc0a-4cef-be57-8904daedcc81");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Nije pronadjenja agencija za brisanje"));


        }
        [Test]
        public async Task TestObrisiAgencijuReturnsBadRequestID()
        {

            var result = await _administratorController.ObrisiAgenciju("1198765432");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var losZahtev = result as BadRequestObjectResult;
            Assert.That(losZahtev, Is.Not.Null);

            var sadrzaj = losZahtev.Value as string;
            Assert.That(sadrzaj, Is.Not.Null.And.Contains("Identifikator agencije koja se brise mora biti veci od 30 karaktera"));


        }
        [OneTimeTearDown]
        public async Task OnTearDown()
        {
            await _administratorController.ObrisiAgenciju(ID1);
            await _administratorController.ObrisiAgenciju(ID2);
            await _administratorController.ObrisiAgenciju(ID3);
        }
        ~AgencijaTests() { OnTearDown(); }


    }


}
