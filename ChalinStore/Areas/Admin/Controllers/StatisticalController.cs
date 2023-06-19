using ChalinStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChalinStore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details() 
        {
            return View();
        }
       

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        where o.TypePayment == 2
                        select new
                        {
                            
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            TypePayment= o.TypePayment,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice,
                            Quantityp=p.Quantity
                        };
            
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.CreatedDate <= endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {

                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            })
                .Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetStatistical_Product(string fromDate, string toDate)
        {
            var query = from o in db.Orders

                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        where o.TypePayment == 2
                        select new
                        {

                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            TypePayment = o.TypePayment,
                            Title = p.Title,
                            ido = od.ProductId,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice,
                            Quantityp = p.Quantity
                        };
            var odq = query.GroupBy(x => x.ido).Select(x => new {Quantityo=x.Sum(y=>y.Quantity) });
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.CreatedDate <= endDate);
            }

            var jquery = query.GroupBy(x => x.ido).Select(x => new
            {
                
                Quantity = x.Sum(y => y.Quantity),
                Quantityp=x.Min(y => y.Quantityp),
                Title = x.Min(y => y.Title),
                ido = x.Min(y => y.ido),
                Price = x.Min(y => y.Price),
                OriginalPrice = x.Min(y => y.OriginalPrice),
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),

            }).Select(x => new
            {
                Quantity = x.Quantity,
                Quantityp=x.Quantityp,
                Quantityb = x.Quantity + x.Quantityp,
                ido= x.ido,
                Title = x.Title,
                Price = x.Price,
                OriginalPrice = x.OriginalPrice,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });  
            return Json(new { Data = jquery }, JsonRequestBehavior.AllowGet);
        }

    }
}