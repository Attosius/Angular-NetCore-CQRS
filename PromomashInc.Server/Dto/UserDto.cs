namespace PromomashInc.Server.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string CountryCode { get; set; }
    public string ProvinceCode { get; set; }
}