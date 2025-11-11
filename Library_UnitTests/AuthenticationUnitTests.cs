using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Library_backend.Context;
using Library_backend.Controllers;
using Library_backend.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.Extensions.Logging;
using Library_backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library_UnitTests
{
    [TestFixture]
    public class AuthenticationUnitTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private TokenService _tokenService;
        private Mock<IConfiguration> _configurationMock;
        private AuthenticationController _authenticationController;
        private Mock<ILogger<AuthenticationController>> logger = new Mock<ILogger<AuthenticationController>>();

        [SetUp]
        public void Setup()
        {
           // Mock UserManager
           var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
           _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Mock IConfiguration for TokenService
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["AdminSettings:SecretAdminCode"]).Returns("access");
            _configurationMock.Setup(c => c["JwtSettings:SecretKey"]).Returns("SuperSecretKey12345678901234567890");
            _configurationMock.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");
            _configurationMock.Setup(c => c["JwtSettings:ExpiryMinutes"]).Returns("30");

            // Use real TokenService with mocked IConfiguration
            _tokenService = new TokenService(_configurationMock.Object);

            // Initialize AuthenticationController with mocked UserManager, real TokenService, and logger
            _authenticationController = new AuthenticationController(_userManagerMock.Object, _tokenService, _configurationMock.Object, logger.Object);
        }

        [Test]
        public async Task RegisterUser_IfValidModel_ReturnsOk()
        {
            //Arrange
            var registerModel = new RegisterModel
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = "Test@1234",
            };

            _userManagerMock.Setup(m => m.FindByNameAsync("testuser")).ReturnsAsync((ApplicationUser)null);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny< ApplicationUser>(),It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _authenticationController.RegisterUser(registerModel);
            //Assert
            Assert.That(result,Is.InstanceOf<OkObjectResult>());
        }
    }
}
