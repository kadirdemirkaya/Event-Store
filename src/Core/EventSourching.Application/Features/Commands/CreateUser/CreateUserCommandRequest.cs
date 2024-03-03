using EventSourching.Application.ViewModels;
using MediatR;

namespace EventSourching.Application.Features.Commands.CreateUser
{
    public record CreateUserCommandRequest(
        UserCreateViewModel userCreateViewModel
    ) : IRequest<bool>;
}
