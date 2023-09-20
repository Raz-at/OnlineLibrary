using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class UserBookController : Controller
    {
        private readonly LibraryDbContext userBook;
        private readonly IHttpContextAccessor contxt;

        public UserBookController(LibraryDbContext userBook,IHttpContextAccessor contxt)
        {
            this.userBook = userBook;
            this.contxt = contxt;
        }

        public async Task<IActionResult> TakenBooks()
        {
            if (contxt.HttpContext.Session.GetString("role")=="user")
            {
                var userId = contxt.HttpContext.Session.GetString("Id");
                var books = await userBook.userBooks.Where(x => x.UserId == Guid.Parse(userId)).ToListAsync();
                return View(books);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var userId = contxt.HttpContext.Session.GetString("Id");
            var FindUser = await userBook.Users.FindAsync(Guid.Parse(userId));

            //var userCount = await userBook.Users.CountAsync(x => x.Id ==Guid.Parse(userId));

            //ViewBag.count = userCount;

            var FindBook = await userBook.Books.FirstOrDefaultAsync(x => x.BookId == id);
            if(FindUser != null && FindBook != null)

            {
                var TakeBook = await userBook.userBooks.FirstOrDefaultAsync(x => x.UserId == FindUser.Id && x.BookId == FindBook.BookId);
                if (TakeBook == null)
                {
                    ViewBag.takeBook = false;
                }
                else
                {
                    ViewBag.takeBook = true;
                }

                var bookUser = new UserBook()
                {
                    Id = Guid.NewGuid(),
                    UserId = FindUser.Id,
                    BookId = FindBook.BookId,
                    Title = FindBook.Title,
                    Username = FindUser.Name,
                    IssueDate = DateTime.Today                    
                };

                ViewBag.active = FindUser.status;
                return View(bookUser);
            }
            return RedirectToAction("Index","Home");

        }

        [HttpPost]
        public async Task<IActionResult> Issuse(UserBook book1)
        {
            var checkBook = await userBook.userBooks.FirstOrDefaultAsync(x => x.UserId == book1.UserId && x.BookId == book1.BookId);
            var bookId = book1.BookId;
            if(checkBook != null)
            {
                ViewBag.message = true;
                return RedirectToAction("TakenBooks");
            }
            else
            {
                var addBook = new UserBook
                {
                    Id = book1.Id,
                    UserId = book1.UserId,
                    BookId = book1.BookId,
                    Title = book1.Title,
                    Username = book1.Username,
                    IssueDate = book1.IssueDate
                };
                ViewBag.message = false;
                await userBook.userBooks.AddAsync(addBook);
                var findBook = await userBook.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
                if (findBook != null)
                {
                    findBook.RemainingStock -= 1;
                    userBook.Books.Update(findBook);
                }
                await userBook.SaveChangesAsync();
                return RedirectToAction("TakenBooks");
            }           

        }

        public async Task<IActionResult> Return(Guid id)
        {
            var findUserBook = await userBook.userBooks.FirstOrDefaultAsync(x => x.Id == id);
            userBook.userBooks.Remove(findUserBook);

            var findBook = await userBook.Books.FirstOrDefaultAsync(x => x.BookId == findUserBook.BookId);
            if(findBook != null) 
            {
                findBook.RemainingStock += 1;
                userBook.Books.Update(findBook);
            }
            await userBook.SaveChangesAsync();
            return RedirectToAction("TakenBooks");

        }


    }
}
