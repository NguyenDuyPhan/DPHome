using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(int? page)
        {

            IEnumerable<Product> item = _dbConnect.Products.OrderByDescending(x => x.Id);
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            item = item.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item);
        }

        public ActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(_dbConnect.ProductCategories.ToList(),"Id","Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if(ModelState.IsValid)
            {
                if(Images!=null && Images.Count>0)
                {
                    for(int i=0;i<Images.Count;i++)
                    {
                        if(i+1==rDefault[0])
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                isDefault = true,

                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                isDefault = false,

                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                _dbConnect.Products.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(_dbConnect.ProductCategories.ToList(), "Id", "Title");
             return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(_dbConnect.ProductCategories.ToList(), "Id", "Title");
            // var item = _dbConnect.Products.Find(id);
            var item = _dbConnect.Products
    .Include(p => p.ProductImage)  // Lấy kèm danh sách hình ảnh
    .FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            // Truyền danh sách hình ảnh của sản phẩm xuống View
            ViewBag.ProductImages = item.ProductImage.ToList();
            
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Product model, List<string> Images, int? rDefault)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                _dbConnect.Products.Attach(model);
                if (Images != null && Images.Count > 0)
                {
                    // Xóa hình ảnh cũ nếu cần
                    var existingImages = _dbConnect.ProductImages.Where(x => x.ProductId == model.Id).ToList();
                    foreach (var image in existingImages)
                    {
                        _dbConnect.ProductImages.Remove(image);
                    }

                    // Thêm hình ảnh mới
                    for (int i = 0; i < Images.Count; i++)
                    {
                        var productImage = new ProductImage
                        {
                            ProductId = model.Id,
                            Image = Images[i],
                            isDefault = (rDefault.HasValue && rDefault.Value == i + 1) // Đặt hình ảnh đại diện
                        };
                        _dbConnect.ProductImages.Add(productImage);
                    }

                    if (!_dbConnect.ProductImages.Any(x => x.ProductId == model.Id && x.isDefault))
                    {
                        var firstImage = _dbConnect.ProductImages.FirstOrDefault(x => x.ProductId == model.Id); 
                        if (firstImage != null)
                        {
                            firstImage.isDefault = true; // Đặt ảnh đầu tiên làm đại diện nếu không có ảnh nào được chọn
                        }
                    }
                }
                _dbConnect.Entry(model).Property(x => x.Title).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Description).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Alias).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Image).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Detail).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.ModifiedBy).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.IsActive).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.isFeature).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.isHome).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.isHot).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.isSale).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Price).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.PriceSale).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.ProductCategoryId).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Cost).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Quantity).IsModified = true;
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.Products.Find(id);
            if(item!=null)
            {
                _dbConnect.Products.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Active(int id)
        {
            var item = _dbConnect.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _dbConnect.Entry(item).Property(x => x.IsActive).IsModified = true;
                _dbConnect.SaveChanges();
                return Json(new { success = true, Active = item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Home(int id)
        {
            var item = _dbConnect.Products.Find(id);
            if (item != null)
            {
                item.isHome = !item.isHome;
                _dbConnect.Entry(item).Property(x => x.isHome).IsModified = true;
                _dbConnect.SaveChanges();
                return Json(new { success = true, Home = item.isHome });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Hot(int id)
        {
            var item = _dbConnect.Products.Find(id);
            if (item != null)
            {
                item.isHot = !item.isHot;
                _dbConnect.Entry(item).Property(x => x.isHot).IsModified = true;
                _dbConnect.SaveChanges();
                return Json(new { success = true, Hot = item.isHot });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult NhapHang(int id, int quantity)
        {
            var product = _dbConnect.Products.Find(id);
            if (product != null && quantity > 0)
            {
                product.Quantity += quantity;
                product.ModifiedDate = DateTime.Now;
                _dbConnect.Entry(product).Property(x => x.Quantity).IsModified = true;
                _dbConnect.Entry(product).Property(x => x.ModifiedDate).IsModified = true;
                _dbConnect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


    }
}