using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Data;
using NewsPaper.Models;

namespace NewsPaper.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext Context;
        private readonly UserManager<ApplicationUser> UserManager;

        public AdminController(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager)
        {
            this.Context = Context;
            this.UserManager = UserManager;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {

            return View("AdminPanel",Context.Users.Select(user=>user).ToList());
        }
        [HttpPost]
        public async Task<string> ResetRoleUser(string selectRole, string userId)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            string userEventRole = (await UserManager.GetRolesAsync(user)).FirstOrDefault();
            if (!string.IsNullOrEmpty(userEventRole)) {
                await UserManager.RemoveFromRoleAsync(user,userEventRole);
                await UserManager.AddToRoleAsync(user,selectRole);
            }
            else
            {
                await UserManager.AddToRoleAsync(user, selectRole);
            }

            return "Все пиздато";
        }
        [HttpPost]
        public bool BlockUser(string userId)
        {
            try
            {
                var result = Context.Users
                    .Where(user => user.Id == userId)
                    .FirstOrDefault();
                if(result!=null)
                {
                    result.IsBlocked = !result.IsBlocked;
                }
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}