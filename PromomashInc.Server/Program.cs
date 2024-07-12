using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using PromomashInc.Core;
using PromomashInc.DataAccess.Context;
using PromomashInc.Core.Models;
using PromomashInc.Mapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using PromomashInc.Server.Handlers;
using FluentValidation;
using System;
using PromomashInc.EntitiesDto.Command;

namespace PromomashInc.Server
{
    public class Program
    {
        public static void SetOptions(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder
                .UseSqlite("Data Source=clients.db;Cache=Shared")
                .EnableSensitiveDataLogging();
            //.UseSqlServer(@"Server=.;Initial Catalog=ClientsDb;Persist Security Info=False;Integrated Security=True;MultipleActiveResultSets=False;Connection Timeout=180;");
        }
        private static void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<UserDataContext>(SetOptions);
        }

        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Info("Init main");
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                ConfigureServices(builder.Services);
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                var app = builder.Build();
                DbInitializer.Initialize();

                app.UseDefaultFiles();
                app.UseStaticFiles();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();

                app.MapFallbackToFile("/index.html");
                app.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            ConfigureDbContext(services);
            services.AddSingleton(AutoMapperConfig.Configure().CreateMapper());
          
            services.AddScoped<ICustomPasswordHasher, CustomPasswordHasher>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDictionaryRepository, DictionaryRepository>();


            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
    }
}
