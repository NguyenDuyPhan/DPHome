using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        // GET: Admin/ProductImage
        ApplicationDbContext _dbConnect = new ApplicationDbContext();

        public ActionResult Index(int productId)
        {
            var items = _dbConnect.ProductImages.Where(x => x.ProductId == productId).ToList();
            return View(items);
        }
    }
}