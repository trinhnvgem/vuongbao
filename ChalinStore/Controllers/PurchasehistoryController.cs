using System.Linq;
using System.Web.ModelBinding;
using System.Web.Mvc;
using ChalinStore.Models;

namespace ChalinStore.Controllers
{
    public class PurchasehistoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET /Purchasehistory/History/abc
        public ActionResult History(string username)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == username);
            var items = db.Orders.Where(x => x.Email == user.UserName).ToList();
            return View(items);
        }
    }
}