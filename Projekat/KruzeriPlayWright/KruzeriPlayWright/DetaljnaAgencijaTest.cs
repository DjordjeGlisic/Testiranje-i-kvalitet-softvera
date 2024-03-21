using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.Playwright;
using System.Runtime.Intrinsics.X86;
using Microsoft.Extensions.Configuration;


namespace KruzeriPlayWright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class DetaljnaAgencijaTest:PageTest
    {
        IPage page;
        IBrowser browser;
        [SetUp]
        public async Task Setup()
        {
            browser = await Playwright.Chromium.LaunchAsync(new()
            {
                Headless = false,
                SlowMo = 2000
            });

            page = await browser.NewPageAsync(new()
            {
                ViewportSize = new()
                {
                    Width = 1280,
                    Height = 720
                },
                ScreenSize = new()
                {
                    Width = 1280,
                    Height = 720
                },
                RecordVideoSize = new()
                {
                    Width = 1280,
                    Height = 720
                },
                RecordVideoDir = "../../../Videos"
            });

        }
        [Test]
        public async Task DetaljnaAgencijaFormaTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await Expect(page.GetByTestId("naziv")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("telefon")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("adresa")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("email")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("broj")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("ponude")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("korisnici")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("Korisnik0")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("Azuriraj0")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("Obrisi0")).ToBeVisibleAsync();

            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaDetaljnaAgencija.png" });






        }
        [Test]
        public async Task KlikNaAzurirajPonuduTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.WaitForSelectorAsync("[data-testid='Azuriraj0']");
            await page.GetByTestId("Azuriraj0").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/CitajAzurirajPonudu");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaCitajAzurirajPonudu.png" });
        }
        [Test]
        public async Task KlikNaObrisiPonuduTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.WaitForSelectorAsync("[data-testid='Obrisi0']");
            await page.GetByTestId("Obrisi0").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaCitajAzurirajPonudu.png" });
        }
        [Test]
        public async Task AgencijaSaljeAzuriraBrisePorukuTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.WaitForSelectorAsync("[data-testid='Korisnik0']");
            await page.ClickAsync("[data-testid='Korisnik0']");
            await page.WaitForURLAsync("http://localhost:3000/Cet");
            await page.GetByPlaceholder("Napisite komentar").FillAsync("Poruku salje agencija");
            await page.GetByText("Posalji").ClickAsync();
            await page.GetByPlaceholder("Napisite komentar").FillAsync("Ovo je poruka koja se ne brise salje je agencija");
            await page.GetByText("Posalji").ClickAsync();
            await page.WaitForSelectorAsync("[data-testid='Poruku salje agencija']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Poruku salje agencija']"), "Neuspelo slanje poruke");
            await page.GetByPlaceholder("Napisite komentar").FillAsync("Poruku azurira agencija");
            await page.ClickAsync("[data-testid='AzurirajPoruku salje agencija']");
            await page.WaitForSelectorAsync("[data-testid='Poruku azurira agencija']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Poruku azurira agencija']"), "Neuspelo slanje poruke");
            await page.ClickAsync("[data-testid='ObrisiPoruku azurira agencija']");
            Assert.IsNull(await page.QuerySelectorAsync("[data-testid='Poruku azurira agencija']"), "Neuspelo slanje poruke");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaCet.png" });
        }
    }
}
