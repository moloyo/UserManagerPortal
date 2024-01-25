using API.Controllers;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Test.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<User> { new User { Id = Guid.NewGuid(), FullName = "User1" } });

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.GetUsers();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsType<List<User>>(okResult?.Value);
        }

        [Fact]
        public async Task GetUser_WithValidId_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FullName = "TestUser" };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(user);

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.GetUser(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsType<User>(okResult?.Value);
            Assert.Equal(userId, (okResult?.Value as User)?.Id);
        }

        [Fact]
        public async Task GetUser_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync((User)null);

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.GetUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutUser_WithValidIdAndMatchingUser_ReturnsNoContentResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FullName = "TestUser" };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.UserExistsAsync(userId)).ReturnsAsync(true);

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.PutUser(userId, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutUser_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = Guid.NewGuid(), FullName = "TestUser" }; // Different Id

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.UserExistsAsync(userId)).ReturnsAsync(true);

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.PutUser(userId, user);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostUser_ReturnsCreatedAtActionResult_WithNewUser()
        {
            // Arrange
            var newUser = new User { Id = Guid.NewGuid(), FullName = "NewUser" };

            var userRepositoryMock = new Mock<IUserRepository>();

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.PostUser(newUser);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsType<User>(createdAtActionResult?.Value);
            Assert.Equal(newUser.Id, (createdAtActionResult?.Value as User)?.Id);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.UserExistsAsync(userId)).ReturnsAsync(true);

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.UserExistsAsync(userId)).ReturnsAsync(false);

            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = await controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
