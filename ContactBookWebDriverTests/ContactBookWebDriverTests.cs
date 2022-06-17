using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace ContactBookWebDriverTests
{
    public class UITests
    {

        private const string url = "https://contactbook.nikolayrusev1.repl.co/";
        private WebDriver driver;

        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait =  TimeSpan.FromSeconds(5);

        }
        [TearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ListContacts_CheckFirstContact()
        {
            //Arragne
            driver.Navigate().GoToUrl(url);
            var contactsLink = driver.FindElement(By.LinkText("Contacts"));

            //Act
            contactsLink.Click();


            //Assert
            var firstName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td")).Text;

            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));


        }
        [Test]
        public void Test_SearchContacts_FindByKeyword()
        {
            //Arragne
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3)")).Click();

            driver.FindElement(By.Id("keyword")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys("Albert");
            driver.FindElement(By.Id("keyword")).SendKeys(Keys.Enter);

            var firstName = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.lname > td")).Text;


            Assert.That(firstName, Is.EqualTo("Albert"));
            Assert.That(lastName, Is.EqualTo("Einstein"));

        }
        [Test]
        public void Test_SearchContacts_InvalidData()
        {
            //Arragne
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3)")).Click();

            driver.FindElement(By.Id("keyword")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys("invalid2635");
            driver.FindElement(By.Id("keyword")).SendKeys(Keys.Enter);

            var searchResult = driver.FindElement(By.Id("searchResult")).Text;
            


            
            Assert.That(searchResult, Is.EqualTo("No contacts found."));

        }
        [Test]
        public void Test_CreateContact_InvalidData()
        {
            //Arragne
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2)")).Click();

            driver.FindElement(By.Id("firstName")).Click();
            driver.FindElement(By.Id("firstName")).SendKeys("asd");
            driver.FindElement(By.Id("lastName")).Click();
            driver.FindElement(By.Id("lastName")).SendKeys("asd");
            driver.FindElement(By.Id("email")).Click();
            driver.FindElement(By.Id("email")).SendKeys("asd");
            driver.FindElement(By.Id("phone")).Click();
            driver.FindElement(By.Id("phone")).SendKeys("asd");
            driver.FindElement(By.Id("comments")).Click();
            driver.FindElement(By.Id("comments")).SendKeys("asd");
            driver.FindElement(By.Id("create")).Click();

            var errorMsg =  driver.FindElement(By.XPath("/html/body/main/div")).Text;

            Assert.That(errorMsg, Is.EqualTo("Error: Invalid email!"));

        }

        [Test]
        public void Test_CreateContact_ValidData()
        {

            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var firstName = "FirstName" + DateTime.Now.Ticks;
            var lastName = "LastName" + DateTime.Now.Ticks;
            var email = DateTime.Now.Ticks + "randommail@abv.bg";
            var phone = "12345";

            // Act
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("phone")).SendKeys(phone);

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            // Assert
            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();

            var firstNameLabel = lastContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastNameLabel = lastContact.FindElement(By.CssSelector("tr.lname > td")).Text;

            Assert.That(firstNameLabel, Is.EqualTo(firstName));
            Assert.That(lastNameLabel, Is.EqualTo(lastName));




        }
    }
}