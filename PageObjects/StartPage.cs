using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace ZadaniaTestoweITCard.PageObjects
{
    class StartPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//button[@tabindex=-1]")]
        public IWebElement ContactPopUpCloseButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@role='button' and @href='#']")]
        public IWebElement ClosePopupButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='/jak-to-dziala']")]
        public IWebElement HowItWorksButton { get; set; }

    }
}
