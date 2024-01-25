using DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commands.User.Handlers
{
    public class UpdateUserCommandHandler(UserContext context) : IRequestHandler<UpdateUserCommand, Models.User?>
    {
        public async Task<Models.User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user is null)
                return default;

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.Credits = request.Credits;

            context.Users.Add(user);

            await context.SaveChangesAsync();

            return user;
        }
    }
}
