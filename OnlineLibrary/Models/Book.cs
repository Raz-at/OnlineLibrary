namespace OnlineLibrary.Models
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public int ActualStock { get; set; }
        public int RemainingStock { get; set; }
        public int Price { get; set; }

        public List<UserBook> UserBooks { get; set; }
    }
}
