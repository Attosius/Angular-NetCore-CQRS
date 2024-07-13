using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PromomashInc.Core.Models;
using PromomashInc.DataAccess.Context;
using PromomashInc.DataAccess.Models;
using PromomashInc.EntitiesDto.Command;

namespace PromomashInc.Handlers.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly UserDataContext _userDataContext;
        private readonly IMapper _mapper;
        private readonly ICustomPasswordHasher _customPasswordHasher;

        public CreateUserCommandHandler(
            UserDataContext userDataContext,
            IMapper mapper,
            ICustomPasswordHasher customPasswordHasher
            )
        {
            _userDataContext = userDataContext;
            _mapper = mapper;
            _customPasswordHasher = customPasswordHasher;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            user.PasswordHash = _customPasswordHasher.GetHash(request.Password);
            _userDataContext.Entry(user).State = user.Id == 0 ?
                EntityState.Added :
                EntityState.Modified;
            await _userDataContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}