using API.Models;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Test.Repositories
{
    public class UserRepositoryTest
    {
        private readonly Mock<UserContext> _mockContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<UserContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            var data = new List<User>
            {
                new User { FullName = "Test user" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<User>(data.GetEnumerator()));
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext = new Mock<UserContext>(options);
            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            _userRepository = new UserRepository(_mockContext.Object);
        }

        [Fact]
        public async Task CreateAsync_AddsUserToContextAndSavesChanges()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), FullName = "TestUser" };

            // Act
            await _userRepository.CreateAsync(user);

            // Assert
            _mockContext.Verify(c => c.Users.Add(user), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUserFromContextAndSavesChanges_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FullName = "TestUser" };

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.Setup(d => d.FindAsync(userId)).ReturnsAsync(user);

            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            await _userRepository.DeleteAsync(userId);

            // Assert
            _mockContext.Verify(c => c.Users.Remove(user), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DoesNotRemoveUserFromContext_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.Setup(d => d.FindAsync(userId)).ReturnsAsync((User)null);

            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            await _userRepository.DeleteAsync(userId);

            // Assert
            _mockContext.Verify(c => c.Users.Remove(It.IsAny<User>()), Times.Never);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfUsers()
        {
            // Arrange
            var userList = new List<User>
            {
                new User { Id = Guid.NewGuid(), FullName = "User1" },
                new User { Id = Guid.NewGuid(), FullName = "User2" }
            };

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<User>(userList.GetEnumerator()));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userList.AsQueryable().Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.AsQueryable().Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => userList.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _userRepository.GetAllAsync();

            // Assert
            Assert.Equal(userList, result);
        }

        [Fact]
        public async Task GetAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FullName = "TestUser" };

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.Setup(d => d.FindAsync(userId)).ReturnsAsync(user);

            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _userRepository.GetAsync(userId);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.Setup(d => d.FindAsync(userId)).ReturnsAsync((User)null);

            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _userRepository.GetAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesUserInContextAndSavesChanges()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), FullName = "TestUser" };

            // Act
            await _userRepository.UpdateAsync(user);

            // Assert
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userList = new List<User> { new User { Id = userId, FullName = "TestUser" } };

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<User>(userList.GetEnumerator()));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(userList.AsQueryable().Provider));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.AsQueryable().Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => userList.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _userRepository.UserExistsAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userList = new List<User> { new User { Id = Guid.NewGuid(), FullName = "TestUser" } };

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<User>(userList.GetEnumerator()));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(userList.AsQueryable().Provider));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.AsQueryable().Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => userList.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _userRepository.UserExistsAsync(userId);

            // Assert
            Assert.False(result);
        }
    }
}
