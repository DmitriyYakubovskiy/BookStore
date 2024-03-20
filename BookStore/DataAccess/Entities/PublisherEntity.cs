using System.Collections.Generic;

namespace BookStore.DataAccess.Entities
{
    public class PublisherEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
    }
}
