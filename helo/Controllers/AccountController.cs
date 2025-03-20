using helo.Data;
using helo.Models.CustomLogin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace helo.Controllers
{
    public class AccountController : Controller
    {
        private readonly AddDbcontext _context;
        public AccountController(AddDbcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.UserAccounts.ToList());
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserAccount user = new UserAccount();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Password = model.Password;

                try
                {
                    _context.UserAccounts.Add(user);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{user.FirstName} {user.LastName} Registered Successfully. Please Login.";
                }
                catch (Exception ex )
                {
                    ModelState.AddModelError("", "Please enter unique Email or Password");
                }

                return View();
               
            }
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
              var user = _context.UserAccounts.Where(u => (u.UserName == model.UserNameOrEmail || u.Email == model.UserNameOrEmail) && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.Name,user.Email),
                      new Claim("Name",user.FirstName),
                      new Claim(ClaimTypes.Role,"User")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                   return RedirectToAction("Index");
                    
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is incorrect");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();



            
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Login");
        }

        [Authorize]
         public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

    }
}
