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
        //
        // GET: /Admin/Admin/

        //
        public ActionResult Index(string search = "")
        {
            ViewBag.search = search;
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            List<电商.Areas.Admin.Models.person> list = db.person.Where(m => m.name.Contains(search) || m.work.Contains(search)).ToList();
            ViewBag.list = list;
            return View();
        }

        public ActionResult Del(int Id)
        {
            //删除需判断与之关联的的实体是否存在

            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.person p = db.person.Find(Id);
            db.person.Attach(p);
            db.person.Remove(p);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.person p = db.person.Find(id);
            ViewBag.p = p;
            return View();
        }

        public ActionResult EditSave(int id, string name, string phone, string work)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.person p = db.person.Find(id);
            p.work = work;
            p.name = name;
            p.phone = phone;
            db.SaveChanges();
            return RedirectToAction("index");
        }


        public ActionResult Add()
        {

            return View();
        }


        public ActionResult AddSave(string name, string phone, string work)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.person people = new Models.person();
            people.name = name;
            people.phone = phone;
            people.work = work;
            db.person.Add(people);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        /**出版社管理**/
        public ActionResult PublishList(string search = "")
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            List<电商.Areas.Admin.Models.publish> list = db.publish.Where(m => (m.location.Contains(search) || m.name.Contains(search)) && (m.isdeal == 0)).ToList();
            ViewBag.list = list;
            ViewBag.search = search;
            return View();
        }
        public ActionResult PublishListAdd()
        {

            return View();
        }

        public ActionResult PublishListAddSave(string name, string phone, string location, string email)
        {

            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.publish p = new Models.publish();
            p.name = name;
            p.phone = phone;
            p.location = location;
            p.email = email;
            db.publish.Add(p);
            db.SaveChanges();
            List<电商.Areas.Admin.Models.publish> list = db.publish.Where(m=>m.name==name).ToList();

            return Content("添加成功");
        }
        public ActionResult PublishListEdit(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.publish p = db.publish.Find(id);
            ViewBag.p = p;
           
            return View();
            
        }

        public ActionResult PublishListEditSave(int id, string name, string phone, string location, string email)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.publish p = db.publish.Find(id);
            p.name = name;
            p.phone = phone;
            p.location = location;
            p.email = email;
            db.SaveChanges();
            return RedirectToAction("publishlist");
        }

        public ActionResult PublishListDel(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.publish p = db.publish.Find(id);
            List<电商.Areas.Admin.Models.book> booklist = db.book.Where(m => m.pulish_id == id).ToList();
            if (booklist.Count >= 1)
            {
                foreach (电商.Areas.Admin.Models.book item in booklist)
                {
                    item.isdeal = 1;      
                }
            }
            p.isdeal = 1;
            //db.publish.Attach(p);
            //db.publish.Remove(p);
            db.SaveChanges();
            return RedirectToAction("publishlist");
        }


        //图书列表
        public ActionResult BookList(int publishid, string search = "")
        {
            ViewBag.publishid = publishid;
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.publish publish = db.publish.Find(publishid);
            List<电商.Areas.Admin.Models.book> list = db.book.Where(m => (m.pulish_id == publishid && (m.name.Contains(search) || m.publish.name.Contains(search))) && m.isdeal == 0).ToList();
            ViewBag.list = list;
            ViewBag.publish = publish;
            return View();
        }

        public ActionResult BookListAdd(int publishid)
        {
            ViewBag.publishid = publishid;
            return View();
        }
        public ActionResult BookListAddSave(string publishid, string name, string type, string price,string author)
        {
            if (name == "" || type == "")
            {
                return Content("输入格式不对请重新输入");
            }
            else
            {
                try
                {
                    电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
                    电商.Areas.Admin.Models.book b = new Models.book();
                    b.pulish_id = Convert.ToInt32(publishid);
                    b.name = name;
                    b.price = Convert.ToInt32(price);
                    b.type = type;
                    b.author = author;
                    db.book.Add(b);
                    db.SaveChanges();
                    return Content("添加成功");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var msg = string.Empty;
                    var errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                    foreach (var item in errors)
                        msg += item.FirstOrDefault().ErrorMessage;
                    return Content(msg);
                }

            }
        }

        //图书信息更新
        public ActionResult BookListEdit(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.book book = db.book.Find(id);
            ViewBag.book = book;
            return View();
            
        }
        public ActionResult BookListEditSave(int id, string publishid, string name, string type, string price,string author)
        {
            if (name == "" || type == "")
            {
                return Content("输入格式不对请重新输入");
            }
            else
            {
                try
                {
                    电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
                    电商.Areas.Admin.Models.book b = db.book.Find(id);
                    b.name = name;
                    b.price = Convert.ToInt32(price);
                    b.author = author;
                    b.type = type;
                    db.SaveChanges();
                    return Content("编辑成功");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var msg = string.Empty;
                    var errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                    foreach (var item in errors)
                        msg += item.FirstOrDefault().ErrorMessage;
                    return Content(msg);
                }
            }
        }

        /***出版社的图书删除外键***/
        public ActionResult BookListDel(int id)
        {
            //判断有是否有与其关联的外键
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.book b = db.book.Find(id);
            b.isdeal = 1;
            //db.book.Attach(b);
            //db.book.Remove(b);
            db.SaveChanges();
            return RedirectToAction("booklist", new { publishid = b.pulish_id });
        }

        //订单系统
        public ActionResult OrderListAdd(int id = -1)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            List<电商.Areas.Admin.Models.publish> Listp = db.publish.Where(m=>m.isdeal == 0).ToList();
            if (id == -1)
            {
                if (Listp.Count >= 1)
                {
                    id = Listp[0].id;
                }
            }
            List<电商.Areas.Admin.Models.book> Listb = db.book.Where(m => m.pulish_id == id).ToList();

            ViewBag.Listp = Listp;
            ViewBag.Listb = Listb;
            ViewBag.id = id;
            return View();
        }

        public ActionResult OrderListAddSave(int publishid,int bookid,string time,int num)
        {
            try
            {
                电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
                电商.Areas.Admin.Models.order order = new Models.order();
                order.publish_id = publishid;
                order.book_id = bookid;
                order.time = time;
                order.num = num;
                db.order.Add(order);
                db.SaveChanges();
                return Content("添加成功");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var msg = string.Empty;
                var errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                foreach (var item in errors)
                    msg += item.FirstOrDefault().ErrorMessage;
                return Content(msg);
            }
        }

        public ActionResult OrderList(string search = "")
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            List<电商.Areas.Admin.Models.order> list = db.order.Where(m=>m.book.name.Contains(search)||m.publish.name.Contains(search)).ToList();
            ViewBag.list = list;
            return View();
        }

        public ActionResult OrderListDel(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.order o = db.order.Find(id);
            db.order.Attach(o);
            db.order.Remove(o);
            db.SaveChanges();
            return RedirectToAction("orderlist");
        }

        //加入图书馆的库存中
        public ActionResult AddtoLibrary(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.order o = db.order.Find(id);
            List<电商.Areas.Admin.Models.bookstatus> list = db.bookstatus.Where(m => m.book_id == o.book_id).ToList();
            if (list.Count>= 1)
            {
                ViewBag.location = list[0].location;
            }
            else
            {
                ViewBag.location = "";
            }
            ViewBag.order = o;
            ViewBag.orderid = id;
            ViewBag.o = o;
            return View();
        }


        public ActionResult AddtoLibrarySave(int orderId, string location , string finishtime)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.order order = db.order.Find(orderId);
            order.isFinish = 1;
            order.endtime = finishtime;
            List<电商.Areas.Admin.Models.bookstatus> bs = db.bookstatus.Where(m => m.book_id == order.book_id && m.location == location).ToList();
            if(bs.Count >= 1)
            {
                电商.Areas.Admin.Models.bookstatus b = bs[0];
                b.num += order.num;
            }
            else
            {
                电商.Areas.Admin.Models.bookstatus b = new Models.bookstatus();
                b.num = order.num;
                b.location = location;
                b.book_id = order.book_id;
                db.bookstatus.Add(b);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //书籍的清单
        public ActionResult BookStatusList(string search = "")
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            List<电商.Areas.Admin.Models.bookstatus> list = db.bookstatus.Where(m=>m.book.name.Contains(search)||m.book.type.Contains(search)||m.book.publish.name.Contains(search)).ToList();
            ViewBag.list = list;
            return View();
        }

        //添加借书请求
        public ActionResult BringListAdd(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.bookstatus b = db.bookstatus.Find(id);
            ViewBag.b = b;
            ViewBag.id = id;
            return View();
        }

        public ActionResult BringListAddSave(int bookid,string time, string user)
        {
            int userId = Convert.ToInt32(user);
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.bookstatus bookstaus = db.bookstatus.Find(bookid);
            if (bookstaus.num - bookstaus.bringnum <= 0)
            {
                return Content("余量不足");
            }
            List<电商.Areas.Admin.Models.person> list = db.person.Where(m => m.id == userId).ToList();
            if(list.Count>=1)
            {
                try
                {
                    电商.Areas.Admin.Models.person this_people = list[0];
                    电商.Areas.Admin.Models.bring b = new Models.bring();
                    b.book_id = bookid;
                    b.person_id = this_people.id;
                    b.bringtime = time;
                    bookstaus.num--;
                    db.bring.Add(b);
                    db.SaveChanges();
                    return Content("借阅成功");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var msg = string.Empty;
                    var errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                    foreach (var item in errors)
                        msg += item.FirstOrDefault().ErrorMessage;
                    return Content(msg);
                }
            }
            else
            {
                return Content("用户不存在");
            }
        }

        public ActionResult BringList(string search = "")
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            List<电商.Areas.Admin.Models.bring> list = db.bring.Where(m => m.person.name.Contains(search) || m.bookstatus.book.name.Contains(search)).ToList();
            ViewBag.list = list;
            return View();
        }

        public ActionResult ReturnBook(int id)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.bring b = db.bring.Find(id);
            ViewBag.b = b;
            return View();
        }
        public ActionResult ReturnBookSave(int id,string returntime)
        {
            电商.Areas.Admin.Models.Entities4 db = new Models.Entities4();
            电商.Areas.Admin.Models.bring bring = db.bring.Find(id);
            bring.isreturn = 1;
            bring.returntime = returntime;
            bring.bookstatus.bringnum++;
            db.SaveChanges();
            return RedirectToAction("BookStatusList");
        }

    }
}