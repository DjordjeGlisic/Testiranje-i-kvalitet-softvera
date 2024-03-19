
using Microsoft.Playwright;
using System.Collections.Specialized;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KruzeriPlayWright
{
    [TestFixture]
    internal class KorisnikControllerTest: PlaywrightTest
    {
        private IAPIRequestContext Request;
        private string IDKorisnika1;
        private string IDKorisnika2;
        private string IDPonude;
        private string IDAgencije;
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
            await using var response = await Request.PostAsync("/Korisnik/RegisterUser", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "",
                    Ime = "ime",
                    Prezime = "prezime",
                    Email = "korisnikemail@gmail.com",
                    Sifra = "korisnik123",
                    Telefon = 3331212330,
                    DatumRodjenja = "20.8.2020",
                    Grad = "Sombor",
                    Adresa = "Adresa 4"
                }
            });
            var jsonResponse = await response.JsonAsync();
            IDKorisnika1 = jsonResponse?.GetProperty("id").ToString();



            await using var response2 = await Request.PostAsync("/Korisnik/RegisterUser", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "",
                    Ime = "ime2",
                    Prezime = "prezime2",
                    Email = "korisnikemail2@gmail.com",
                    Sifra = "korisnik1232",
                    Telefon = 3331212330,
                    DatumRodjenja = "20.8.2020",
                    Grad = "Sombor",
                    Adresa = "Adresa 4"
                }
            });
            var jsonResponse2 = await response2.JsonAsync();
            IDKorisnika2 = jsonResponse2?.GetProperty("id").ToString();


            await using var response3 = await Request.PostAsync("/Administrator/DodajAgenciju", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "",
                    Naziv = "AgencijaKorisnik",
                    Adresa = "Adresa 4",
                    Telefon = 3331212330,
                    Email = "agencijaproba@gmail.com",
                    Sifra = "agencija123",
                    ProsecnaOcena = 0,
                    BrojKorisnikaKojiSuOcenili = 0,
                }
            });
            var jsonResponse3 = await response3.JsonAsync();
            IDAgencije = jsonResponse3?.GetProperty("id").ToString();


            string[] lista = { "jedan", "dva", "tri" };
            await using var response4 = await Request.PostAsync($"/Ponuda/DodajPonudu/{IDAgencije}", new APIRequestContextOptions
            {
               
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id="string",
                   NazivPonude="string",
                   GradPolaskaBroda="string",
                   NazivAerodroma="Aerodrom string",
                   DatumPolaska="22.7.20204",
                   DatumDolaska="28.8.2024",
                   CenaSmestajaBezHrane=330,
                   CenaSmestajaSaHranom=990,
                   ListaGradova=lista,
                   OpisPutovanja="Lepo putovanje"
                }
            });
            var jsonResponse4 = await response4.JsonAsync();
            IDPonude = jsonResponse4?.GetProperty("id").ToString();
        }
        [Test]
        [TestCase("Ime", "Prezime", "korisnikime@gmail.com", "korisnik123", 1180100110,"4.3.1972","Sombor","Somborska,14")]
        public async Task RegisterUserTest(string ime,string prezime,string email,string sifra,long telefon,string datum,string grad,string adresa)
        {
            await using var response = await Request.PostAsync("/Korisnik/RegisterUser", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                  Id="",
                  Ime=ime,
                  Prezime=prezime,
                  Email=email,
                  Sifra=sifra,
                  Telefon=telefon,
                  DatumRodjenja=datum,
                  Grad=grad,
                  Adresa=adresa
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {

                Assert.That(jsonResponse?.GetProperty("ime").ToString(), Is.EqualTo(ime));
                Assert.That(jsonResponse?.GetProperty("prezime").ToString(), Is.EqualTo(prezime));
                Assert.That(jsonResponse?.GetProperty("email").ToString(), Is.EqualTo(email));
                Assert.That(jsonResponse?.GetProperty("sifra").ToString(), Is.EqualTo(sifra));
                Assert.That(jsonResponse?.GetProperty("telefon").ToString(), Is.EqualTo(telefon.ToString()));
                Assert.That(jsonResponse?.GetProperty("datumRodjenja").ToString(), Is.EqualTo(datum.ToString()));
                Assert.That(jsonResponse?.GetProperty("grad").ToString(), Is.EqualTo(grad.ToString()));
                Assert.That(jsonResponse?.GetProperty("adresa").ToString(), Is.EqualTo(adresa.ToString()));



            });
            string id = jsonResponse?.GetProperty("id").ToString();
            await using var response1 = await Request.DeleteAsync($"/Korisnik/DeleteUser/{id}");
        }
        [Test]
        [TestCase("korisnikemail@gmail.com", "korisnik123")]
        public async Task LoginUserTest(string email, string sifra)
        {
            await using var response = await Request.PostAsync("/Korisnik/LoginUser", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Email = email,
                    Sifra = sifra,
                    
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(jsonResponse?.GetProperty("ime"));
                Assert.IsNotNull(jsonResponse?.GetProperty("prezime"));
                Assert.That(jsonResponse?.GetProperty("email").ToString(), Is.EqualTo(email));
                Assert.That(jsonResponse?.GetProperty("sifra").ToString(), Is.EqualTo(sifra));
                Assert.That(jsonResponse?.GetProperty("telefon").ToString().Length, Is.EqualTo(10));
                Assert.IsNotNull(jsonResponse?.GetProperty("datumRodjenja"));
                Assert.IsNotNull(jsonResponse?.GetProperty("grad"));
                Assert.IsNotNull(jsonResponse?.GetProperty("adresa").ToString());



            });
        }
        [Test]
        public async Task ReadUserTest()
        {
            await using var response = await Request.GetAsync($"/Korisnik/ReadUser/{IDKorisnika1}");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            
           
            Assert.That(jsonResponse?.GetProperty("id").ToString(), Is.EqualTo(IDKorisnika1));
        }
        [Test]
        [TestCase("Azurrano ime", "Azurirano prezime", "korisnikemail@gmail.com","korisnik123", 7210173201,"1.1.2001","Nis","Niska 22")]
        public async Task UpdateUserTest(string ime,string prezime,string email,string sifra,long telefon,string datum,string grad,string adresa)
        {
            await using var response = await Request.PutAsync($"/Korisnik/UpdateUser/{IDKorisnika1}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = IDKorisnika1,
                    Ime = ime,
                    Prezime = prezime,
                    Email = email,
                    Sifra = sifra,
                    Telefon = telefon,
                    DatumRodjenja = datum,
                    Grad = grad,
                    Adresa = adresa
                }
            
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("id").ToString(), Is.EqualTo(IDKorisnika1));
                Assert.That(jsonResponse?.GetProperty("ime").ToString(), Is.EqualTo(ime));
                Assert.That(jsonResponse?.GetProperty("prezime").ToString(), Is.EqualTo(prezime));
                Assert.That(jsonResponse?.GetProperty("email").ToString(), Is.EqualTo(email));
                Assert.That(jsonResponse?.GetProperty("sifra").ToString(), Is.EqualTo(sifra));
                Assert.That(jsonResponse?.GetProperty("telefon").ToString(), Is.EqualTo(telefon.ToString()));
                Assert.That(jsonResponse?.GetProperty("datumRodjenja").ToString(), Is.EqualTo(datum.ToString()));
                Assert.That(jsonResponse?.GetProperty("grad").ToString(), Is.EqualTo(grad.ToString()));
                Assert.That(jsonResponse?.GetProperty("adresa").ToString(), Is.EqualTo(adresa.ToString()));
            });
        }
        [Test]
        public async Task DeleteUserTest()
        {
            await using var response = await Request.DeleteAsync($"/Korisnik/DeleteUser/{IDKorisnika2}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo($"Uspesno obrisan korisnik."));
        }
        [Test]
        public async Task RezervisiPonuduTest()
        {
            await using var response = await Request.PostAsync($"/Korisnik/RezervisiPonudu/{IDKorisnika1}/{IDPonude}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo("Uspesno rezervisana ponuda"));
        }
        [Test]
        public async Task OtkaziRezervacijuTest()
        {
            await using var response = await Request.PostAsync($"/Korisnik/OtkaziRezervaciju/{IDKorisnika1}/{IDPonude}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo("Uspesno otkazana rezervacija"));
        }
        [TearDown]
        public async Task TearDownAPITesting()
        {
            await using var response1 = await Request.DeleteAsync($"/Korisnik/DeleteUser/{IDKorisnika1}");
            await using var response2 = await Request.DeleteAsync($"/Korisnik/DeleteUser/{IDKorisnika2}");
            await using var response3 = await Request.DeleteAsync($"/Administrator/ObrisiAgenciju/{IDAgencije}");
            await using var response4 = await Request.DeleteAsync($"/Ponuda/ObrisiPonudu/{IDPonude}");
            await Request.DisposeAsync();
        }

    }
}
