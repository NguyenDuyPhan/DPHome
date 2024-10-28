using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Post
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if(page==null)
            {
                page = 1;
            }
            IEnumerable<Post> item = _dbConnect.Posts.OrderByDescending(x=>x.Id);
            if(!string.IsNullOrEmpty(Searchtext))
            {
               item= item.Where(x => x.Title.Contains(Searchtext) || x.Alias.Contains(Searchtext)).ToList();  
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
             item = item.ToPagedList(pageIndex,pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Add(Post model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                model.CategoryId = 1;
                _dbConnect.Posts.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = _dbConnect.Posts.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Post model)
        {
            if(ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                _dbConnect.Posts.Attach(model);
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
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.Posts.Find(id);
            if(item!=null)
            {
                _dbConnect.Posts.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]

        public ActionResult Active(int id)
        {
            var item = _dbConnect.Posts.Find(id);
            if(item!=null)
            {
                item.IsActive = !item.IsActive;
                _dbConnect.Entry(item).Property(x => x.IsActive).IsModified = true;
                _dbConnect.SaveChanges();
                return Json(new { success = true,Active=item.IsActive });
            }
            return Json(new { success = false });
        }


    }
}