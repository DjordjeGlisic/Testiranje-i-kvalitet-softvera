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

    public class CitajAzurirajAgencijuOdjaviAdminaTest:PageTest
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
        public async Task OdjaviSeTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.GetByText("Odjavi se").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaOdjaviAdministratora.png" });


        }
        [Test]
        public async Task AzurirajAgencijuTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.GetByTestId("Agencija-Azuriraj0").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/CitajAzurirajAgenciju");
            await Expect(page.GetByText("Azuriraj agenciju")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("naziv")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("adresa")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("telefon")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("email")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("azuriraj")).ToBeVisibleAsync();
            await page.GetByTestId("nov-naziv").FillAsync("Agencija1");
            await page.GetByTestId("nova-adresa").FillAsync("Azurirana 14");
            await page.GetByTestId("nov-telefon").FillAsync("9189459701");
            await page.GetByTestId("novi-email").FillAsync("agencija1@gmail.com");
            await page.GetByTestId("nova-sifra").FillAsync("agencija123");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaAzurirajAgenciju.png" });
            await page.GetByText("Potvrdi azuriranje").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");






        }
        [Test]
        public async Task KlikNaPocetnaTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.GetByTestId("Agencija-Azuriraj0").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/CitajAzurirajAgenciju");
            await page.GetByText("Pocetna").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSveAgencijeKaoPocetna.png" });


        }
    }
}
