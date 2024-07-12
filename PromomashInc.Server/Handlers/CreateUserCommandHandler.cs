using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Fluent;
using PromomashInc.Core;
using PromomashInc.Core.Models;
using PromomashInc.DataAccess.Context;
using PromomashInc.DataAccess.Models;
using PromomashInc.EntitiesDto.Command;
using PromomashInc.Server.Controllers;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace PromomashInc.Server.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly UserDataContext _userDataContext;
        private readonly IMapper _mapper;
        private readonly ICustomPasswordHasher _customPasswordHasher;

        public CreateUserCommandHandler(ILogger<UserRepository> logger,
            UserDataContext userDataContext,
            IMapper mapper,
            ICustomPasswordHasher customPasswordHasher)
        {
            _logger = logger;
            _userDataContext = userDataContext;
            _mapper = mapper;
            _customPasswordHasher = customPasswordHasher;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    throw new Exception("User data undefined");
                }

                var user = _mapper.Map<User>(request);
                var isNew = user.Id == 0;

                var hashed = _customPasswordHasher.GetHash(request.Password);
                user.PasswordHash = hashed;
                _userDataContext.Entry(user).State = isNew ?
                    EntityState.Added :
                    EntityState.Modified;
                await _userDataContext.SaveChangesAsync();

                return user.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Results error");
                throw;
            }
        }
    }

    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"Before execution for {typeof(TRequest).Name}");

                return await next();
            }
            finally
            {
                _logger.Log(LogLevel.Information,$"After execution for {typeof(TRequest).Name}");
            }
        }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty");
           
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Please specify a valid email address");
            
        }
    }

}