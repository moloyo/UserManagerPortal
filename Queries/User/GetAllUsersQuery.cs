using MediatR;

namespace Queries.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<Models.User>>
    {
    }
}
