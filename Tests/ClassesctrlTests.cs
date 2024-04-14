using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using OOPLab1.Controllers;
using OOPLab1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OOPLab1.Tests.Controllers
{

    [TestFixture]
    [Category("A")]
    [Category("B")]
    public class ClassesControllerTests
    {
        private PillsContext _context;
        private ClassesController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PillsContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            _context = new PillsContext(options);
            _controller = new ClassesController(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            _controller.Dispose();
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithListOfClasses()
        {
            // Arrange
            var testData = new List<PillClass>
            {
                new PillClass { Id = 100, Name = "Class A" },
                new PillClass { Id = 102, Name = "Class B" }
            };
            _context.PillClasses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as List<PillClass>;
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithClass()
        {
            // Arrange
            var testData = new List<PillClass>
            {
                new PillClass { Id = 3, Name = "Class A" },
                new PillClass { Id = 4, Name = "Class B" }
            };
            _context.PillClasses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(3) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as PillClass;
            Assert.IsNotNull(model);
            Assert.AreEqual("Class A", model.Name);
        }

        [Test]
        public async Task Create_ReturnsViewResult()
        {
            // Act
            var result = (_controller.Create()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
        }

        [Test]
        public async Task Create_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var pillClass = new PillClass { Name = "Class A" };

            // Act
            var result = await _controller.Create(pillClass) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Edit_ReturnsViewResult_WithClass()
        {
            // Arrange
            var testData = new List<PillClass>
            {
                new PillClass { Id = 5, Name = "Class A" }
            };
            _context.PillClasses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Edit(5) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as PillClass;
            Assert.IsNotNull(model);
            Assert.AreEqual("Class A", model.Name);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WithClass()
        {
            // Arrange
            var testData = new List<PillClass>
            {
                new PillClass { Id = 89, Name = "Class A" }
            };
            _context.PillClasses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Delete(89) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as PillClass;
            Assert.IsNotNull(model);
            Assert.AreEqual("Class A", model.Name);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesClassAndRedirectsToIndex()
        {
            // Arrange
            var testData = new List<PillClass>
            {
                new PillClass { Id = 90, Name = "Class A" }
            };
            _context.PillClasses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(90) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.IsNull(_context.PillClasses.Find(90));
        }

        [Test]
        public void ClassExists_ReturnsTrue_WhenClassExists()
        {
            // Arrange
            var testData = new List<PillClass>
            {
                new PillClass { Id = 1, Name = "Class A" }
            };
            _context.PillClasses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = _controller.ClassExists(1);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ClassExists_ReturnsFalse_WhenClassDoesNotExist()
        {
            // Act
            var result = _controller.ClassExists(1);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
