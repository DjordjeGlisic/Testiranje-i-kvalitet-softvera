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
    public class DodavanjeAgencijeTest:PageTest
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
        public async Task DodajAgencijuTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");



            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.GetByText("Dodaj Agenciju").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DodajAgenciju");
            await page.GetByTestId("naziv").FillAsync("Agencija1");
            await page.GetByTestId("adresa").FillAsync("Agencijska1");
            await page.GetByTestId("telefon").FillAsync("1233331230");
            await page.GetByTestId("email").FillAsync("agencija1@gmail.com");
            await page.GetByTestId("sifra").FillAsync("agencija123");
            await page.GetByTestId("dodaj").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaDodajAgenciju.png" });


        }
    }
}
