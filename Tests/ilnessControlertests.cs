using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using OOPLab1.Controllers;
using OOPLab1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OOPLab1.Tests.Controllers
{
    [TestFixture]
    public class IlnessesControllerTests
    {
        private PillsContext _context;
        private IlnessesController _controller;

        [TearDown]
        public void Tearr()
        {
            _controller.Dispose();
            _context.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PillsContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            _context = new PillsContext(options);
            _controller = new IlnessesController(_context);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithListOfIlnesses()
        {
            // Arrange
            var testData = new List<Ilness>
            {
                new Ilness { Id = 120, Name = "Ilness A", Description = "des", Symptoms = "sympt" },
                new Ilness {Id = 122, Name = "Ilness B", Description = "des", Symptoms = "sympt"}
            };
            _context.Ilnesses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as List<Ilness>;
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithIlness()
        {
            // Arrange
            var testData = new List<Ilness>
            {
                new Ilness {Id = 10, Name = "Ilness A", Description = "des", Symptoms = "sympt"},
                new Ilness {Id = 20, Name = "Ilness B", Description = "des", Symptoms = "sympt"}
            };
            _context.Ilnesses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(10) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as Ilness;
            Assert.IsNotNull(model);
            Assert.AreEqual("Ilness A", model.Name);
        }

        [Test]
        public async Task Create_ReturnsViewResult()
        {
            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
        }

        [Test]
        public async Task Create_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new Ilness { Name = "Test Ilness", Description = "des", Symptoms = "sympt"};

            // Act
            var result = await _controller.Create(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Edit_ReturnsViewResult_WithIlness()
        {
            // Arrange
            var testData = new List<Ilness>
            {
                new Ilness { Id = 100, Name = "Ilness A", Description = "des", Symptoms = "sympt" },
                new Ilness { Id = 200, Name = "Ilness B", Description = "des", Symptoms = "sympt" }
            };
            _context.Ilnesses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Edit(100) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as Ilness;
            Assert.IsNotNull(model);
            Assert.AreEqual("Ilness A", model.Name);
        }

        [Test]
        public async Task Edit_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new Ilness { Id = 1, Name = "Test Ilness", Description = "des", Symptoms = "sympt" };

            // Act
            var result = await _controller.Edit(1, model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WithIlness()
        {
            // Arrange
            var testData = new List<Ilness>
            {
                new Ilness {Id = 5, Name = "Ilness A", Description = "des", Symptoms = "sympt"},
                new Ilness { Id = 6, Name = "Ilness B", Description = "des", Symptoms = "sympt" }
            };
            _context.Ilnesses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.Delete(5) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
            var model = result.Model as Ilness;
            Assert.IsNotNull(model);
            Assert.AreEqual("Ilness A", model.Name);
        }

        public static IEnumerable<object[]> DeleteConfirmedTestCases()
        {
            yield return new object[] { 1000, "Index" };
            yield return new object[] { 2000, "Index" };
            yield return new object[] { 4000, "Index" };
            // Add more test cases as needed
        }

        [Test, TestCaseSource(nameof(DeleteConfirmedTestCases))]
        public async Task DeleteConfirmed_DeletesIlness_RedirectsToIndex(int id, string expectedAction)
        {
            // Arrange
            var testData = new List<Ilness>
            {
                new Ilness {Id = id, Name = "Ilness A", Description = "des", Symptoms = "sympt"},
                new Ilness { Id = id+1, Name = "Ilness B", Description = "des", Symptoms = "sympt" }
            };
            _context.Ilnesses.AddRange(testData);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAction, result.ActionName);
            Assert.IsNull(_context.Ilnesses.Find(id));
        }

    }
}
