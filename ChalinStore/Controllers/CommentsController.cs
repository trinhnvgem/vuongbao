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
            if (ModelState.IsValid)
            {
                model.CommentDate = DateTime.Now;
                db.Comments.Add(model);
                db.SaveChanges();
                return RedirectToAction("Detail", "Product", new { id = model.ProductId });
            }
            return RedirectToAction("Detail", "Product", new { id = model.ProductId });
        }

    }
}