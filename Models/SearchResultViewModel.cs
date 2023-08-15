using alwatnia.Helper;

namespace alwatnia.Models
{
    public class SearchResultViewModel
    {
        public string Query { get; set; }
        public SearchTypes Type { get; set; }
        public Functions.PaginatedResult<Product> Products { get; set; }
        public Functions.PaginatedResult<News> News { get; set; }
        public Functions.PaginatedResult<Activity> Activities { get; set; }
    }
}