using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public ActionResult Index()
        {
            var item = db.Products.ToList();
            return View(item);
        }

        public ActionResult Detail(int id)
        {
            var item = db.Products.Find(id);
            return View(item);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.isHome).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_BestSeller()
        {
            var items = db.Products.Where(x => x.isHot).ToList();
            return PartialView(items);
        }

        public ActionResult Category(int id)
        {
            // Lấy danh sách sản phẩm theo CategoryId
            var products = db.Products.Where(p => p.ProductCategoryId == id).ToList();

            // Truyền danh sách sản phẩm vào View
            return View(products);
        }

        public ActionResult FilterByPrice(decimal? minPrice, decimal? maxPrice)
        {
            var products = db.Products.AsQueryable();

            if (minPrice.HasValue)
            {
                products = products.Where(p => (p.PriceSale.HasValue ? p.PriceSale.Value : p.Price) >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => (p.PriceSale.HasValue ? p.PriceSale.Value : p.Price) <= maxPrice.Value);
            }

            return PartialView("_ProductList", products.ToList());
        }


    }
}