using Microsoft.Extensions.Logging;
using Moq;
using PromomashInc.Core;
using PromomashInc.DataAccess.Context;
using PromomashInc.EntitiesDto;
using PromomashInc.Mapper;

namespace PromomashInc.Tests.IntegrationTests
{
    public class UserRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            DbInitializer.Initialize();
        }

        [Test]
        public async Task TestUserRepository_Save()
        {
            // arrange
            await using var context = new UserDataContext();
            var mapper = AutoMapperConfig.Configure().CreateMapper();
            var loggerMock = new Mock<ILogger<UserRepository>>();

            var userRepository = new UserRepository(loggerMock.Object, context, mapper, new CustomPasswordHasher());
            var userData = new UserDto
            {
                Email = "1@1.com",
                Password = "1Q",
                CountryCode = "Country_1",
                ProvinceCode = "Province_4",
            };

            // act
            var result = await userRepository.Save(userData);

            // assert
            var user = context.Users.FirstOrDefault(o => o.Email == "1@1.com");

            Assert.IsTrue(result.IsSuccess);
            Assert.NotNull(user);
            Assert.That("1@1.com" == user.Email);
            Assert.That("Country_1" == user.CountryCode);
            Assert.That("Province_4" == user.ProvinceCode);
        }
    }
}