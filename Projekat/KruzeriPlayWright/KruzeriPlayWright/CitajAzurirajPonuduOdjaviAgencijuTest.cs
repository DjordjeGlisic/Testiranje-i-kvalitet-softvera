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
    public class CitajAzurirajPonuduOdjaviAgencijuTest:PageTest
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
        public async Task KlikNaOdjavuAgencijeTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.GetByText("Odjavi se").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaOdjaviAgenciju.png" });
        
        }
        [Test]
        public async Task CitajAzurirajPonuduTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.GetByTestId("Azuriraj0").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/CitajAzurirajPonudu");
            await Expect(page.GetByTestId("text-azuriraj")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("naziv")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("aerodrom")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("grad")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("polazak")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("dolazak")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("bez")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("sa")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("opis")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("lista")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("Grad0")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("nove")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("dugme-azuriraj")).ToBeVisibleAsync();
            await page.GetByTestId("unos-naziv").FillAsync("Ponuda azurirana");
            await page.GetByTestId("unos-aerodrom").FillAsync("Aerodrom azuriran");
            await page.GetByTestId("unos-grad").FillAsync("Venice");
            await page.GetByTestId("unos-polazak").FillAsync("28.8.2024");
            await page.GetByTestId("unos-dolazak").FillAsync("5.9.2024");
            await page.GetByTestId("unos-bez").FillAsync("900");
            await page.GetByTestId("unos-sa").FillAsync("1100");
            await page.GetByTestId("unos-opis").FillAsync("Ponuda je azurirana ova je bolja");
            await page.GetByTestId("unos-Grad").FillAsync("Naples");
            await page.GetByTestId("dugme-grad").ClickAsync();
            await page.GetByTestId("unos-Grad").FillAsync("Bari");
            await page.GetByTestId("dugme-grad").ClickAsync();
            await page.GetByTestId("unos-Grad").FillAsync("Sasuolo");
            await page.GetByTestId("dugme-grad").ClickAsync();
            await Expect(page.GetByTestId("grad0")).ToBeVisibleAsync();
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaCitajAzurirajAgenciju.png" });
            await page.GetByTestId("dugme-azuriraj").ClickAsync();

            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");












        }
    }
}
