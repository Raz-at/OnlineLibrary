using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using System.Net;

namespace OnlineLibrary.Controllers
{
    public class AdminController : Controller
    {
        private readonly LibraryDbContext admin;
        private readonly IHttpContextAccessor contxt;

        public AdminController(LibraryDbContext admin, IHttpContextAccessor contxt)
        {
            this.admin = admin;
            this.contxt = contxt;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet] 
        public IActionResult AdminLogin()
        { 
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admin user)
        {
            if(ModelState.IsValid)
            {
                var findEmail = await admin.Admins.FirstOrDefaultAsync(x => x.Email == user.Email);
                if (findEmail != null)
                {
                    if(findEmail.Password == user.Password)
                    {
                        contxt.HttpContext.Session.SetString("role", "Admin");
                        contxt.HttpContext.Session.SetString("Active", "Active");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password Incorrect");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email Incorrect");
                    return BadRequest(ModelState);
                }

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> showUser()
        {
            var getAll = await admin.Users.ToListAsync();
            return View(getAll);
        }

        [HttpGet]
        public async Task<IActionResult> searchBook(Guid id)
        {
            contxt.HttpContext.Session.SetString("IssueUser", id.ToString());
            var searchUser = await admin.Users.FirstOrDefaultAsync(x => x.Id == id);
            contxt.HttpContext.Session.SetString("IssueUserRole", searchUser.roll);

            var countUser = await admin.userBooks.CountAsync(x => x.UserId == id);
            
            TempData["UserCount"] = countUser;



            return RedirectToAction("bookIssue");
        }

        
        [HttpGet]
        public async Task<IActionResult> bookIssue()
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var userCount = (int)TempData["UserCount"];
                ViewBag.countUser = userCount;
                var allBook = await admin.Books.ToListAsync();
                return View(allBook);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }



        public async Task<IActionResult> Delete(Guid id)
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var findUser = await admin.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (findUser != null)
                {
                    admin.Users.Remove(findUser);
                    await admin.SaveChangesAsync();
                    return RedirectToAction("showUser");
                }
                return RedirectToAction("showUser");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> DeleteBook(Guid id)
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var findBook = await admin.Books.FirstOrDefaultAsync(x => x.BookId == id);
                var findBookAndUser = await admin.userBooks.FirstOrDefaultAsync(x => x.BookId == id);
                if (findBook != null)
                {
                    admin.Books.Remove(findBook);

                    if (findBookAndUser != null)
                    {
                        admin.userBooks.Remove(findBookAndUser);
                    }
                    else
                    { }
                    await admin.SaveChangesAsync();
                    return RedirectToAction("showBook", "Book");


                }
                return RedirectToAction("showBook", "Book");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ShowUserBook()
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var userBooks = await admin.userBooks.ToListAsync();
                return View(userBooks);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Change(Guid id)
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var findUser = await admin.Users.FirstOrDefaultAsync(a => a.Id == id);
                if (findUser != null)
                {
                    if (findUser.status == "Active")
                    {

                        findUser.status = "Deactive";
                        admin.Users.Update(findUser);
                    }
                    else
                    {

                        findUser.status = "Active";
                        admin.Users.Update(findUser);
                    }
                    await admin.SaveChangesAsync();
                    return RedirectToAction("showUser");
                }
                else
                {
                    return RedirectToAction("showUser");
                }
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        public async Task<IActionResult> ManageBook()
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var allBook = await admin.Books.ToListAsync();
                return View(allBook);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        
       [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var findBook = await admin.Books.FirstOrDefaultAsync(x => x.BookId == id);
                if (findBook != null)
                {
                    var bookProfile = new Book()
                    {
                        BookId = findBook.BookId,
                        Title = findBook.Title,
                        Description = findBook.Description,
                        Author = findBook.Author,
                        PublishDate = findBook.PublishDate,
                        Price = findBook.Price,
                        ActualStock = findBook.ActualStock,
                        RemainingStock = findBook.RemainingStock
                    };
                    return View(bookProfile);
                }
                return RedirectToAction("ManageBook");
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var findBook = await admin.Books.FirstOrDefaultAsync(x => x.BookId == book.BookId);
                if (findBook != null)
                {
                    var books = findBook.ActualStock;
                    findBook.Title = book.Title;
                    findBook.Description = book.Description;
                    findBook.Author = book.Author;
                    findBook.PublishDate = book.PublishDate;
                    findBook.ActualStock = book.ActualStock;
                    findBook.RemainingStock = book.ActualStock - (books - findBook.RemainingStock);
                    findBook.Price = book.Price;

                    await admin.SaveChangesAsync();
                    return RedirectToAction("ManageBook");
                }
                return RedirectToAction("ManageBook");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> userbook(Guid id)
        {
            if (contxt.HttpContext.Session.GetString("role") == "admin")
            {
                var userId = contxt.HttpContext.Session.GetString("IssueUser");
                var findUser = await admin.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId));
                var findBook = await admin.Books.FirstOrDefaultAsync(x => x.BookId == id);
                var findUserBook = await admin.userBooks.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId) && x.BookId == id);
                var countUser = await admin.userBooks.CountAsync(x => x.Id == Guid.Parse(userId));


                if (findUserBook != null)
                {
                    return RedirectToAction("bookIssue");
                    //return NotFound();
                }
                else
                {
                    if (findBook != null && findUser != null)
                    {
                        var bookUser = new UserBook()
                        {
                            Id = Guid.NewGuid(),
                            UserId = findUser.Id,
                            BookId = findBook.BookId,
                            Title = findBook.Title,
                            Username = findUser.Name,
                            IssueDate = DateTime.Today
                        };
                        await admin.userBooks.AddAsync(bookUser);
                        var findBookInBook = await admin.Books.FirstOrDefaultAsync(x => x.BookId == id);
                        if (findBookInBook != null)
                        {
                            findBookInBook.RemainingStock -= 1;
                            admin.Books.Update(findBookInBook);
                        }
                        await admin.SaveChangesAsync();
                        return RedirectToAction("ShowUserBook");
                    }
                }
                return RedirectToAction("bookIssue");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
