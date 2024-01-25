using DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Queries.User.Handlers
{
    public class GetAllUsersQueryHandler(UserContext context) : IRequestHandler<GetAllUsersQuery, IEnumerable<Models.User>>
    {
        public async Task<IEnumerable<Models.User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await context.Users.ToListAsync();
        }
    }
}
