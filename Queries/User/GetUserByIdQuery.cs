using MediatR;

namespace Queries.User
{
    public class GetUserByIdQuery : IRequest<Models.User?>
    {
        public Guid Id { get; set; }
    }
}
