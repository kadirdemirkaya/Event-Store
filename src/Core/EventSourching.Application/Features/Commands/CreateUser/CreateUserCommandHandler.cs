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
        private readonly ITriggeredEventRepository _triggeredEventRepository;
        public CreateUserCommandHandler(IDomainEventRepository<User, UserId> userRepo, IUserRepository userRepository, ITriggeredEventRepository triggeredEventRepository)
        {
            _userRepo = userRepo;
            _userRepository = userRepository;
            _triggeredEventRepository = triggeredEventRepository;
        }

        public async Task<bool> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = User.Create(Guid.NewGuid(), request.userCreateViewModel.Username, request.userCreateViewModel.Email, request.userCreateViewModel.Password);

                await _userRepository.AddAsync(user);

                var userEvent = new UserCreatedDomainEvent(Guid.NewGuid(), user.Username, user.Email, user.Password);
                userEvent.AddSerializeData(userEvent);
                user.AddUserDomainEvent(userEvent, typeof(UserCreatedDomainEvent));

                var roleEvent = new RoleCreatedDomainEvent(Guid.NewGuid(), request.userCreateViewModel.Email);
                roleEvent.AddSerializeData(roleEvent);
                user.AddUserDomainEvent(roleEvent, typeof(RoleCreatedDomainEvent));

                //var userStateEvent = new UserStateUpdateDomainEvent(Guid.NewGuid(), user.Id.Id, request.userCreateViewModel.Email);
                //userStateEvent.AddSerializeData(userStateEvent);
                //user.AddUserDomainEvent(userStateEvent, typeof(UserStateUpdateDomainEvent));

                await _userRepository.SaveChangesAsync();

                bool isSucceess = await _userRepo.SendAsync(user);

                return isSucceess;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
