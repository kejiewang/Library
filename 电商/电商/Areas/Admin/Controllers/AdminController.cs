using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace 电商.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Check(string LoginName, string PWD)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.Admin> lst =
                  db.Admins.Where(m => m.username == LoginName && m.passward == PWD).ToList();
            FormsAuthentication.SignOut();
            if (lst.Count >= 1)
            {
                FormsAuthentication.SetAuthCookie(LoginName, false);
                return Redirect("/admin/admin/index");
            }
            else
            {
                return Redirect("/admin/admin/Login?message=error");
            }
        }

        [Authorize] 
        public ActionResult UserList(string search = "")
        {
            电商.Areas.Admin.Models.Entities2 db =
                new 电商.Areas.Admin.Models.Entities2();
            List<电商.Areas.Admin.Models.User> lst =
             db.Users.Where(m => m.name.Contains(search) ).ToList();

            ViewBag.list = lst;
            ViewBag.search = search;
            return View();
        }
        [Authorize]
        public ActionResult OrderList(string search = "")
        {
            电商.Areas.Admin.Models.Entities2 db =
                new 电商.Areas.Admin.Models.Entities2();
            List<电商.Areas.Admin.Models.Order> lst =
             db.Orders.Where(m => m.name.Contains(search)||m.Product.name.Contains(search)).ToList();

            ViewBag.list = lst;
            ViewBag.search = search;
            return View();
        }
        public ActionResult push(int id)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Order order = db.Orders.Find(id);
            db.Orders.Attach(order);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        //
        // GET: /Admin/Admin/
        [Authorize] 
        public ActionResult Index(string search = "")
        {
            电商.Areas.Admin.Models.Entities2 db =
                new 电商.Areas.Admin.Models.Entities2();
            List<电商.Areas.Admin.Models.Product> lst =
             db.Product.Where(m => m.name.Contains(search) || m.Category.Name.Contains(search)).ToList();
            
            ViewBag.list = lst;
            ViewBag.search = search;
            return View();
        }
        [Authorize] 
        public ActionResult del(int Id)
        {

            电商.Areas.Admin.Models.Entities2 db =  new Models.Entities2();
            delfile(Id);
            电商.Areas.Admin.Models.Product product =
                new Models.Product() { id = Id };
            
            db.Product.Attach(product);
            //Remove()起到了标记当前对象为删除状态，可以删除
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("index");

        }
        [Authorize] 
        public ActionResult edit(int id)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Product product = db.Product.Find(id);
            List<电商.Areas.Admin.Models.Category> list = db.Categories.ToList();
            ViewBag.CategoryList = list;

            ViewBag.product = product;
            return View();
        }
        [Authorize] 
        public ActionResult editsave(string Id, string Price, string ProductName, string Amount,string type)
        {
            int id = Convert.ToInt32(Id);
            电商.Areas.Admin.Models.Entities2 db = new 电商.Areas.Admin.Models.Entities2();
            电商.Areas.Admin.Models.Product item = db.Product.Find(id);
            
            item.name = ProductName;
            item.price = Convert.ToInt32(Price);
            item.num = Convert.ToInt32(Amount);
            item.CagegoryId = Convert.ToInt32(type);
            var file = Request.Files["pic"];
            if (file.FileName != "")
            {
                delfile(item.id);
                string path = Server.MapPath("\\upload\\");
                string filename = Guid.NewGuid().ToString() + ".jpg";
                item.pic = filename;
                file.SaveAs(path + filename);
            }
            db.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize] 
        public void delfile(int id)
        {

            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Product cc = db.Product.Find(id);
            String path = cc.pic;
            System.IO.File.Delete(Server.MapPath("//upload//"+path));
        }
        [Authorize] 
        public ActionResult Add()
        {
            电商.Areas.Admin.Models.Entities2 db =
                new 电商.Areas.Admin.Models.Entities2();
            List<电商.Areas.Admin.Models.Category> CategoryList = db.Categories.ToList();
            ViewBag.CategoryList = CategoryList;
            return View();
        }
        [Authorize] 
        public ActionResult addsave(string Price,string ProductName,string Amount,string type)
        {
            var file = Request.Files["pic"];
            string path = Server.MapPath("\\upload\\");
            string filename = Guid.NewGuid().ToString() + ".jpg";
            file.SaveAs(path + filename);
         
            电商.Areas.Admin.Models.Product item = new 电商.Areas.Admin.Models.Product();
            item.name = ProductName;
            item.price = Convert.ToInt32(Price);
            item.num = Convert.ToInt32(Amount);
            item.pic =  filename;
            item.CagegoryId = Convert.ToInt32(type);
            电商.Areas.Admin.Models.Entities2 db = new 电商.Areas.Admin.Models.Entities2();
            db.Product.Add(item);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize] 
        public ActionResult listCategory(string search = "")
        {
            电商.Areas.Admin.Models.Entities2 db =
                new 电商.Areas.Admin.Models.Entities2();
            List<电商.Areas.Admin.Models.Category> lst =
             db.Categories.Where(m=>m.Name.Contains(search)).ToList();
            ViewBag.search = search;
            ViewBag.categorylist = lst;
            return View();
        }
        [Authorize] 
        public ActionResult listCategoryAdd()
        {
            return View();
        }

        //增加保存
        [Authorize] 
        public ActionResult Categoryaddsave(string ProductName)
        {
            var file = Request.Files["pic"];
            string path = Server.MapPath("\\upload\\");
            string filename = Guid.NewGuid().ToString() + ".jpg";
            file.SaveAs(path + filename);

            电商.Areas.Admin.Models.Category item = new 电商.Areas.Admin.Models.Category();
            item.Name = ProductName;
            item.pic_ = filename;
            电商.Areas.Admin.Models.Entities2 db = new 电商.Areas.Admin.Models.Entities2();
            db.Categories.Add(item);
            db.SaveChanges();
            return RedirectToAction("listCategory");
        }

        //删除类别
        [Authorize] 
        public ActionResult DelCategory(int id)
        {

            电商.Areas.Admin.Models.Entities2 db =  new Models.Entities2();
            delCategoryPic(id);
            电商.Areas.Admin.Models.Category product =
                new Models.Category() { Id = id };
            //删除类别里面的
            List<电商.Areas.Admin.Models.Product> list = db.Product.Where(m => m.CagegoryId == id).ToList();
            foreach (电商.Areas.Admin.Models.Product item in list)
            {
                delfile(item.id);
                db.Product.Attach(item);
                db.Product.Remove(item);

            }
            db.Categories.Attach(product);
            //Remove()起到了标记当前对象为删除状态，可以删除
            db.Categories.Remove(product);
            db.SaveChanges();
            return RedirectToAction("listCategory");
        }
          [Authorize] 
        public void delCategoryPic(int id)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Category cc = db.Categories.Find(id);
            String path = cc.pic_;
            System.IO.File.Delete(Server.MapPath("//upload//" + path));
        }


        //编辑类别
        [Authorize] 
        public ActionResult editCategory(int id)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Category product = db.Categories.Find(id);
            ViewBag.product = product;
            return View();
        }

        [Authorize] 
        public ActionResult editCategorysave(string id, string ProductName)
        {
            int Id = Convert.ToInt32(id);
            电商.Areas.Admin.Models.Entities2 db = new 电商.Areas.Admin.Models.Entities2();
            电商.Areas.Admin.Models.Category item = db.Categories.Find(Id);
            
            var file = Request.Files["pic"];
            if (file.FileName != "")
            {
                delCategoryPic(item.Id);
                string path = Server.MapPath("\\upload\\");
                string filename = Guid.NewGuid().ToString() + ".jpg";
                file.SaveAs(path + filename);
                item.pic_ = filename;
            }
            item.Name = ProductName;
            db.SaveChanges();
            return RedirectToAction("listCategory");
        }
	}
}