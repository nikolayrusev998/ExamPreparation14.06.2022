using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace ContactBookAPITests
{
    public class APITests
    {

        private const string url = "https://contactbook.nikolayrusev1.repl.co/api/contacts";
        private RestClient client;
        private RestRequest request;


        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_GetAllClients_CheckFirstClient()
        {
            //Arrange
            this.request = new RestRequest(url);

            //Act
            var response = this.client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.That(contacts[0].firstName, Is.EqualTo("Steve"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Jobs"));

        }
        [Test]
        public void Test_SearchClient_CheckFirstResult()
        {
            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "albert");

            //Act
            var response = this.client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));

        }

        [Test]
        public void Test_SearchClient_EmptyResults()
        {
            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "missing2381283");

            //Act
            var response = this.client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.EqualTo(0));
          

        }

        [Test]
        public void Test_CreateContact_EmptyLastname()
        {
            //Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                firstName = "Gulia",
                email = "gulia@abv.bg",
                phone = "123123231"
            };
            request.AddJsonBody(body);

            //Act
            var response = this.client.Execute(request, Method.Post);
            

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));
            
            

        }

        [Test]
        public void Test_CreateContact_ValidData()
        {
            //Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                firstName = "Test",
                lastName = "Testerov",
                email = "testtesterov@abv.bg",
                phone = "123123231"
            };
            request.AddJsonBody(body);

            //Act
            var response = this.client.Execute(request, Method.Post);
            


            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            
            var allContacts =  this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(allContacts.Content);

            var lastContact = contacts.Last();


            Assert.That(lastContact.firstName, Is.EqualTo(body.firstName));
            Assert.That(lastContact.lastName, Is.EqualTo(body.lastName));

        }
    }
}