namespace BookStore.DataAccess.Entities
{
    public class DiscountEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Title { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public string PublisherName { get; set; } = "";
        public string Genre { get; set; } = "";
        public int Percent { get;set; }
    }
}
