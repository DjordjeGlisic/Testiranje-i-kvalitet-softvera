using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.Playwright;
using System.Runtime.Intrinsics.X86;
using Microsoft.Extensions.Configuration;
using Kruzeri.Controllers;
using Kruzeri.Models;
namespace KruzeriPlayWright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class AzurirajBrisiOdjaviKorisnikaTest:PageTest
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
        public async Task AzurirajKorisnikaFormaTest()
        {

            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.GetByText("Azuriraj informacije").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/CitajAzurirajKorisnika");

            await page.WaitForSelectorAsync("[data-testid='ime']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='ime']"), "Neuspelo pribavljanje korisnika");
            await page.WaitForSelectorAsync("[data-testid='prezime']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='prezime']"), "Neuspelo pribavljanje korisnika");
            await page.WaitForSelectorAsync("[data-testid='email']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='email']"), "Neuspelo pribavljanje korisnika");
            await page.WaitForSelectorAsync("[data-testid='sifra']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='sifra']"), "Neuspelo pribavljanje korisnika");
            await page.WaitForSelectorAsync("[data-testid='telefon']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='telefon']"), "Neuspelo pribavljanje korisnika");
            await page.WaitForSelectorAsync("[data-testid='ime']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='datum']"), "Neuspelo pribavljanje korisnika");
            await page.WaitForSelectorAsync("[data-testid='ime']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='grad']"), "Neuspelo pribavljanje korisnika");
            await page.WaitForSelectorAsync("[data-testid='ime']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='adresa']"), "Neuspelo pribavljanje korisnika");

            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSvePonude.png" });
        }
        [Test]
        public async Task AzurirajKorisnikaButtonTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.GetByText("Azuriraj informacije").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/CitajAzurirajKorisnika");

            await page.GetByPlaceholder("Unesite novo ime").FillAsync("Azurirano ime");
            await page.GetByPlaceholder("Unesite novo prezime").FillAsync("Azurirano prezime");
            await page.GetByPlaceholder("Unesite novi email").FillAsync("korisnikaki@gmail.com");
            await page.GetByPlaceholder("Unesite novu sifru").FillAsync("aki12345");
            await page.GetByPlaceholder("Unesite novi telefon").FillAsync("1233211234");
            await page.GetByPlaceholder("Unesite novi datum").FillAsync("27.8.1992");
            await page.GetByPlaceholder("Unesite novi grad").FillAsync("Azuriran grad");
            await page.GetByPlaceholder("Unesite novu adresu").FillAsync("Azurirana adresa");
            await page.GetByTestId("dugme").ClickAsync();

            await page.WaitForURLAsync("http://localhost:3000/Prijava");








        }
        [Test]
        public async Task OdjaviKorisnikaTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.GetByText("Odjavi se").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");

        }
        [Test]
        public async Task ObrisiKorisnikaTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.GetByText("Obrisi nalog").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");

        }
    }
}
