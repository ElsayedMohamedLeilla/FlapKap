namespace FlapKap.Models.Requests
{
    public class BaseCriteria
    {
        public int? Id { get; set; }
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string FreeText { get; set; }
    }
}
