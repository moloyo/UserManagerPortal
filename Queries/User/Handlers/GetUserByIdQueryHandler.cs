using DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Queries.User.Handlers
{
    public class GetUserByIdQueryHandler(UserContext context) : IRequestHandler<GetUserByIdQuery, Models.User?>
    {
        public async Task<Models.User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
