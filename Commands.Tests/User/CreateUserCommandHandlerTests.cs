using Commands.Test;
using Commands.User;
using Commands.User.Handlers;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Commands.Tests.User.Handlers
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<UserContext> _contextMock;
        private readonly CreateUserCommandHandler _commandHandler;

        public CreateUserCommandHandlerTests()
        {
            _contextMock = new Mock<UserContext>();

            List<Models.User> data = [new Models.User { FullName = "Test user" }];

            var mockSet = new Mock<DbSet<Models.User>>();
            mockSet.As<IAsyncEnumerable<Models.User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Models.User>(data.GetEnumerator()));
            mockSet.As<IQueryable<Models.User>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<Models.User>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<Models.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _contextMock.Setup(c => c.Users).Returns(mockSet.Object);

            _commandHandler = new CreateUserCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAddedUser()
        {
            // Arrange
            var command = new CreateUserCommand();
            var tokenSource = new CancellationTokenSource();

            // Act
            var result = await _commandHandler.Handle(command, tokenSource.Token);

            // Assert
            Assert.IsType<Models.User>(result);
        }

        [Fact]
        public async Task GetUsers_CallsContextAdd()
        {
            // Arrange
            var command = new CreateUserCommand() { FullName = "Test User"};
            var tokenSource = new CancellationTokenSource();

            // Act
            var result = await _commandHandler.Handle(command, tokenSource.Token);

            // Assert
            _contextMock.Verify(c => c.Users.Add(It.Is<Models.User>(u => u.FullName == command.FullName)), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
