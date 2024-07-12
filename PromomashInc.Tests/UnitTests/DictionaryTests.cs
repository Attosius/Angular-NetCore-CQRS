using Microsoft.Extensions.Logging;
using Moq;
using PromomashInc.Core.Models;
using PromomashInc.EntitiesDto;
using PromomashInc.Server.Controllers;

namespace PromomashInc.Tests.UnitTests
{
    public class DictionaryTests
    {
        private Mock<ILogger<DictionaryController>> _loggerMock;
        private Mock<IDictionaryRepository> _dictionaryRepositoryMock;
        private List<CountryDto> _countryDtos;
        private List<ProvinceDto> _provinceDto;


        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<DictionaryController>>();
            _dictionaryRepositoryMock = new Mock<IDictionaryRepository>();
            _countryDtos =
            [
                new CountryDto
                {
                    DisplayText = "C1",
                    Code = "C1"
                },

                new CountryDto
                {
                    DisplayText = "C2",
                    Code = "C2"
                }

            ];
            _provinceDto =
            [
                new ProvinceDto
                {
                    DisplayText = "P1",
                    Code = "P1",
                    ParentCode = "C1"
                },

                new ProvinceDto
                {
                    DisplayText = "P_11",
                    Code = "P_11",
                    ParentCode = "C1"
                },

                new ProvinceDto
                {
                    DisplayText = "P2",
                    Code = "P2",
                    ParentCode = "C2"
                }

            ];

            _dictionaryRepositoryMock
                .Setup(a => a.GetCountries())
                .Returns(() => Task.FromResult(_countryDtos));
            _dictionaryRepositoryMock
                .Setup(a => a.GetProvince(It.IsAny<string>()))
                .Returns((string parentCode) =>
            {
                var provinceDtos = _provinceDto.Where(o => o.ParentCode == parentCode).ToList();
                return Task.FromResult(provinceDtos);
            });
        }

        [Test]
        public async Task TestDictionaryController_GetCountries()
        {

            var dictionaryController = new DictionaryController(_loggerMock.Object, _dictionaryRepositoryMock.Object);

            var result = await dictionaryController.GetCountries();


            Assert.IsTrue(result.IsSuccess);
            CollectionAssert.AreEqual(_countryDtos, result.Value);
        }

        [Test]
        public async Task TestDictionaryController_GetProvince()
        {

            var dictionaryController = new DictionaryController(_loggerMock.Object, _dictionaryRepositoryMock.Object);

            var result = await dictionaryController.GetProvince("C1");

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(2, result.Value.Count);
            Assert.AreEqual(_provinceDto[0], result.Value[0]);
            Assert.AreEqual(_provinceDto[1], result.Value[1]);
        }

    }
}