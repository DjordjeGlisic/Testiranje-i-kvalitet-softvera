using System;
using Microsoft.Playwright;
using System.Runtime.Intrinsics.X86;
using Microsoft.Extensions.Configuration;
using Kruzeri.Controllers;
using Kruzeri.Models;

namespace KruzeriPlayWright
{
    public class ObrisiKorisnikaTest:PageTest
    {
        [Parallelizable(ParallelScope.Self)]
        [TestFixture]
        public class AzurirajBrisiOdjaviKorisnikaTest : PageTest
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
                await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaCitajAzurirajKorisnika4.png" });

            }
        }
    }
}
