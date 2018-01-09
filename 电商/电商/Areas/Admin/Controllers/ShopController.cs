using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 电商.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {

        static 电商.Areas.Admin.Models.User user =null;
        //
        // GET: /Admin/Shop/
        public ActionResult Index()
        {
            ViewBag.user = user;
            return View();
        }
        public ActionResult Type()
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.Category> list = db.Categories.ToList();
            ViewBag.list = list;
            ViewBag.user = user;
            return View();
        }
        public ActionResult Detail(int id)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.Product> list = db.Product.Where(m => m.CagegoryId == id).ToList();
            ViewBag.list = list;
            ViewBag.user = user;
            return View();
        }
        public ActionResult Product(int id)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Product product = db.Product.Find(id);
            ViewBag.user = user;

            ViewBag.product = product;
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Check(string txtName,string txtPwd)
        {

            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.User> users = db.Users.Where(m => m.name == txtName && m.passward == txtPwd).ToList();
            if (users.Count >= 1)
            {
                user = users[0];
                ViewBag.user = user;
                return RedirectToAction("index");
            }
            else
            {
                user = null;
                ViewBag.user = null;
                return RedirectToAction("login");
            }
           
        }
        public ActionResult Register()
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.User> users = db.Users.ToList();
            List<string> name = new List<string>();
            foreach(电商.Areas.Admin.Models.User item in users)
            {
                name.Add(item.name);
            }
            ViewBag.name = name;
            return View();
        }
        public ActionResult RegisterSave(String txtName, String txtPwd)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.User> list = db.Users.ToList();
            foreach(电商.Areas.Admin.Models.User item in list)
            {
                if(item.name == txtName)
                    return RedirectToAction("Register");
            }
            电商.Areas.Admin.Models.User userr = new Models.User();
            user = userr;
            userr.name = txtName;
            userr.passward = txtPwd;
            db.Users.Add(user);
            db.SaveChanges();
            
            return RedirectToAction("index");
        }
        public ActionResult Basket()
        {
            if(user == null)
               return RedirectToAction("login");
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.Basket> list = db.Baskets.Where(m=>m.name == user.name).ToList();
            ViewBag.user = user;
            ViewBag.list = list;   
            return View();
        }

        public ActionResult logout()
        {
            user = null;
            return RedirectToAction("index");
        }
        public ActionResult addbasket(String id, String num)
        {

            if (user == null)
            {
                return Content("请先登陆");
            }
            if (num == "")
            {
                return Content("请输入购买数量");
            }
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Product pd = db.Product.Find(Convert.ToInt32(id));
            if (pd.num < Convert.ToInt32(num))
                return Content("余量不足");
            电商.Areas.Admin.Models.Basket b = new Models.Basket();
            b.num = Convert.ToInt32(num);
            pd.num -= b.num;
            b.name = user.name;
            b.productid = Convert.ToInt32(id);
            db.Baskets.Add(b);
            db.SaveChanges();
      
            return Content("加入成功");
        }
        public ActionResult delbasket(int id)
        {
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            电商.Areas.Admin.Models.Basket basket = db.Baskets.Find(id);
            basket.Product.num += basket.num;
            db.Baskets.Attach(basket);
            db.Baskets.Remove(basket);
            db.SaveChanges();
            return RedirectToAction("basket");
        }
        public ActionResult submit()
        {
            if (user == null)
                return RedirectToAction("login");
            电商.Areas.Admin.Models.Entities2 db = new Models.Entities2();
            List<电商.Areas.Admin.Models.Basket> list = db.Baskets.Where(m => m.name == user.name).ToList();
            
            foreach(电商.Areas.Admin.Models.Basket item in list)
            {
                电商.Areas.Admin.Models.Order order = new Models.Order();
                order.num = item.num;
                order.productid = item.productid;
                order.name = item.name;
                db.Orders.Add(order);
                db.Baskets.Attach(item);
                db.Baskets.Remove(item);
            }
            db.SaveChanges();
            
            return RedirectToAction("index");
        }
	}
}