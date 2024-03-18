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
    public class SveAgencijeTest:PageTest
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
        public async Task FormaSveAgencijeTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");

            await Expect(page.GetByText("Agencije koje svoje ponude stavljaju na platformi \"Kruzeri\"")).ToBeVisibleAsync();
            await page.WaitForSelectorAsync("[data-testid='Agencija0']");
            Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='Agencija0']"), "Neuspelo pribavljanje agencije");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSveAgencije1.png" });
        }
        [Test]
        public async Task KlikNaAzurirajAgencijuTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.WaitForSelectorAsync("[data-testid='Agencija0']");
            await page.GetByTestId("Agencija-Azuriraj0").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/CitajAzurirajAgenciju");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSveAgencije2.png" });
        }
        [Test]
        public async Task KlikNaObrisiAgencijuTest()
        {
            await page.GotoAsync("http://localhost:3000/");
            await page.GetByText("PRIJAVI SE").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/Prijava");
            await page.GetByLabel("Email Address").FillAsync("adminaki@gmail.com");
            await page.GetByLabel("Password").FillAsync("admin123");
            await Expect(page.GetByText(" Prijavi se")).ToBeVisibleAsync();
            await page.GetByTestId("prijava").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.GetByTestId("Agencija-Obrisi0").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/OkStranica");
            await page.GetByText("OK").ClickAsync();
            await page.WaitForURLAsync("http://localhost:3000/SveAgencije");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/stranicaSveAgencije3.png" });

        }
    }
}
