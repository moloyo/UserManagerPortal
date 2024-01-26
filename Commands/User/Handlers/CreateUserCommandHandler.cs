using DataAccessLayer;
using MediatR;

namespace Commands.User.Handlers
{
    public class CreateUserCommandHandler(UserContext context) : IRequestHandler<CreateUserCommand, Models.User>
    {
        public async Task<Models.User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Models.User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                Credits = request.Credits
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();

            return user;
        }
    }
}
