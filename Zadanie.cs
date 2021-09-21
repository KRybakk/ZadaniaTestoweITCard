using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using Zadania;
using ZadaniaTestoweITCard.PageObjects;

namespace ZadaniaTestoweITCard
{
    [TestFixture("Chrome")]
    [TestFixture("Firefox")]   
    public class Zadanie{

        ExtentReports rep = ExtentBuilder.GetExtent();
        StartPage startPage;
        HowItWorksPage howItWorksPage;
        DevicesPage devicesPage;
        IWebDriver driver;
        WebDriverWait wait;
        string Browser;

        public Zadanie(string browser)
        {
            Browser = browser;
        }

        [SetUp]
        public void Init()
        {
            //Wybór przeglądarki na podstawie parametru w TestFixture 
            //i maksymalizacja okna podczas inicjalizacji Drivera
            switch (Browser)
            {
                case "Chrome":
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("start-maximized");
                    driver = new ChromeDriver(chromeOptions);
                    break;
                case "Firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArguments("start-maximized");
                    driver = new FirefoxDriver(firefoxOptions);
                    break;
                default:
                    chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("start-maximized");
                    driver = new ChromeDriver(chromeOptions);
                    break;
            }

            driver.Manage().Cookies.DeleteAllCookies();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Url = "https://planetpay.pl/";

            startPage = new StartPage();
            PageFactory.InitElements(driver, startPage);

            howItWorksPage = new HowItWorksPage();
            PageFactory.InitElements(driver, howItWorksPage);

            devicesPage = new DevicesPage();
            PageFactory.InitElements(driver, devicesPage);

        }

        [Test]
        public void CheckIfButtonContainsText()
        {
            var test = rep.CreateTest("Check if button contains text \"skontaktuj się z nami już dziś\"");


            //Sprawdzenie czy test rozpoczyna się na odowiedniej stronie
            //jesli nie, test jest przerywany i do raportu dodawany jest zrzut ekranu
            try
            {
                Assert.That(driver.Url, Is.EqualTo("https://planetpay.pl/"));
                test.Pass("Starting site on https://planetpay.pl/");
            }
            catch(Exception ex)
            {
                test.Fail("Wrong sarting site");

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(ExtentBuilder.ReportFolderPath + @"\wrongSiteZad1.png", 
                    ScreenshotImageFormat.Png);
                test.Info("Details", MediaEntityBuilder.CreateScreenCaptureFromPath
                    (ExtentBuilder.ReportFolderPath + @"\wrongSiteZad1.png").Build());

                throw ex;
            }

            //Zamknięcie okienka z początkowym formularzem
            ClosePopupIfElementNotInteractableAndInteract(startPage.ClosePopupButton,
                startPage.ContactPopUpCloseButton, wait);

            //Naciśnięcie przycisku Terminale POS
            ClosePopupIfElementNotInteractableAndInteract(startPage.HowItWorksButton,
                startPage.ContactPopUpCloseButton, wait);


            //Oczekiwanie aż przycsk będzie widoczny i dostępny
            wait.Until(d => howItWorksPage.ContactButton.Displayed && howItWorksPage.ContactButton.Enabled);

            //Sprawdzenie czy przycisk zawiera tekst "skontaktuj się z nami już dziś"
            //jesli nie, test jest przerywany i do raportu dodawany jest zrzut ekranu
            try
            {
                Assert.That(howItWorksPage.ContactButton.Text, Does.Contain("skontaktuj się z nami już dziś"));
                test.Pass("Button contains expected text");
            }
            catch(Exception ex)
            {
                test.Fail("Button does not contain expected text");

                ScrollToView(howItWorksPage.ContactButton, driver);

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(ExtentBuilder.ReportFolderPath + @"\butonNotFound.png", 
                    ScreenshotImageFormat.Png);
                test.Info("Details", MediaEntityBuilder.CreateScreenCaptureFromPath
                    (ExtentBuilder.ReportFolderPath + @"\butonNotFound.png").Build());
                throw ex;
            }
        }

        [Test]
        public void CheckAmountOfDevices()
        {
            var test = rep.CreateTest("Check if site contains 10 devices and starting site title contains \"PlanetPay / Strona główna\"");

            //Sprawdzenie czy test rozpoczyna się na odowiedniej stronie
            //jesli nie, test jest przerywany i do raportu dodawany jest zrzut ekranu
            try
            {
                Assert.That(driver.Url, Is.EqualTo("https://planetpay.pl/"));
                test.Pass("Starting site on https://planetpay.pl/");
            }
            catch (Exception ex)
            {
                test.Fail("Wrong sarting site");

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(ExtentBuilder.ReportFolderPath + @"\wrongSiteZad2.png", 
                    ScreenshotImageFormat.Png);
                test.Info("Details", MediaEntityBuilder.CreateScreenCaptureFromPath
                    (ExtentBuilder.ReportFolderPath + @"\wrongSiteZad2.png").Build());

                throw ex;
            }

            //Zamknięcie okienka z początkowym formularzem
            ClosePopupIfElementNotInteractableAndInteract(startPage.ClosePopupButton, 
                startPage.ContactPopUpCloseButton, wait);

            //Naciśnięcie przycisku Terminale POS
            ClosePopupIfElementNotInteractableAndInteract(startPage.HowItWorksButton, 
                startPage.ContactPopUpCloseButton, wait);

            //Naciśnięcie przycisku Urządzenia
            howItWorksPage.DevicesButton.Click();


            //Sprawdzenie czy strona zwiera dokładnie 10 urządzeń
            //jesli nie, test jest przerywany i do raportu dodawany jest zrzut ekranu
            try
            {
                Assert.That(devicesPage.Devices.Count, Is.EqualTo(10));
                test.Pass("Site conains exactlu 10 devices");
            }
            catch (Exception ex)
            {
                test.Fail("Site contains differnt number of devices than 10");

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(ExtentBuilder.ReportFolderPath + @"\wrongDevicesAmount.png", 
                    ScreenshotImageFormat.Png);
                test.Info("Details", MediaEntityBuilder.CreateScreenCaptureFromPath
                    (ExtentBuilder.ReportFolderPath + @"\wrongDevicesAmount.png").Build());

                throw ex;
            }

            devicesPage.Logo.Click();

            //Sprawdzenie czy tytuł strony zawiera tekst "PlanetPay / Strona główna"
            //jesli nie, test jest przerywany i do raportu dodawany jest zrzut ekranu
            try
            {
                Assert.That(driver.Title, Does.Contain("PlanetPay / Strona główna"));
                test.Pass("Site title contains text \"PlanetPay / Strona główna\"");
            }
            catch(Exception ex)
            {
                test.Fail("Site title does not contain text \"PlanetPay / Strona główna\"");

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(ExtentBuilder.ReportFolderPath + @"\wrongSiteTitle.png", 
                    ScreenshotImageFormat.Png);
                test.Info("Details", MediaEntityBuilder.CreateScreenCaptureFromPath
                    (ExtentBuilder.ReportFolderPath + @"\wrongSiteTitle.png").Build());

                throw ex;
            }
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Close();
            rep.Flush();
        }

        //Metoda któej zadaniem jest naciśnięcie elementu nawet w przypadku, gdy zostanie 
        //on przysłonięty wyskakującym okienkiem z zapytaniem o kontak telefoniczny
        public static void ClosePopupIfElementNotInteractableAndInteract(IWebElement elementToInteract, 
            IWebElement popupToClose, WebDriverWait wait)
        {
            try
            {
                elementToInteract.Click();
            }
            catch (Exception ex)
            {
                popupToClose.Click();
                wait.Until(d => elementToInteract.Displayed && elementToInteract.Enabled);
                elementToInteract.Click();
            }
        }

        //Metoda przesuwająca stronę, aby element znalazł się w polu widocznym dla użytkownika
        public static void ScrollToView(IWebElement element, IWebDriver driver)
        {
            if (element.Location.Y > 200)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                var jstring = String.Format("window.scrollTo({0}, {1})", 0, element.Location.Y - 100);

                string title = (string)js.ExecuteScript(jstring);
            }

        }
    }


}
