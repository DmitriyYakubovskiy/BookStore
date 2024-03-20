namespace BookStore.DataAccess.Entities
{
    public class BookEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int PageCount { get; set; }
        public int GenreId { get; set; }
        public int Year { get; set; }
        public float CostPrice { get; set; }
        public float SellingPrice { get; set; }
        public int? ContinuationBookId { get; set; }
        public int NumberOfSales { get; set; }
        public virtual AuthorEntity Author { get; set; } = null!;
        public virtual PublisherEntity Publisher { get; set; } = null!;
        public virtual GenreEntity Genre { get; set; } = null!;
        public virtual BookEntity ContinuationBook { get; set; } = null!;
    }
}
