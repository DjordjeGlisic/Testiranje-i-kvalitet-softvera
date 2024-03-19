using Kruzeri.Models;
using Microsoft.Playwright;
using System.Collections.Specialized;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KruzeriPlayWright
{
    [TestFixture]
    internal class PorukaControllerTest:PlaywrightTest
    {
        private IAPIRequestContext Request;
        private string IDAgencije;
        private string IDKorisnika;
        private string IDPoruke1;
        private string IDPoruke2;
        [SetUp]
        public async Task SetUpAPITesting()
        {
            var headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            };

            Request = await Playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = "https://localhost:7199",
                ExtraHTTPHeaders = headers,
                IgnoreHTTPSErrors = true
            });
            await using var response = await Request.PostAsync("/Administrator/DodajAgenciju", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "",
                    Naziv = "AgencijaTestiranjePoruke",
                    Adresa = "Adresa 11",
                    Telefon = 3331212330,
                    Email = "agencijatestiranjeporuke@gmail.com",
                    Sifra = "agencija123",
                    ProsecnaOcena = 0,
                    BrojKorisnikaKojiSuOcenili = 0,
                }
            });
            var jsonResponse = await response.JsonAsync();
            IDAgencije = jsonResponse?.GetProperty("id").ToString();
            await using var response2 = await Request.PostAsync("/Korisnik/RegisterUser", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "",
                    Ime = "imeP",
                    Prezime = "prezimeP",
                    Email = "korisnikp@gmail.com",
                    Sifra = "korisnik123",
                    Telefon = 3331212330,
                    DatumRodjenja = "20.8.2020",
                    Grad = "Sombor",
                    Adresa = "Adresa 4"
                }
            });
            var jsonResponse2 = await response2.JsonAsync();
            IDKorisnika = jsonResponse2?.GetProperty("id").ToString();


            await using var response3 = await Request.PostAsync("/Poruka/PosaljiPoruku", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id="string",
                    IdKorisnika=IDKorisnika,
                    IdAgencije=IDAgencije,
                   Datum=DateTime.Now.ToString(),
                   Sadrzaj="Ovo je poruka koju salje korisnik",
                   PoslataOdStraneAgencije=false,
                   PoslataOdStraneKorisnika=true
                }
            });
            var jsonResponse3 = await response3.JsonAsync();
            IDPoruke1 = jsonResponse3?.GetProperty("id").ToString();


            await using var response4 = await Request.PostAsync("/Poruka/PosaljiPoruku", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "string",
                    IdKorisnika = IDKorisnika,
                    IdAgencije = IDAgencije,
                    Datum = DateTime.Now.ToString(),
                    Sadrzaj = "Ovo je poruka koju salje agencija",
                    PoslataOdStraneAgencije = true,
                    PoslataOdStraneKorisnika = false
                }
            });
            var jsonResponse4 = await response4.JsonAsync();
            IDPoruke2 = jsonResponse4?.GetProperty("id").ToString();
        }
        [Test]
        [TestCase("Zdravo",true,false)]
        [TestCase("Pozdrav",false,true)]
        [TestCase("Kako si",true,false)]
        public async Task PosaljiPorukuTest(string sadrzaj, bool korisnik,bool agencija)
        {
            await using var response = await Request.PostAsync("/Poruka/PosaljiPoruku", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "string",
                    IdKorisnika = IDKorisnika,
                    IdAgencije = IDAgencije,
                    Datum = DateTime.Now.ToString(),
                    Sadrzaj = sadrzaj,
                    PoslataOdStraneAgencije = agencija,
                    PoslataOdStraneKorisnika = korisnik
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {

                Assert.That(jsonResponse?.GetProperty("sadrzaj").ToString(), Is.EqualTo(sadrzaj));
                Assert.That(jsonResponse?.GetProperty("poslataOdStraneKorisnika").ToString(), Is.EqualTo(korisnik.ToString()));
                Assert.That(jsonResponse?.GetProperty("poslataOdStraneAgencije").ToString(), Is.EqualTo(agencija.ToString()));
                Assert.That(jsonResponse?.GetProperty("idKorisnika").ToString(), Is.EqualTo(IDKorisnika.ToString()));
                Assert.That(jsonResponse?.GetProperty("idAgencije").ToString(), Is.EqualTo(IDAgencije.ToString()));
  



            });
            string id = jsonResponse?.GetProperty("id").ToString();
            await using var response1 = await Request.DeleteAsync($"/Poruka/ObrisiPoruku/{id}");
        }
        [Test]
        public async Task PribaviKorisnikeAgencijeTest()
        {
            await using var response = await Request.GetAsync($"/Poruka/PribaviKorisnikeAgencije/{IDAgencije}");
            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

        }
        [Test]
        public async Task PribaviCetTest()
        {
            await using var response = await Request.GetAsync($"/Poruka/PribaviCet/{IDKorisnika}/{IDAgencije}");
            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
        }
        [Test]
        public async Task PribaviPorukuTest()
        {
            await using var response = await Request.GetAsync($"/Poruka/PribaviPoruku/{IDPoruke1}");
            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("id").ToString(), Is.EqualTo(IDPoruke1));
                Assert.IsNotNull(jsonResponse?.GetProperty("sadrzaj"));
                Assert.IsNotNull(jsonResponse?.GetProperty("datum"));
                Assert.IsNotNull(jsonResponse?.GetProperty("idKorisnika").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("idAgencije").ToString());
            



            });

        }
        [Test]
        [TestCase("Azuriranje1", true, false)]
        [TestCase("Azuriranje2", false, true)]
        [TestCase("Azuriranje3", true, false)]
        public async Task AzurirajPorukuTest(string sadrzaj, bool korisnik,bool agencija)
        {
            await using var response = await Request.PutAsync($"/Poruka/AzurirajPoruku/{IDPoruke1}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = IDPoruke1,
                    IdKorisnika = IDKorisnika,
                    IdAgencije = IDAgencije,
                    Datum = DateTime.Now.ToString(),
                    Sadrzaj = sadrzaj,
                    PoslataOdStraneAgencije = agencija,
                    PoslataOdStraneKorisnika = korisnik
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("id").ToString(), Is.EqualTo(IDPoruke1));
                Assert.That(jsonResponse?.GetProperty("sadrzaj").ToString(), Is.EqualTo(sadrzaj));
                Assert.That(jsonResponse?.GetProperty("poslataOdStraneKorisnika").ToString(), Is.EqualTo(korisnik.ToString()));
                Assert.That(jsonResponse?.GetProperty("poslataOdStraneAgencije").ToString(), Is.EqualTo(agencija.ToString()));
                Assert.That(jsonResponse?.GetProperty("idKorisnika").ToString(), Is.EqualTo(IDKorisnika.ToString()));
                Assert.That(jsonResponse?.GetProperty("idAgencije").ToString(), Is.EqualTo(IDAgencije.ToString()));




            });
        }
        [Test]
        public async Task ObrisiPorukuTest()
        {
            await using var response = await Request.DeleteAsync($"/Poruka/ObrisiPoruku/{IDPoruke2}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo("Uspesno obrisana poruka."));
        }
        [TearDown]
        public async Task TearDownAPITesting()
        {
            await using var response1 = await Request.DeleteAsync($"/Poruka/ObrisiPoruku/{IDPoruke1}");
            await using var response2 = await Request.DeleteAsync($"/Poruka/ObrisiPoruku/{IDPoruke2}");
            await using var response3 = await Request.DeleteAsync($"/Administrator/ObrisiAgenciju/{IDAgencije}");
            await using var response4 = await Request.DeleteAsync($"/Korisnik/DeleteUser/{IDKorisnika}");
            await Request.DisposeAsync();
        }
    }
}
