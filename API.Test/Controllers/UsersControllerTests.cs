using API.Controllers;
using Commands.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;
using Queries.User;

namespace API.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new UsersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var userList = new List<User>();
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userList);

            // Act
            var result = await _controller.GetUsers();

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
            var user = new User() { Id = userId };
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetUserByIdQuery>(q => q.Id == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(userId);

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

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutUser_WithValidIdAndMatchingUser_ReturnsNoContentResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User();
            _mediatorMock
               .Setup(m => m.Send(It.Is<UpdateUserCommand>(q => q.Id == userId), It.IsAny<CancellationToken>()))
               .ReturnsAsync(user);

            // Act
            var result = await _controller.PutUser(userId, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostUser_ReturnsCreatedAtActionResult_WithNewUser()
        {
            // Arrange
            var newUser = new User { FullName = "NewUser" };
            _mediatorMock
               .Setup(m => m.Send(It.Is<CreateUserCommand>(q => q.FullName == newUser.FullName), It.IsAny<CancellationToken>()))
               .ReturnsAsync(newUser);

            // Act
            var result = await _controller.PostUser(newUser);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsType<User>(createdAtActionResult?.Value);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mediatorMock
               .Setup(m => m.Send(It.Is<DeleteUserCommand>(q => q.Id == userId), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new User());

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
