namespace PromomashInc.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }

        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
    }
}