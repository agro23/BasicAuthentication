using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BasicAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BasicAuthentication.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly BasicAuthenticationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemController(UserManager<ApplicationUser> userManager, BasicAuthenticationContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(_db.Items.Where(x => x.User.Id == currentUser.Id));
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            item.User = currentUser;
            _db.Items.Add(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}