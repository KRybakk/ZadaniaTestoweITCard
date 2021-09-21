using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace ZadaniaTestoweITCard.PageObjects
{
    class DevicesPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//div[@id='services']/div[@class='service']")]
        public IList<IWebElement> Devices { get; set; }

        [FindsBy(How = How.XPath, Using = "//h1[contains(@class,'logo')]/a[@href='/']")]
        public IWebElement Logo { get; set; }
    }
}
