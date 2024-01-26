using Commands.Test;
using Commands.User;
using Commands.User.Handlers;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Commands.Tests.User.Handlers
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<UserContext> _contextMock;
        private readonly List<Models.User> _data;
        private readonly DeleteUserCommandHandler _commandHandler;

        public DeleteUserCommandHandlerTests()
        {
            _contextMock = new Mock<UserContext>();

            _data = [new Models.User { Id = new Guid(), FullName = "Test user" }];

            var mockSet = new Mock<DbSet<Models.User>>();
            var asyncProvider = new TestAsyncQueryProvider<Models.User>(_data.AsQueryable().Provider);
            var enumerator = new TestAsyncEnumerator<Models.User>(_data.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Models.User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(enumerator);
            mockSet.As<IQueryable<Models.User>>().Setup(m => m.Provider).Returns(asyncProvider);
            mockSet.As<IQueryable<Models.User>>().Setup(m => m.Expression).Returns(_data.AsQueryable().Expression);
            mockSet.As<IQueryable<Models.User>>().Setup(m => m.ElementType).Returns(_data.AsQueryable().ElementType);
            mockSet.As<IQueryable<Models.User>>().Setup(m => m.GetEnumerator()).Returns(() => _data.GetEnumerator());

            _contextMock.Setup(c => c.Users).Returns(mockSet.Object);

            _commandHandler = new DeleteUserCommandHandler(_contextMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsRemovedUser()
        {
            // Arrange
            var command = new DeleteUserCommand() { Id = _data[0].Id.Value };
            var tokenSource = new CancellationTokenSource();

            // Act
            var result = await _commandHandler.Handle(command, tokenSource.Token);

            // Assert
            Assert.IsType<Models.User>(result);
        }

        [Fact]
        public async Task Handle_CallsContextRemove()
        {
            // Arrange
            var command = new DeleteUserCommand() { Id = _data[0].Id.Value };
            var tokenSource = new CancellationTokenSource();

            // Act
            var result = await _commandHandler.Handle(command, tokenSource.Token);

            // Assert
            _contextMock.Verify(c => c.Remove(It.Is<Models.User>(u => u.Id == command.Id)), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
