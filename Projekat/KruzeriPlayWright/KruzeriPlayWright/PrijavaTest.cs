using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.Playwright;

namespace KruzeriPlayWright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
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
        public async Task FormaPrijaveTest()
        {
            await page.GotoAsync("http://localhost:3000/Prijava");

            await page.WaitForSelectorAsync("[data-testid='papir-prijava']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='papir-prijava']"), "Stranica prijave nije prikazana");

            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaPrijava.png" });
        }
        [Test]
        public async Task PrijaviKorisnikaTest()
        {

            await page.GotoAsync("http://localhost:3000/Prijava");

            await page.GetByLabel("Email Address").FillAsync("korisnikaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("aki12345");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
           
            await page.WaitForURLAsync("http://localhost:3000/");
            await page.WaitForSelectorAsync("[test-id='sve-ponude']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[test-id='sve-ponude']"), "Stranica sve ponude nije prikazana");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaPrijava1.png" });
        }
        [Test]
        public async Task PrijaviAgenciju()
        {
            await page.GotoAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("agencija1@gmail.com");
            await page.GetByLabel("Password").FillAsync("agencija123");
            await Expect(page.GetByText("Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/DetaljnaAgencija");
            await Expect(page.GetByText("Naziv Agencije")).ToBeVisibleAsync();
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaPrijava2.png" });
        }
        [Test]
        public async Task PrijaviAdministratora()
        {
            await page.GotoAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText("Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await Expect(page.GetByText("Agencije koje svoje ponude stavljaju na platformi \"Kruzeri\"")).ToBeVisibleAsync();
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaPrijava3.png" });
        }
        [Test]
        public async Task PrelazakNaRegistracijuTest()
        {
            await page.GotoAsync("http://localhost:3000/Prijava");
            await Expect(page.GetByRole(AriaRole.Link)).ToBeVisibleAsync();
            await page.GetByRole(AriaRole.Link).ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Registracija#");
            await Expect(page.GetByText("Registracija")).ToBeVisibleAsync();
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaPrijava4.png" });
        }
        [TearDown]
        public async Task Teardown()
        {
            await page.CloseAsync();
            await browser.DisposeAsync();
        }
    }
}
