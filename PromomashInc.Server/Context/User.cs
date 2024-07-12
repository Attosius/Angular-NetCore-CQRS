namespace PromomashInc.Server.Context;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
public class Country
{
    public string Code { get; set; }

    public string DisplayText { get; set; }
}
public class Province
{
    public string Code { get; set; }
    public string ParentCode { get; set; }

    public string DisplayText { get; set; }
}