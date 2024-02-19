using MediatR;

namespace Queries.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<Models.User>>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
