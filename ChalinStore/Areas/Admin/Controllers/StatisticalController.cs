using ChalinStore.Models;
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
        /*public ActionResult Details_2()
        {
            return View();
        }*/

        // thống kê biểu đồ
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from order in db.Orders
                        join orderDetail in db.OrderDetails
                        on order.Id equals orderDetail.OrderId
                        join product in db.Products
                        on orderDetail.ProductId equals product.Id
                        where order.TypePayment == 2
                        select new
                        {
                            order_CreatedDate = order.CreatedDate,
                            orderDetail_Quantity = orderDetail.Quantity,
                            product_Price = product.Price,
                            product_OriginalPrice = product.OriginalPrice,
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.order_CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.order_CreatedDate <= endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.order_CreatedDate)).Select(x => new
            {

                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.orderDetail_Quantity * y.product_OriginalPrice),
                TotalSell = x.Sum(y => y.orderDetail_Quantity * y.product_Price),
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
            
            
            
            var query = from order in db.Orders
                        join orderDetail in db.OrderDetails
                        on order.Id equals orderDetail.OrderId
                        join product in db.Products
                        on orderDetail.ProductId equals product.Id
                        where order.TypePayment == 2
                        select new
                        {
                            order_CreatedDate = order.CreatedDate,
                            order_TypePayment = order.TypePayment,
                            orderDetail_Quantity = orderDetail.Quantity,
                            orderDetail_ProductId = orderDetail.ProductId,
                            product_Price = product.Price,
                            product_Title = product.Title,
                            product_OriginalPrice = product.OriginalPrice,
                            product_Quantity = product.Quantity
                        };
            
            
            
            
            //tính số lượng sản phẩm nhập
            var odq = query.GroupBy(x => x.orderDetail_ProductId).Select(x => new {
                orderDetail_Quantity=x.Sum(y=>y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                orderDetail_ProductId = x.Min(y => y.orderDetail_ProductId),
            }).Select(x => new
            {
                orderDetail_ProductId=x.orderDetail_ProductId,
                Input_Quantity=x.orderDetail_Quantity+x.product_Quantity,
            });
            
            
            
            
            
            
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.order_CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.order_CreatedDate <= endDate);
            }

            var jquery = query.GroupBy(x => x.orderDetail_ProductId).Select(x => new
            {

                orderDetail_Quantity = x.Sum(y => y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                product_Title = x.Min(y => y.product_Title),
                orderDetail_ProductId = x.Min(y => y.orderDetail_ProductId),
                product_Price = x.Min(y => y.product_Price),
                product_OriginalPrice = x.Min(y => y.product_OriginalPrice),
                TotalBuy = x.Sum(y => y.orderDetail_Quantity * y.product_OriginalPrice),
                TotalSell = x.Sum(y => y.orderDetail_Quantity * y.product_Price),

            }).Select(x => new
            {
                orderDetail_Quantity = x.orderDetail_Quantity,
                product_Quantity = x.product_Quantity,
                orderDetail_ProductId= x.orderDetail_ProductId,
                product_Title = x.product_Title,
                product_Price = x.product_Price,
                product_OriginalPrice = x.product_OriginalPrice,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new { Data = jquery,Data1=odq },JsonRequestBehavior.AllowGet);
            
        }


        // thống kê sản phẩm đã bán
        [HttpGet]
        public ActionResult GetStatistical_Productb(string fromDate, string toDate)
        {
            var query = from order in db.Orders
                        join orderDetail in db.OrderDetails
                        on order.Id equals orderDetail.OrderId
                        join product in db.Products
                        on orderDetail.ProductId equals product.Id
                        where order.TypePayment == 2
                        select new
                        {
                            order_CreatedDate = order.CreatedDate,
                            order_TypePayment = order.TypePayment,
                            orderDetail_Quantity = orderDetail.Quantity,
                            orderDetail_ProductId = orderDetail.ProductId,
                            product_Price = product.Price,
                            product_Title = product.Title,
                            product_OriginalPrice = product.OriginalPrice,
                            product_Quantity = product.Quantity
                        };
            var jquery = query.Where(x => x.order_TypePayment == 2).GroupBy(x => x.orderDetail_ProductId).Select(x => new {
                orderDetail_Quantity = x.Sum(y => y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                orderDetail_ProductId = x.Min(y => y.orderDetail_ProductId),
            }).Select(x => new
            {
                orderDetail_ProductId = x.orderDetail_ProductId,
                Input_Quantity = x.orderDetail_Quantity + x.product_Quantity,
            });
            var odq = query.Where(x => x.order_TypePayment == 2).GroupBy(x => x.orderDetail_ProductId).Select(x => new {
                product_Title = x.Min(y => y.product_Title),
                orderDetail_Quantity = x.Sum(y => y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                product_Price = x.Min(y => y.product_Price),
                product_OriginalPrice = x.Min(y => y.product_OriginalPrice),
            })
                .Select(x => new
                {
                    orderDetail_Quantity = x.orderDetail_Quantity,
                    product_Quantity = x.product_Quantity,
                    product_Title = x.product_Title,
                    product_Price = x.product_Price,
                    product_OriginalPrice = x.product_OriginalPrice,
                });
            return Json(new { Data = odq ,Data1=jquery}, JsonRequestBehavior.AllowGet);
        }

        // thống kê sp theo trạng thái chờ thanh toán
        [HttpGet]
        public ActionResult GetStatistical_Productc(string fromDate, string toDate)
        {

            var query = from order in db.Orders
                        join orderDetail in db.OrderDetails
                        on order.Id equals orderDetail.OrderId
                        join product in db.Products
                        on orderDetail.ProductId equals product.Id
                        select new
                        {
                            order_CreatedDate = order.CreatedDate,
                            order_TypePayment = order.TypePayment,
                            orderDetail_Quantity = orderDetail.Quantity,
                            orderDetail_ProductId = orderDetail.ProductId,
                            product_Price = product.Price,
                            product_Title = product.Title,
                            product_OriginalPrice = product.OriginalPrice,
                            product_Quantity = product.Quantity
                        };


            var jquery = query.Where(x => x.order_TypePayment == 2).GroupBy(x => x.orderDetail_ProductId).Select(x => new {
                orderDetail_Quantity = x.Sum(y => y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                orderDetail_ProductId = x.Min(y => y.orderDetail_ProductId),
            }).Select(x => new
            {
                orderDetail_ProductId = x.orderDetail_ProductId,
                Input_Quantity = x.orderDetail_Quantity + x.product_Quantity,
            });


            var odq = query.Where(x=>x.order_TypePayment==1).GroupBy(x => x.orderDetail_ProductId).Select(x => new {
                product_Title = x.Min(y => y.product_Title),
                orderDetail_Quantity = x.Sum(y => y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                product_Price = x.Min(y => y.product_Price),
                product_OriginalPrice = x.Min(y => y.product_OriginalPrice),

            })
                .Select(x => new
                {

                    orderDetail_Quantity = x.orderDetail_Quantity,
                    product_Quantity = x.product_Quantity,
                    product_Title = x.product_Title,
                    product_Price = x.product_Price,
                    product_OriginalPrice = x.product_OriginalPrice,
                });
            return Json(new { Data = odq ,Data1=jquery}, JsonRequestBehavior.AllowGet);
        }

        // thống kê sản phẩm theo trạng thái Huỷ thanh toán
        [HttpGet]
        public ActionResult GetStatistical_Producth(string fromDate, string toDate)
        {
            var query = from order in db.Orders
                        join orderDetail in db.OrderDetails
                        on order.Id equals orderDetail.OrderId
                        join product in db.Products
                        on orderDetail.ProductId equals product.Id
                        select new
                        {
                            order_CreatedDate = order.CreatedDate,
                            order_TypePayment = order.TypePayment,
                            orderDetail_Quantity = orderDetail.Quantity,
                            orderDetail_ProductId = orderDetail.ProductId,
                            product_Price = product.Price,
                            product_Title = product.Title,
                            product_OriginalPrice = product.OriginalPrice,
                            product_Quantity = product.Quantity
                        };
            var jquery = query.Where(x => x.order_TypePayment == 2).GroupBy(x => x.orderDetail_ProductId).Select(x => new {
                orderDetail_Quantity = x.Sum(y => y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                orderDetail_ProductId = x.Min(y => y.orderDetail_ProductId),
            }).Select(x => new
            {
                orderDetail_ProductId = x.orderDetail_ProductId,
                Input_Quantity = x.orderDetail_Quantity + x.product_Quantity,
            });
            var odq = query.Where(x=>x.order_TypePayment == 3).GroupBy(x => x.orderDetail_ProductId).Select(x => new {
                product_Title = x.Min(y => y.product_Title),
                orderDetail_Quantity = x.Sum(y => y.orderDetail_Quantity),
                product_Quantity = x.Min(y => y.product_Quantity),
                product_Price = x.Min(y => y.product_Price),
                product_OriginalPrice = x.Min(y => y.product_OriginalPrice),

            })
                .Select(x => new
                {

                    orderDetail_Quantity = x.orderDetail_Quantity,
                    product_Quantity = x.product_Quantity,
                    product_Title = x.product_Title,
                    product_Price = x.product_Price,
                    product_OriginalPrice = x.product_OriginalPrice,
                });
            return Json(new { Data = odq,Data1=jquery }, JsonRequestBehavior.AllowGet);
        }

    }
}