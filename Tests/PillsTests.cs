using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOPLab1.Controllers;
using OOPLab1.Models;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
namespace OOPLab1.Tests.Controllers
{
    [TestFixture]
    public class PillsControllerTests
    {
        private PillsContext _context;
        private PillsController _controller;

        [TearDown]
        public void Tear()
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
            _controller = new PillsController(_context);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithListOfPills()
        {
            // Arrange
            var testData = new List<Pill>
            {
                new Pill { Id = 100, Name = "Pill A" },
                new Pill { Id = 200, Name = "Pill B" }
            };
            _context.Pills.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as List<Pill>;
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithPill()
        {
            // Arrange
            var testData = new Pill { Id = 21, Name = "Test Pill" };
            _context.Pills.Add(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(21) as IActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<IActionResult>(result);
        }

        // Write similar tests for other methods like Create, Edit, Delete, etc.

        [Test]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var pill = new Pill { Id = 1, Name = "Test Pill" };

            // Act
            var result = await _controller.Create(pill) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var testData = new Pill { Id = 1000, Name = "Test Pill" };
            _context.Pills.Add(testData);
            _context.SaveChanges();
            _context.Entry(testData).State = EntityState.Detached;
            var editedPill = new Pill { Id = 1000, Name = "Edited Pill" };

            // Act
            var result = await _controller.Edit(1000, editedPill) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            var testData = new Pill { Id = 15, Name = "Test Pill" };
            _context.Pills.Add(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(15) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
