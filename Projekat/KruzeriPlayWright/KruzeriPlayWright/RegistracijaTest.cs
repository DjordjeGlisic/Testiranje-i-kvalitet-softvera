using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.Playwright;
using System.Runtime.Intrinsics.X86;

namespace KruzeriPlayWright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class RegistracijaTest:PageTest
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
        public async Task RegistracijaFormaTest()
        {
            await page.GotoAsync("http://localhost:3000/Registracija");

            await page.WaitForSelectorAsync("[data-testid='registracija']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='registracija']"), "Stranica registracije nije prikazana");

            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaRegistracija.png" });
        }
        [Test]
        public async Task RegistracijaKorisnikaTest()
        {

            await page.GotoAsync("http://localhost:3000/Registracija");
            await page.GetByTestId("ime").FillAsync("Aki");

            await page.GetByTestId("prezime").FillAsync("Laki");
            await page.GetByTestId("email").FillAsync("korisnikaki@gmail.com");
            await page.GetByTestId("sifra").FillAsync("aki12345");
            await page.GetByTestId("telefon").FillAsync("3201233210");
            await page.GetByTestId("datum").FillAsync("4.8.1970");
            await page.GetByTestId("grad").FillAsync("New Test");
            await page.GetByTestId("adresa").FillAsync("Testovska,20");
            await Expect(page.GetByText(" Registrujte se")).ToBeVisibleAsync();
            await page.GetByText(" Registrujte se").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.WaitForSelectorAsync("[data-testid='papir-prijava']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='papir-prijava']"), "Stranica prijave nije prikazana");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaPrijava1.png" });
        }
        [Test]
        public async Task PrelazakNaPrijavuTest()
        {
            await page.GotoAsync("http://localhost:3000/Registracija");
            await Expect(page.GetByRole(AriaRole.Link)).ToBeVisibleAsync();
            await page.GetByRole(AriaRole.Link).ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava#");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
        }
        [TearDown]
        public async Task Teardown()
        {
            await page.CloseAsync();
            await browser.DisposeAsync();
        }
    }
}
