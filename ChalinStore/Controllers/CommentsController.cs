using ChalinStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ChalinStore.Models.EF;

namespace ChalinStore.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        // tạo controller để lưu comment
        public ActionResult Create(Comment model)
        {   
                model.CommentDate = DateTime.Now;
                db.Comments.Add(model);
                db.SaveChanges();
                var product = db.Products.Find(model.ProductId);
                return RedirectToAction("Detail", "Products", new { alias = product.Alias, id = model.ProductId });
                
        }

    }
}