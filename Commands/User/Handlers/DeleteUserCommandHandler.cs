using DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commands.User.Handlers
{
    public class DeleteUserCommandHandler(UserContext context) : IRequestHandler<DeleteUserCommand, Models.User?>
    {
        public async Task<Models.User?> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (user is null)
                return default;

            context.Remove(user);

            await context.SaveChangesAsync();

            return user;
        }
    }
}
