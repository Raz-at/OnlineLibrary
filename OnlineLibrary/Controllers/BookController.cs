using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryDbContext bookDb;
        private readonly IHttpContextAccessor contxt;

        public BookController(LibraryDbContext bookDb, IHttpContextAccessor contxt)
        {
            this.bookDb = bookDb;
            this.contxt =   contxt;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult addBook()
        {
            if (contxt.HttpContext.Session.GetString("role") == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> addBook(Book book)
        {
            if (contxt.HttpContext.Session.GetString("role") == "Admin")
            {
                var findBook = await bookDb.Books.FirstOrDefaultAsync(x => x.Title == book.Title && x.Author == book.Author);
                if (findBook == null)
                {
                    var newBook = new Book
                    {
                        BookId = Guid.NewGuid(),
                        Title = book.Title,
                        Author = book.Author,
                        Description = book.Description,
                        PublishDate = book.PublishDate,
                        ActualStock = book.ActualStock,
                        RemainingStock = book.ActualStock,
                        Price = book.Price
                    };
                    await bookDb.Books.AddAsync(newBook);
                    await bookDb.SaveChangesAsync();
                    return RedirectToAction("showBook");
                }
                return RedirectToAction("showBook");
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> showBook(string search)
        {
            if (contxt.HttpContext.Session.GetString("role") == "user")
            {
                if (contxt.HttpContext.Session.GetString("role") == "user" || contxt.HttpContext.Session.GetString("role") == "Admin")
                {
                    var userId = contxt.HttpContext.Session.GetString("Id");
                    if (contxt.HttpContext.Session.GetString("role") == "user")
                    {
                        var CountUser = await bookDb.userBooks.CountAsync(x => x.UserId == Guid.Parse(userId));
                        ViewBag.count = CountUser;
                    }

                    //ViewData["message"] = search;
                    var allBook = await bookDb.Books.ToListAsync();
                    if (!String.IsNullOrEmpty(search))
                    {
                        allBook = allBook.Where(x => x.Title.Contains(search)).ToList();
                    }

                    return View(allBook);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        
    }
}
