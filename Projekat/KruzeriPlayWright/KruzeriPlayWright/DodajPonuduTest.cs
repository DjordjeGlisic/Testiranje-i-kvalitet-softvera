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
    public class DodajPonuduTest:PageTest
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
        public async Task DodavanjePonuduTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await page.GetByText("Dodaj ponudu").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DodajPonudu");
            await page.GetByTestId("naziv").FillAsync("Ponuda1");
            await page.GetByTestId("aerodrom").FillAsync("Aerodrom Atina");
            await page.GetByTestId("grad").FillAsync("Atina");
            await page.GetByTestId("polazak").FillAsync("11.7.2024");
            await page.GetByTestId("dolazak").FillAsync("31.7.2024");
            await page.GetByTestId("ne-hrana").FillAsync("200");
            await page.GetByTestId("hrana").FillAsync("400");
            await page.GetByTestId("opis").FillAsync("Veoma kvalitetno putovanje");
            await page.GetByTestId("gradovi").FillAsync("Atina");
            await page.GetByTestId("potvrda").ClickAsync();
            await page.GetByTestId("gradovi").FillAsync("Haifa");
            await page.GetByTestId("potvrda").ClickAsync();
            await page.GetByTestId("gradovi").FillAsync("Nikozija");
            await page.GetByTestId("potvrda").ClickAsync();
            await Expect(page.GetByTestId("grad0")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("grad1")).ToBeVisibleAsync();
            await Expect(page.GetByTestId("grad2")).ToBeVisibleAsync();
            await page.GetByText("Dodaj ponudu").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaDodajPonudu.png" });
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");


        }

    }

}
