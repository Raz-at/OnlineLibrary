using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class UserController : Controller
    {
        private readonly LibraryDbContext userDb;
        private readonly IHttpContextAccessor contxt;

        public UserController(LibraryDbContext userDb, IHttpContextAccessor contxt) 
        {
            this.userDb = userDb;
            this.contxt = contxt;
        }
        public IActionResult Index()
        {
            if(contxt.HttpContext.Session.GetString("Active")=="Active")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            var checkEmail = await userDb.Users.FirstOrDefaultAsync(x=>x.Email == user.Email);
            if (checkEmail == null)
            {
                var addUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    Phone = user.Phone,
                    roll = user.roll,
                    status = "Active"                    
                };
                
                await userDb.Users.AddAsync(addUser);
                await userDb.SaveChangesAsync();
                ViewBag.EmailExists = false;
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.EmailExists = true;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var findEmail = await userDb.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if(findEmail == null)
            {
                ViewBag.findEmail = true;
                return View();
            }
            else
            {
                if(findEmail.Password == user.Password)
                {
                    contxt.HttpContext.Session.SetString("Name",findEmail.Name.ToString());
                    contxt.HttpContext.Session.SetString("Id", findEmail.Id.ToString());
                    contxt.HttpContext.Session.SetString("roll", findEmail.roll.ToString());
                    contxt.HttpContext.Session.SetString("role", "user");
                    contxt.HttpContext.Session.SetString("Active", findEmail.status.ToString());
                    
                    return RedirectToAction("TakenBooks","UserBook");
                }
                else
                {
                    ViewBag.findEmail = true;
                    return View();

                }
            }
        }

        public IActionResult Logout()
        {
            contxt.HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            if (contxt.HttpContext.Session.GetString("role") == "user")
            {
                var userIdString = contxt.HttpContext.Session.GetString("Id");
                var findEmail = await userDb.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userIdString));
                if (findEmail == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var ViewProfile = new User()
                    {
                        Name = findEmail.Name,
                        Email = findEmail.Email,
                        Password = findEmail.Password,
                        DateOfBirth = findEmail.DateOfBirth,
                        Address = findEmail.Address,
                        Phone = findEmail.Phone,
                        Gender = findEmail.Gender,
                        roll = findEmail.roll,
                        status = findEmail.status
                    };
                    return View(ViewProfile);
                }
            }
            else 
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            var findUser =  await userDb.Users.FindAsync(Guid.Parse(contxt.HttpContext.Session.GetString("Id")));

            if(findUser != null)
            {
                findUser.Name = user.Name;
                findUser.Password = user.Password;
                findUser.DateOfBirth = user.DateOfBirth;
                findUser.Address = user.Address;
                findUser.Phone = user.Phone;
                contxt.HttpContext.Session.SetString("Name", findUser.Name.ToString());
                await userDb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");


        }


        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var findUser = await userDb.Users.FindAsync(Guid.Parse(contxt.HttpContext.Session.GetString("Id")));
            if(findUser != null)
            {
                userDb.Users.Remove(findUser);
                await userDb.SaveChangesAsync();
                return RedirectToAction("SignUp");
            }
            return RedirectToAction("SignUp");

        }

    }
}
