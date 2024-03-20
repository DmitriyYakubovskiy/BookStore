namespace BookStore.DataAccess.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
    }
}
