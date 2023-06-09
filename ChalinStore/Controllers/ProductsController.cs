using ChalinStore.Models;
using ChalinStore.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChalinStore.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(string Searchtext, int? page, int? sizePage)
            // phân chia trang product vs số lượng là 10
        {
            var pageSize = sizePage ?? 8;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.CreatedDate);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Detail(string alias, int id)
        // hiển thi chi tiết sp
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                
            }
            
            var comments = db.Comments.Where(x => x.ProductId == id).ToList();
            //get name user
            foreach (var comment in comments)
            {
                var user = db.Users.Find(comment.UserId);
                if (user != null)
                {
                    comment.UserId = user.UserName;
                }
            }
            ViewBag.Comments = comments;
           
            return View(item);
        }
        
        
        public ActionResult ProductCategory(string alias, int id, int? page)
        // hiển thi danh sách sản phẩm
        {
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);

        }

        public ActionResult Partial_ItemsByCateId()
            // hiển thi số lương sp trong phần menu-arrivals là 20
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(20).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        // hiển thi số lương sp trong phần sale là 20
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(20).ToList();
            return PartialView(items);
        }
    }
}