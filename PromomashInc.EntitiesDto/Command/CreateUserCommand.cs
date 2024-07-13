using MediatR;

namespace PromomashInc.EntitiesDto.Command
{
    public class CreateUserCommand : IRequest<int>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
    }
}
