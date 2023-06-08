//using ChalinStore.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PagedList;
//using ChalinStore.Models.EF;

//namespace ChalinStore.Controllers
//{
//    public class CommentsController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();
//        // GET: Comments
//        public ActionResult Index(int? page)
//        {
//            var pageSize = 10;
//            if (page == null)
//            {
//                page = 1;
//            }
//            IEnumerable<Comments> items = db.Comments.OrderByDescending(x => x.Id);
//            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
//            items = items.ToPagedList(pageIndex, pageSize);
//            ViewBag.PageSize = pageSize;
//            ViewBag.Page = page;
//            return View(items);
//        }
//        public ActionResult Add()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Add(Comments model)
//        {

//            if (ModelState.IsValid)
//            {
//                model.CommentDate = DateTime.Now;
//                db.Comments.Add(model);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(model);
//        }
//        public ActionResult Detail(int id)
//        {
//            var item = db.Comments.Find(id);
//            return View(item);
//        }
//    }
//}