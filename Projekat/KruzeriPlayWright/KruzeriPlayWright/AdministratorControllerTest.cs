using Microsoft.Playwright;
using System.Collections.Specialized;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace KruzeriPlayWright
{
    [TestFixture]
    internal class AdministratorControllerTest:PlaywrightTest
    {
        private IAPIRequestContext Request;
        private string IDAgencije1;
        private string IDAgencije2;
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
                    Naziv = "AgencijaTestiranje",
                    Adresa = "Adresa 4",
                    Telefon = 3331212330,
                    Email = "agencijatestiranje1@gmail.com",
                    Sifra = "agencija123",
                    ProsecnaOcena = 0,
                    BrojKorisnikaKojiSuOcenili = 0,
                }
            });
            var jsonResponse = await response.JsonAsync();
            IDAgencije1 = jsonResponse?.GetProperty("id").ToString();
            await using var response2 = await Request.PostAsync("/Administrator/DodajAgenciju", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "",
                    Naziv = "AgencijaTestiranje2",
                    Adresa = "Adresa 4",
                    Telefon = 3331212330,
                    Email = "agencijatestiranje2@gmail.com",
                    Sifra = "agencija123",
                    ProsecnaOcena = 0,
                    BrojKorisnikaKojiSuOcenili = 0,
                }
            });
            var jsonResponse2 = await response2.JsonAsync();
            IDAgencije2 = jsonResponse2?.GetProperty("id").ToString();
        }
        [Test]
        [TestCase("AgencijaTest1","Agencijska 17",1234567890,"agencijatst1@gmail.com", "agencija123", 0,0)]
        public async Task DodajAgencijuTest(string naziv, string adresa, long telefon, string email, string sifra, int prosecna, int broj)
        {
            await using var response = await Request.PostAsync("/Administrator/DodajAgenciju", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "",
                    Naziv = naziv,
                    Adresa = adresa,
                    Telefon = telefon,
                    Email = email,
                    Sifra = sifra,
                    ProsecnaOcena = prosecna,
                    BrojKorisnikaKojiSuOcenili = broj,
                    
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {

                Assert.That(jsonResponse?.GetProperty("naziv").ToString(), Is.EqualTo(naziv));
                Assert.That(jsonResponse?.GetProperty("adresa").ToString(), Is.EqualTo(adresa));
                Assert.That(jsonResponse?.GetProperty("telefon").ToString(), Is.EqualTo(telefon.ToString()));
                Assert.That(jsonResponse?.GetProperty("email").ToString(), Is.EqualTo(email));
                Assert.That(jsonResponse?.GetProperty("sifra").ToString(), Is.EqualTo(sifra));
                Assert.That(jsonResponse?.GetProperty("prosecnaOcena").ToString(), Is.EqualTo(prosecna.ToString()));
                Assert.That(jsonResponse?.GetProperty("brojKorisnikaKojiSuOcenili").ToString(), Is.EqualTo(broj.ToString()));
                


            });
            string id = jsonResponse?.GetProperty("id").ToString();
            await using var response1 = await Request.DeleteAsync($"/Administrator/ObrisiAgenciju/{id}");

        }
        [Test]
        public async Task PribaviAgencijeTest()
        {
            await using var response = await Request.GetAsync("/Administrator/PribaviAgencije");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
        }
        [Test]
        [TestCase("agencijatestiranje1@gmail.com", "agencija123")]
        public async Task PribaviAgencijuTest(string email, string sifra)
        {
            await using var response = await Request.PostAsync("/Administrator/PribaviAgenciju", new APIRequestContextOptions
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
                Assert.IsNotNull(jsonResponse?.GetProperty("naziv"));
                Assert.IsNotNull(jsonResponse?.GetProperty("adresa"));
                Assert.That(jsonResponse?.GetProperty("telefon").ToString().Length, Is.EqualTo(10));
                Assert.That(jsonResponse?.GetProperty("email").ToString(), Is.EqualTo(email));
                Assert.That(jsonResponse?.GetProperty("sifra").ToString(), Is.EqualTo(sifra));
                


            });
        }
        [Test]
        public async Task PribaviAgencijuPoIdTest()
        {
            await using var response = await Request.PostAsync($"/Administrator/PribaviAgencijuPoId/{IDAgencije1}");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("id").ToString(), Is.EqualTo(IDAgencije1));
                Assert.IsNotNull(jsonResponse?.GetProperty("naziv"));
                Assert.IsNotNull(jsonResponse?.GetProperty("adresa"));
                Assert.That(jsonResponse?.GetProperty("telefon").ToString().Length, Is.EqualTo(10));
                Assert.IsNotNull(jsonResponse?.GetProperty("email").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("sifra").ToString());



            });



        }
        [TestCase("AgencijaAzurirana1", "Agencijska 11", 1234511110, "agencijatst1@gmail.com", "agencija123", 0, 0)]
        public async Task AzurirajAgencijuTest(string naziv, string adresa, long telefon, string email, string sifra, int prosecna, int broj)
        {
            await using var response = await Request.PutAsync($"/Administrator/AzurirajAgenciju/{IDAgencije1}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = IDAgencije1,
                    Naziv = naziv,
                    Adresa = adresa,
                    Telefon = telefon,
                    Email = email,
                    Sifra = sifra,
                    ProsecnaOcena = prosecna,
                    BrojKorisnikaKojiSuOcenili = broj,

                }
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("id").ToString(), Is.EqualTo(IDAgencije1));
                Assert.That(jsonResponse?.GetProperty("naziv").ToString(), Is.EqualTo(naziv));
                Assert.That(jsonResponse?.GetProperty("adresa").ToString(), Is.EqualTo(adresa));
                Assert.That(jsonResponse?.GetProperty("telefon").ToString(), Is.EqualTo(telefon.ToString()));
                Assert.That(jsonResponse?.GetProperty("email").ToString(), Is.EqualTo(email));
                Assert.That(jsonResponse?.GetProperty("sifra").ToString(), Is.EqualTo(sifra));
                Assert.That(jsonResponse?.GetProperty("prosecnaOcena").ToString(), Is.EqualTo(prosecna.ToString()));
                Assert.That(jsonResponse?.GetProperty("brojKorisnikaKojiSuOcenili").ToString(), Is.EqualTo(broj.ToString()));



            });
        }
        [Test]
        public async Task ObrisiAgencijuTest()
        {
            await using var response = await Request.DeleteAsync($"/Administrator/ObrisiAgenciju/{IDAgencije2}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo("Uspesno obrisana agencija."));
        }
        [TestCase(5)]
        [Test]
        public async Task DodajOcenuAgencijiTest(int ocena)
        {

            await using var response = await Request.PostAsync($"/Administrator/DodajOcenuAgenciji/{IDAgencije1}/{ocena}");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
        }
        [TearDown]
        public async Task TearDownAPITesting()
        {
            await using var response1 = await Request.DeleteAsync($"/Administrator/ObrisiAgenciju/{IDAgencije1}");
            await using var response2 = await Request.DeleteAsync($"/Poruka/ObrisiAgenciju/{IDAgencije2}");
            
            await Request.DisposeAsync();
        }
    }
}
