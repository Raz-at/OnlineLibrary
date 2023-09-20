namespace OnlineLibrary.Models
{
    public class UserBook
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

        public string Title { get; set; }   
        public string Username { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }

        public DateTime IssueDate { get; set; }  
    }
}
