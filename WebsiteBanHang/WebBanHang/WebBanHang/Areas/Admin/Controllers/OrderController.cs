using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using PagedList;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        ApplicationDbContext _dbConnect = new ApplicationDbContext();

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var item = _dbConnect.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if(page==null)
            {
                page = 1;
            }
           
            var pageNumber = page ?? 1;
            var pageSize = 5;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult View(int id)
        {
            var item = _dbConnect.Orders.Find(id); 
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var item = _dbConnect.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(item);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id,int trangthai)
        {
            var item = _dbConnect.Orders.Find(id);
            if(item!=null)
            {
                _dbConnect.Orders.Attach(item);
                item.TypePayment = trangthai;
                _dbConnect.Entry(item).Property(x => x.TypePayment).IsModified = true;
                _dbConnect.SaveChanges();
                return Json(new {message="Success",success=true });
            }
            return Json(new { message = "Unsuccess", success = false });
        }

    }
}