﻿using ChalinStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Configuration;
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
        // danh sách sp và lọc theo time
        public ActionResult Details() 
        {
            return View();
        }

        // danh sách sp theo trạng thái
        public ActionResult Details_1()
        {
            return View();
        }


        // thống kê biểu đồ
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

        // thống kê sản phẩm và tìm kiếm theo thời gian
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
            var odq = query.GroupBy(x => x.ido).Select(x => new {
                Quantityo=x.Sum(y=>y.Quantity),
                Quantityt = x.Min(y => y.Quantityp)
            }).Select(x => new
            {
                Quantityn=x.Quantityt+x.Quantityo,
            });
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
            return Json(new { Data = jquery,Data1=odq },JsonRequestBehavior.AllowGet);
            
        }


        // thống kê sản phẩm đã bán
        [HttpGet]
        public ActionResult GetStatistical_Productb(string fromDate, string toDate)
        {
            var query = from o in db.Orders

                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id  
                        where o.TypePayment==2
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
            var odq = query.GroupBy(x => x.ido).Select(x => new {
                Title = x.Min(y => y.Title),                
                Quantity = x.Sum(y => y.Quantity),
                Quantityt = x.Min(y => y.Quantityp),
                Price = x.Min(y => y.Price),
                OriginalPrice = x.Min(y => y.OriginalPrice),

            }) 
                .Select(x => new
                {

                    Quantity = x.Quantity,
                    Quantityt = x.Quantityt,
                    Quantityn = x.Quantity + x.Quantityt,
                    Title = x.Title,
                    Price = x.Price,
                    OriginalPrice = x.OriginalPrice,
                });
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
            return Json(new { Data = odq }, JsonRequestBehavior.AllowGet);
        }

        // thống kê sp theo trạng thái chờ thanh toán
        [HttpGet]
        public ActionResult GetStatistical_Productc(string fromDate, string toDate)
        {
            var query = from o in db.Orders

                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        where o.TypePayment == 1
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
            var odq = query.GroupBy(x => x.ido).Select(x => new {
                Title = x.Min(y => y.Title),
                Quantity = x.Sum(y => y.Quantity),
                Quantityt = x.Min(y => y.Quantityp),
                Price = x.Min(y => y.Price),
                OriginalPrice = x.Min(y => y.OriginalPrice),

            })
                .Select(x => new
                {

                    Quantity = x.Quantity,
                    Quantityt = x.Quantityt,
                    Quantityn = x.Quantity + x.Quantityt,
                    Title = x.Title,
                    Price = x.Price,
                    OriginalPrice = x.OriginalPrice,
                });
            return Json(new { Data = odq }, JsonRequestBehavior.AllowGet);
        }

        // thống kê sản phẩm theo trạng thái Huỷ thanh toán
        [HttpGet]
        public ActionResult GetStatistical_Producth(string fromDate, string toDate)
        {
            var query = from o in db.Orders

                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        where o.TypePayment == 3
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
            var odq = query.GroupBy(x => x.ido).Select(x => new {
                Title = x.Min(y => y.Title),
                Quantity = x.Sum(y => y.Quantity),
                Quantityt = x.Min(y => y.Quantityp),
                Price = x.Min(y => y.Price),
                OriginalPrice = x.Min(y => y.OriginalPrice),

            })
                .Select(x => new
                {

                    Quantity = x.Quantity,
                    Quantityt = x.Quantityt,
                    Quantityn = x.Quantity + x.Quantityt,
                    Title = x.Title,
                    Price = x.Price,
                    OriginalPrice = x.OriginalPrice,
                });
            return Json(new { Data = odq }, JsonRequestBehavior.AllowGet);
        }

    }
}