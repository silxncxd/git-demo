﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Selenium
{
    [TestFixture]
    public class WebTests
    {
        public IWebDriver webDriver;
        WebDriverWait wait;

        [SetUp]
        public void StartBrowserAndGoToTheSite()
        {
            webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            webDriver.Navigate().GoToUrl("https://travel.ldz.lv/");
        }

        [TearDown]
        public void QuitDriver()
        {
            webDriver.Quit();
        }

        [Test]
        //test 1
        public void SearchTrips()
        {
            string departure = "RĪGA PASAŽIERU";
            string arrival = "MINSKA PASAŽIERU";

            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));

            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var departureStation = webDriver.FindElement(By.XPath("//input[@name = 'from']"));
            departureStation.SendKeys(departure);

            var arrivalStation = webDriver.FindElement(By.XPath("//input[@name = 'to']"));
            arrivalStation.SendKeys(arrival);

            string todayDate = DateTime.Now.ToShortDateString();
            var dateInput = webDriver.FindElement(By.XPath("//input[@name = 'departure']"));
            dateInput.SendKeys(todayDate);

            var cancelReturnTripButton = webDriver.FindElement(By.XPath("//span[@class='dotted-link js-book-tickets-oneway-trigger']"));
            cancelReturnTripButton.Click();

            wait.Until(condition: ExpectedConditions.ElementToBeClickable(By.CssSelector(".button.with-outlines")));
            var sendRequestButton = webDriver.FindElement(By.CssSelector(".button.with-outlines"));
            sendRequestButton.Click();

            var correctDepartureStation = webDriver.FindElement(By.XPath("//span[@class = 'from']"));
            var correctArrivalStation = webDriver.FindElement(By.XPath("//span[@class = 'to']"));

            string correctTrip = departure + arrival;
            string returnedTrip = (correctDepartureStation.Text + correctArrivalStation.Text).ToUpper();

            Assert.AreEqual(correctTrip, returnedTrip);
        }

        [Test]
        //test 2
        public void SameStationError()
        {
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var departureStation = webDriver.FindElement(By.XPath("//input[@name = 'from']"));
            departureStation.SendKeys("RĪGA PASAŽIERU");

            var arrivalStation = webDriver.FindElement(By.XPath("//input[@name = 'to']"));
            arrivalStation.SendKeys("RĪGA PASAŽIERU");

            string todayDate = DateTime.Now.ToShortDateString();
            var dateInput = webDriver.FindElement(By.XPath("//input[@name = 'departure']"));
            dateInput.SendKeys(todayDate);

            var cancelReturnTripButton = webDriver.FindElement(By.XPath("//span[@class='dotted-link js-book-tickets-oneway-trigger']"));
            cancelReturnTripButton.Click();

            wait.Until(condition: ExpectedConditions.ElementToBeClickable(By.CssSelector(".button.with-outlines")));
            var sendRequestButton = webDriver.FindElement(By.CssSelector(".button.with-outlines"));
            sendRequestButton.Click();

            var errorText = webDriver.FindElement(By.CssSelector(".modal-title")).Text;
            Assert.AreEqual("KĻŪDA!", errorText);
        }
    }
}
