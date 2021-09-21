using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace ZadaniaTestoweITCard.PageObjects
{
    class HowItWorksPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//a[@href='/kontakt' and @target]")]
        public IWebElement ContactButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='/urzadzenia']")]
        public IWebElement DevicesButton { get; set; }

    }
}
