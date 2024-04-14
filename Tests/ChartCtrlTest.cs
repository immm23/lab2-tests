using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOPLab1.Controllers;
using OOPLab1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tests
{

    [TestFixture]
    public class ChartControllerTests
    {
        private PillsContext _context;
        private ChartController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PillsContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            _context = new PillsContext(options);
            _controller = new ChartController(_context);
        }

        [TearDown]
        public void Tear()
        {
            _context.Dispose();
        }

        [Test]
        public void JsonClassData_ReturnsJsonResult()
        {
            // Arrange
            var testData = new List<PillClass>
            {
                new PillClass { Name = "Class A", Pills = new List<Pill>(), Id = 1000, },
                new PillClass { Name = "Class B", Pills = new List<Pill>(), Id = 2000  }
            };
            _context.PillClasses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = _controller.JsonClassData() as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<JsonResult>(result);
        }

        [Test]
        public void JsonPharmasyData_ReturnsJsonResult()
        {
            // Arrange
            var testData = new List<Pharmasy>
            {
                new Pharmasy { Name = "Pharmacy A", Pills = new List<Pill>(), Adress = "a", OwnerName = "ow", PhoneNumber = "ph"},
                new Pharmasy { Name = "Pharmacy B", Pills = new List<Pill>(),  Adress = "a", OwnerName = "ow", PhoneNumber = "ph" }
            };
            _context.Pharmasies.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = _controller.JsonPharmasyData() as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<JsonResult>(result);
        }

        // Add more tests as needed to cover different scenarios
    }
}
