using MediatR;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Commands.User
{
    public class DeleteUserCommand : IRequest<Models.User?>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
