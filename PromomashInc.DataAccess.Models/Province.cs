namespace PromomashInc.DataAccess.Models
{
    public class Province
    {
        public string Code { get; set; }
        public string ParentCode { get; set; }

        public string DisplayText { get; set; }
        public virtual Country Country { get; set; }
    }
}