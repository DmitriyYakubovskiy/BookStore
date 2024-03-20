using System.Collections.Generic;

namespace BookStore.DataAccess.Entities
{
    public class AuthorEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
    }
}
