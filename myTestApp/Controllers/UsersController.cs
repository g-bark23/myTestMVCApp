using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myTestApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using static myTestApp.Session.SessionHelper;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace myTestApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyTestAppContext _context;

        public UsersController(MyTestAppContext context)
        {
            _context = context;
        }

        public IActionResult getUser()
        {
            var usersName = from User in _context.User
                            select User;

            var myUser = new User();
            myUser.name = usersName.ToString();

            return View(myUser);
        }

        // POST: Create User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name, username, userID, isAdmin, password")] User user)
        {
            if (ModelState.IsValid)
            {
                if (user.name == null || user.username == null || user.password == null){
                    return RedirectToAction("myTestView", "Home");
                }

                byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
                //using (var rng = RandomNumberGenerator.Create())
                //{
                //    rng.GetBytes(salt);
                //}
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                _context.Add(new User
                {
                    name = user.name,
                    username = user.username,
                    password = hashed
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("myTestView", "Home");
            }
            return RedirectToAction("myTestView", "Home");
        }

        // POST: Log in user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInUser([Bind("name, username, userID, isAdmin, password")] User user)
        {
            if (ModelState.IsValid)
            {
                if(user.username == null || user.password == null){
                    return RedirectToAction("myTestView", "Home"); 
                }
                var logInUserPassword = from myUser in _context.User
                                                               where myUser.username == user.username
                                                           select myUser.password;

                var logInUserID = from myUser in _context.User
                                        where myUser.username == user.username
                                        select myUser.userID;
                String uID = logInUserID.First().ToString();

                byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                
                if(logInUserPassword.First() == hashed){
                    HttpContext.Session.SetString(USERKEY, uID);
                    HttpContext.Session.SetString(SELECTEDUSER, uID);
                    return RedirectToAction("dashboard", "Home");
                }else {
                    return RedirectToAction("myTestView", "Home");
                }
            }
            return RedirectToAction("myTestView", "Home");
        }

        // POST: input time punch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Time(DateTime newStartTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new TimeCard
              {
                    userID = 4,
                    startTime = newStartTime.ToString()
              });
                await _context.SaveChangesAsync();  
            }
            return RedirectToAction("dashboard", "Home");
        }

        // POST: input time punch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StopTime(DateTime newStopTime)
        {
            if (ModelState.IsValid)
            {
                var theTimeCard = from myTimeCard in _context.TimeCard
                                 where myTimeCard.stopTime == null
                                 where myTimeCard.userID == 4
                                 select myTimeCard;
                TimeCard m = theTimeCard.First();
                m.stopTime = newStopTime.ToString();
                _context.Update(m);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("dashboard", "Home");
        }
    }
}
