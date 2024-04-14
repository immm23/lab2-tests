using NUnit.Framework;
using OOPLab1.Controllers;
using Microsoft.AspNetCore.Mvc;
using OOPLab1.Models;
using System.Diagnostics.CodeAnalysis;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OOPLab1.Tests.Controllers
{

    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new HomeController(null);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
        }

        [Test]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<ViewResult>(result);
        }

        [Test]
        public void Error_ReturnsViewResultWithModelError()
        {
            // Act
            var result = _controller.Error();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
