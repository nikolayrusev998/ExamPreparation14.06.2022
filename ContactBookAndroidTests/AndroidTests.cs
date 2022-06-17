using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace ContactBookAndroidTests
{
    public class AndroidTests
    {

        private AndroidDriver <AndroidElement> driver;
        private AppiumOptions options;
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactsBookURL = "https://contactbook.nikolayrusev1.repl.co/";
        private const string location = @"D:\contactbook-androidclient.apk";


        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Android"};
            options.AddAdditionalCapability("app", location);

            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumUrl) ,options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void Close()
        {
            driver.Quit();

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}