using EventSourching.Application.Abstractions;
using EventSourching.Domain.Aggregates.UserAggregate;
using EventSourching.Domain.Aggregates.UserAggregate.Events;
using EventSourching.Domain.Aggregates.UserAggregate.ValueObjects;
using EventSourching.Domain.Common.Abstractions;
using MediatR;

namespace EventSourching.Application.Features.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, bool>
    {
        private readonly IDomainEventRepository<User, UserId> _userRepo;
        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(IDomainEventRepository<User, UserId> userRepo, IUserRepository userRepository)
        {
            _userRepo = userRepo;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = User.Create(Guid.NewGuid(), request.userCreateViewModel.Username, request.userCreateViewModel.Email, request.userCreateViewModel.Password);

                await _userRepository.AddAsync(user);

                Guid eventId = Guid.NewGuid();

                var userEvent = new UserCreatedDomainEvent(eventId, user.Username, user.Email, user.Password);
                userEvent.AddSerializeData(userEvent);
                user.AddUserDomainEvent(userEvent, typeof(UserCreatedDomainEvent));

                var roleEvent = new RoleCreatedDomainEvent(eventId, request.userCreateViewModel.Email);
                roleEvent.AddSerializeData(roleEvent);
                user.AddUserDomainEvent(roleEvent, typeof(RoleCreatedDomainEvent));

                var userStateEvent = new UserStateUpdateDomainEvent(eventId, user.Id.Id, request.userCreateViewModel.Email);
                userStateEvent.AddSerializeData(userStateEvent);
                user.AddUserDomainEvent(userStateEvent, typeof(UserStateUpdateDomainEvent));

                bool isSucceess = await _userRepo.SendAsync(user);

                await _userRepository.SaveChangesAsync();

                return isSucceess;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
