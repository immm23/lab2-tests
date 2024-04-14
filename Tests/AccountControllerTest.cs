using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OOPLab1.Controllers;
using OOPLab1.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
namespace Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [TearDown]
        public void Displose()
        {
            _controller.Dispose();
        }

        [Test]
        public void Register_Get_ReturnsView()
        {
            // Act
            var result = _controller.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Register_Post_FailedRegistration_ReturnsViewWithError()
        {
            // Arrange
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                            .ThrowsAsync(new InvalidOperationException("Failed to create user."));

            // Act & Assert
            Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                // Act
                var result = await _controller.Register(new RegisterViewModel()) as ViewResult;
            });
        }

        [Test]
        public async Task Login_Post_InvalidLogin_ReturnsViewWithError()
        {
            // Arrange
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                              .ThrowsAsync(new InvalidOperationException("Invalid login."));

            // Act & Assert
            Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                // Act
                var result = await _controller.Login(new LoginViewModel()) as ViewResult;
            });
        }

    }
}
