using Kruzeri.Models;
using Microsoft.Playwright;
using System.Collections.Specialized;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace KruzeriPlayWright
{
    [TestFixture]
    internal class PonudaControllerTest:PlaywrightTest
    {
        private IAPIRequestContext Request;
        private string IDAgencije;
        private string IDKorisnika;
        private string IDPonude1;
        private string IDPonude2;

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
                    Naziv = "AgencijaTestiranjePonude",
                    Adresa = "Adresa ponude 11",
                    Telefon = 3331212330,
                    Email = "agencijatestiranjeponude@gmail.com",
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
                    Ime = "imePoruka",
                    Prezime = "prezimePoruka",
                    Email = "korisnikporuka@gmail.com",
                    Sifra = "korisnik123",
                    Telefon = 3331212330,
                    DatumRodjenja = "20.8.2020",
                    Grad = "Sombor",
                    Adresa = "Adresa 4"
                }
            });
            var jsonResponse2 = await response2.JsonAsync();
            IDKorisnika = jsonResponse2?.GetProperty("id").ToString();

            string[] lista = { "jedan", "dva", "tri" };
            await using var response3 = await Request.PostAsync($"/Ponuda/DodajPonudu/{IDAgencije}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "string",
                    NazivPonude = "Ponuda controller test1",
                    GradPolaskaBroda = "string",
                    NazivAerodroma = "Aerodrom string",
                    DatumPolaska = "22.7.20204",
                    DatumDolaska = "28.8.2024",
                    CenaSmestajaBezHrane = 330,
                    CenaSmestajaSaHranom = 990,
                    ListaGradova = lista,
                    OpisPutovanja = "Lepo putovanje"
                }
            });
            var jsonResponse3 = await response3.JsonAsync();
            IDPonude1 = jsonResponse3?.GetProperty("id").ToString();


            await using var response4 = await Request.PostAsync($"/Ponuda/DodajPonudu/{IDAgencije}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "string",
                    NazivPonude = "Ponuda controller test2",
                    GradPolaskaBroda = "string",
                    NazivAerodroma = "Aerodrom string",
                    DatumPolaska = "22.7.20204",
                    DatumDolaska = "28.8.2024",
                    CenaSmestajaBezHrane = 330,
                    CenaSmestajaSaHranom = 990,
                    ListaGradova = lista,
                    OpisPutovanja = "Lepo putovanje"
                }
            });
            var jsonResponse4= await response4.JsonAsync();
            IDPonude2 = jsonResponse4?.GetProperty("id").ToString();
            await using var response5 = await Request.PostAsync($"/Korisnik/RezervisiPonudu/{IDKorisnika}/{IDPonude1}");

        }
        [Test]
        [TestCase("Rimska", "Venice", "Aerodrom Marko Polo", "4.2.2024", "7.1.2025", 900, 4500, new string[] { "Venice", "Pisa", "Rome", "Naples", "Bari", "Geona" }, "Veoma lepo putovanje")]
        public async Task DodajPonuduTest(string naziv,string gradPolaska,string aerodrom,string polazak,string dolazak,int cenaBez,int cenaSa, string[]gradovi,string opis)
        {
            List<string> lista = new List<string>();
            foreach (string grad in gradovi)
            {
                lista.Add(grad);
            }
            await using var response = await Request.PostAsync($"/Ponuda/DodajPonudu/{IDAgencije}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "string",
                    NazivPonude = naziv,
                    GradPolaskaBroda = gradPolaska,
                    NazivAerodroma = aerodrom,
                    DatumPolaska = polazak,
                    DatumDolaska = dolazak,
                    CenaSmestajaBezHrane = cenaBez,
                    CenaSmestajaSaHranom = cenaSa,
                    ListaGradova = lista,
                    OpisPutovanja = opis
                }
            });
            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {

                Assert.That(jsonResponse?.GetProperty("nazivPonude").ToString(), Is.EqualTo(naziv));
                Assert.That(jsonResponse?.GetProperty("gradPolaskaBroda").ToString(), Is.EqualTo(gradPolaska));
                Assert.That(jsonResponse?.GetProperty("nazivAerodroma").ToString(), Is.EqualTo(aerodrom));
                Assert.That(jsonResponse?.GetProperty("datumPolaska").ToString(), Is.EqualTo(polazak));
                Assert.That(jsonResponse?.GetProperty("datumDolaska").ToString(), Is.EqualTo(dolazak));
                Assert.That(jsonResponse?.GetProperty("cenaSmestajaBezHrane").ToString(), Is.EqualTo(cenaBez.ToString()));
                Assert.That(jsonResponse?.GetProperty("cenaSmestajaSaHranom").ToString(), Is.EqualTo(cenaSa.ToString()));
                Assert.IsNotEmpty(jsonResponse?.GetProperty("listaGradova").ToString());
                Assert.That(jsonResponse?.GetProperty("opisPutovanja").ToString(), Is.EqualTo(opis));



            });
            string id=jsonResponse?.GetProperty("id").ToString();
            await using var response2 = await Request.DeleteAsync($"/Ponuda/ObrisiPonudu/{id}");
        }
        [Test]
        public async Task PribaviSvePonudeAgencijeTest()
        {
            await using var response = await Request.GetAsync($"/Ponuda/PribaviSvePonudeAgencije/{IDAgencije}");
            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

        }
        [Test]
        public async Task PribaviPonuduKorisnikaTest()
        {
            await using var response = await Request.GetAsync($"/Ponuda/PribaviPonuduKorisnika/{IDKorisnika}/{IDPonude1}");
            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
        }
        [Test]
        public async Task PribaviPonuduPoIdTest()
        {
            await using var response = await Request.GetAsync($"/Ponuda/PribaviPonuduPoId/{IDPonude1}");
            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("id").ToString(), Is.EqualTo(IDPonude1));
                Assert.IsNotNull(jsonResponse?.GetProperty("nazivPonude").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("gradPolaskaBroda").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("nazivAerodroma").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("datumPolaska").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("datumDolaska").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("cenaSmestajaBezHrane").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("cenaSmestajaSaHranom").ToString());
                Assert.IsNotEmpty(jsonResponse?.GetProperty("listaGradova").ToString());
                Assert.IsNotNull(jsonResponse?.GetProperty("opisPutovanja").ToString());



            });
        }
        [Test]
        public async Task PribaviSvePonudeTest()
        {
            await using var response = await Request.GetAsync("/Ponuda/PribaviSvePonude");
            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

        }
        [Test]
        [TestCase("Rimska", "Venice", "Aerodrom Marko Polo", "4.2.2024", "7.1.2025", 900, 4500, new string[] { "Venice", "Pisa", "Rome", "Naples", "Bari", "Geona" }, "Veoma lepo putovanje")]
        public async Task AzurirajPonuduTest(string naziv,string gradPolaska ,string aerodrom, string polazak, string dolazak, int cenaBez, int cenaSa, string[] gradovi, string opis)
        {
            List<string> lista = new List<string>();
            foreach (string grad in gradovi)
            {
                lista.Add(grad);
            }
            await using var response = await Request.PutAsync($"/Ponuda/AzurirajPonudu/{IDPonude1}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Id = "string",
                    NazivPonude = naziv,
                    GradPolaskaBroda = gradPolaska,
                    NazivAerodroma = aerodrom,
                    DatumPolaska = polazak,
                    DatumDolaska = dolazak,
                    CenaSmestajaBezHrane = cenaBez,
                    CenaSmestajaSaHranom = cenaSa,
                    ListaGradova = lista,
                    OpisPutovanja = opis
                }
            });
            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {

                Assert.That(jsonResponse?.GetProperty("nazivPonude").ToString(), Is.EqualTo(naziv));
                Assert.That(jsonResponse?.GetProperty("gradPolaskaBroda").ToString(), Is.EqualTo(gradPolaska));
                Assert.That(jsonResponse?.GetProperty("nazivAerodroma").ToString(), Is.EqualTo(aerodrom));
                Assert.That(jsonResponse?.GetProperty("datumPolaska").ToString(), Is.EqualTo(polazak));
                Assert.That(jsonResponse?.GetProperty("datumDolaska").ToString(), Is.EqualTo(dolazak));
                Assert.That(jsonResponse?.GetProperty("cenaSmestajaBezHrane").ToString(), Is.EqualTo(cenaBez.ToString()));
                Assert.That(jsonResponse?.GetProperty("cenaSmestajaSaHranom").ToString(), Is.EqualTo(cenaSa.ToString()));
                Assert.IsNotEmpty(jsonResponse?.GetProperty("listaGradova").ToString());
                Assert.That(jsonResponse?.GetProperty("opisPutovanja").ToString(), Is.EqualTo(opis));



            });
        }
        [Test]
        public async Task ObrisiPonuduTest()
        {
            await using var response = await Request.DeleteAsync($"/Ponuda/ObrisiPonudu/{IDPonude2}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo("Uspesno obrisana ponuda."));
        }
        [TearDown]
        public async Task TearDownAPITesting()
        {
            await using var response1 = await Request.DeleteAsync($"/Ponuda/ObrisiPonudu/{IDPonude2}");
            await using var response2 = await Request.DeleteAsync($"/Ponuda/ObrisiPonudu/{IDPonude1}");
            await using var response3 = await Request.DeleteAsync($"/Administrator/ObrisiAgenciju/{IDAgencije}");
            await using var response4 = await Request.DeleteAsync($"/Korisnik/DeleteUser/{IDKorisnika}");
            await Request.DisposeAsync();
        }
    }
}
