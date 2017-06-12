using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinalYearProject.Api;
using FinalYearProject.Api.Controllers;
using FinalYearProject.Domain;
using FinalYearProject.Search.BaseClasses;

namespace FinalYearProject.Api.Tests.Controllers
{
    [TestClass]
    public class SearchTest
    {
        [TestMethod]
        public void Get()
        {
            var search = new SearchRepository<Event>("event", "event");
            var results =search.GetAll(0, 100);
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            var search = new SearchRepository<Event>("event", "event");
            
            // Act
            //search.GetById("AVnlQDzDFa3gWpnNqK_q");
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            var search = new SearchRepository<Event>("event", "event");
            var test = new Event();
            test.CreatedAt = DateTime.Today;
            test.Description = "Test object";

            //test.Type = 0;
            // Act
            search.Add(test);

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            var onlinesearch = new Search.ProductSearchRepository("user", "fyp");
            onlinesearch.CreateMap();
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
