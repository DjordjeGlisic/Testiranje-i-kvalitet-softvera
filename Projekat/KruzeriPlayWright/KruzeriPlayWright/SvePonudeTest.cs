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
    public class SvePonudeTest:PageTest
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
        public async Task SvePonudeFormaTest()
        {
            
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.WaitForSelectorAsync("[data-testid='Ponuda0']");
            await Expect(page.GetByText("Ponude koje nude agencije na platformi \"Kruzeri\"")).ToBeVisibleAsync();
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Ponuda0']"), "Neuspelo pribavljanje ponude");


            await Expect(page.GetByText("Agencije koje svoje ponude stavljaju na platformi \"Kruzeri\"")).ToBeVisibleAsync();
            await page.WaitForSelectorAsync("[data-testid='Agencija0']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Agencija0']"), "Neuspelo pribavljanje agencije");


            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSvePonude.png" });
        }
        [Test]
        public async Task SvePonudeRezervacijaTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.WaitForSelectorAsync("[data-testid='Rezervisi0']");
            await page.ClickAsync("[data-testid='Rezervisi0']");
            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.WaitForSelectorAsync("[data-testid='Otkazi0']");
            await page.ClickAsync("[data-testid='Otkazi0']");
            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");




            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSvePonude1.png" });
        }
        [Test]
        public async Task SvePonudeDetaljiPonudetest()
        {

            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");


            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.WaitForSelectorAsync("[data-testid='Ponuda-Detalji0']");
            await page.ClickAsync("[data-testid='Ponuda-Detalji0']");
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaPonuda");
            await Expect(page.GetByText("Naziv ponude")).ToBeVisibleAsync();
            await Expect(page.GetByText("Grad polaska broda")).ToBeVisibleAsync();
            await Expect(page.GetByText("Datum pocetka putovanja")).ToBeVisibleAsync();
            await Expect(page.GetByText("Datum kraja putovanja")).ToBeVisibleAsync();
            await Expect(page.GetByText("Cena smestaja na brodu bez hrane")).ToBeVisibleAsync();
            await Expect(page.GetByText("Cena smestaja na brodu sa hranom")).ToBeVisibleAsync();
            await Expect(page.GetByText("Lista gradova koji se nalaze na ovoj ponudi")).ToBeVisibleAsync();
            await Expect(page.GetByText("Opis putovanja")).ToBeVisibleAsync();
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaDetaljnaPonuda.png" });






            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSvePonude.png" });
        }
        [Test]
        public async Task SvePonudeDetaljiAgencijetest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");




            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.WaitForSelectorAsync("[data-testid='Agencija-Detalji0']");
            await page.ClickAsync("[data-testid='Agencija-Detalji0']");
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await Expect(page.GetByText("Naziv Agencije")).ToBeVisibleAsync();
            await Expect(page.GetByText("Telefon:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Adresa:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Email:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Prosecna ocena")).ToBeVisibleAsync();
            await Expect(page.GetByText("Broj korisnika koji su agenciju ocenili:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Dajte ocenu agenciji:")).ToBeVisibleAsync();
            await Expect(page.GetByText("Ponude koja agencija nudi")).ToBeVisibleAsync();
            await Expect(page.GetByText("Korisnici si u kontaktu sa agencijom")).ToBeVisibleAsync();
            await page.GetByTestId("unos-ocena").FillAsync("4");
            await page.GetByTestId("dugme-oceni").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaDetaljnaAgencija.png" });





          
        }
        [Test]
        public async Task SvePonudePosaljiPorukuTest()
        {

            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.WaitForSelectorAsync("[data-testid='Agencija-PorukaAgencija1']");
            await page.ClickAsync("[data-testid='Agencija-PorukaAgencija1']");
            await page.WaitForURLAsync("http://localhost:3000/Cet");
            await page.GetByPlaceholder("Napisite komentar").FillAsync("Poruku salje korisnik");
            await page.GetByText("Posalji").ClickAsync();
            await page.GetByPlaceholder("Napisite komentar").FillAsync("Ovo je poruka koja se ne brise");
            await page.GetByText("Posalji").ClickAsync();
            await page.WaitForSelectorAsync("[data-testid='Poruku salje korisnik']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Poruku salje korisnik']"), "Neuspelo slanje poruke");
            await page.GetByPlaceholder("Napisite komentar").FillAsync("Poruku azurira korisnik");
            await page.ClickAsync("[data-testid='AzurirajPoruku salje korisnik']");
            await page.WaitForSelectorAsync("[data-testid='Poruku azurira korisnik']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Poruku azurira korisnik']"), "Neuspelo slanje poruke");
            await page.ClickAsync("[data-testid='ObrisiPoruku azurira korisnik']");
           // Assert.IsNull(await page.QuerySelectorAsync("[data-testid='Poruku azurira korisnik']"), "Neuspelo brisanje poruke");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaCet.png" });

        }
        [Test]
        public async Task SvePonudeGuest()
        {
            await page.GotoAsync("http://localhost:3000/");
            


            await Expect(page.GetByText("Ponude koje nude agencije na platformi \"Kruzeri\"")).ToBeVisibleAsync();
            await Expect(page.GetByText("Ponude koje nude agencije na platformi \"Kruzeri\"")).ToBeVisibleAsync();
            await page.WaitForSelectorAsync("[data-testid='Agencija-Detalji0']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Agencija0']"), "Neuspelo pribavljanje agencije");
            await page.WaitForSelectorAsync("[data-testid='Ponuda-Detalji0']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Ponuda0']"), "Neuspelo pribavljanje agencije");
            Assert.IsNull(await page.QuerySelectorAsync("[data-testid='Rezervisi0']"),"Neuspelo sakrivanje dugmeta rezervisi od gosta");
            Assert.IsNull(await page.QuerySelectorAsync("[data-testid='Otkazi0']"), "Neuspelo sakrivanje dugmeta otkazi od gosta");
            Assert.IsNull(await page.QuerySelectorAsync("[data-testid='Agencija-Poruka0']"), "Neuspelo sakrivanje dugmeta poruka od gosta");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSvePonudeGost.png" });

        }
    }
}
