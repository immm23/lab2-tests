using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OOPLab1.Controllers;
using OOPLab1.Models;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OOPLab1.Tests.Controllers
{
    [TestFixture]
    [Category("A")]
    public class PharmasiesControllerTests
    {
        private PillsContext _context;
        private PharmasiesController _controller;

        [TearDown]
        public void TearDw()
        {
            _context.Dispose();
            _controller.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PillsContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            _context = new PillsContext(options);
            _controller = new PharmasiesController(_context);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithListOfPharmasies()
        {
            // Arrange
            var testData = new List<Pharmasy>
            {
                new Pharmasy { Id = 112, Name = "Pharmasy A",  Adress = "ad", OwnerName ="ow", PhoneNumber ="ph" },
                new Pharmasy { Id = 212, Name = "Pharmasy B",  Adress = "ad", OwnerName ="ow", PhoneNumber ="ph"}
            };
            _context.Pharmasies.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as List<Pharmasy>;
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithPharmasy()
        {
            // Arrange
            var testData = new Pharmasy {Id = 2000, Name = "Test Pharmasy", Adress = "ad", OwnerName = "ow", PhoneNumber = "ph" };
            _context.Pharmasies.Add(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(2000) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as Pharmasy;
            Assert.IsNotNull(model);
            Assert.AreEqual("Test Pharmasy", model.Name);
        }

        // Write similar tests for other methods like Create, Edit, Delete, etc.

        [Test]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var pharmasy = new Pharmasy { Id = 1, Name = "Test Pharmasy", Adress = "ad", OwnerName = "ow", PhoneNumber = "ph" };

            // Act
            var result = await _controller.Create(pharmasy) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var testData = new Pharmasy { Id = 4000, Name = "Test Pharmasy", Adress = "ad", OwnerName ="ow", PhoneNumber ="ph"};
            _context.Pharmasies.Add(testData);
            _context.Entry(testData).State = EntityState.Detached;
            _context.SaveChanges();
            var editedPharmasy = new Pharmasy { Id = 4000, Name = "Edited Pharmasy", Adress = "ad", OwnerName = "ow", PhoneNumber = "ph" };

            // Act
            var result = await _controller.Edit(4000, editedPharmasy) as IActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            var testData = new Pharmasy {Id = 1000, Name = "Test Pharmasy", Adress = "ad", OwnerName = "ow", PhoneNumber = "ph" };
            _context.Pharmasies.Add(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(1000) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
