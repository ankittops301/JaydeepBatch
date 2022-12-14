using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ProjectDemo.EDM;
using ProjectDemo.Models;

namespace ProjectDemo.Areas.Users.Controllers
{
    public class DefaultController : Controller
    {
        ecommerceEntities dc = new ecommerceEntities();
        // GET: Users/Default
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string email = fc["email"];
            string pass = fc["password"];

            var obj = dc.tblUsers.Where(x => x.email == email && x.password == pass).FirstOrDefault();
            if (obj != null)
            {
                Session["UserId"] = obj.user_id;
                Session["UserName"] = obj.f_name;

                return RedirectToAction("HomePage");
            }
            ViewBag.loginerror = "Invalid Email Or Password";
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection fc)
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("HomePage");
        }
        public ActionResult HomePage()
        {
            return View();
        }
        public ActionResult Products()
        {
            return View(dc.tblproducts.ToList());
        }
        public ActionResult Single(int id)
        {
            return View(dc.tblproducts.Find(id));
        }

        public string AddToCart(int id)
        {
            int userid = Convert.ToInt32(Session["UserId"].ToString());

            var cartobj = dc.tblcarts.Where(c => c.product_id == id && c.user_id == userid).FirstOrDefault();
            if (cartobj == null)
            {
                tblcart obj = new tblcart();
                obj.product_id = id;
                obj.qty = 1;
                obj.user_id = userid;

                dc.tblcarts.Add(obj);
                dc.SaveChanges();
            }
            else
            {
                cartobj.qty += 1;
                dc.Entry(cartobj).State = System.Data.Entity.EntityState.Modified;
                dc.SaveChanges();
            }

            return "Product added to cart.";
        }

        public ActionResult Cart()
        {
            int userid = Convert.ToInt32(Session["UserId"].ToString());
            return View(dc.tblcarts.Where(c => c.user_id == userid).ToList());
        }
        public ActionResult Delete(int id)
        {
            dc.tblcarts.Remove(dc.tblcarts.Find(id));
            dc.SaveChanges();
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public string UpdateCartQty(int id, int qty)
        {
            var obj = dc.tblcarts.Find(id);
            obj.qty = qty;

            dc.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            dc.SaveChanges();
            return "Cart Qty updated.";
        }

        public ActionResult Checkout()
        {
            int userid = Convert.ToInt32(Session["UserId"].ToString());
            tblorder obj = new tblorder();
            obj.user_id = userid;
            obj.orderdate = DateTime.Now;
            obj.status = (byte)OrderStatusEnum.Confirmed;
            dc.tblorders.Add(obj);
            dc.SaveChanges();

            tblorderdetail objorderdetail = new tblorderdetail();
            var cartitems = dc.tblcarts.Where(c => c.user_id == userid).ToList();
            foreach (var item in cartitems)
            {
                objorderdetail.order_id = obj.order_id;
                objorderdetail.product_id = item.product_id;
                objorderdetail.qty = item.qty;

                dc.tblorderdetails.Add(objorderdetail);
                dc.SaveChanges();
            }
            dc.tblcarts.RemoveRange(dc.tblcarts.Where(c => c.user_id == userid));
            dc.SaveChanges();
            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(FormCollection fc)
        {
            string email = fc["email"];
            Session["emailotp"] = email;
            var obj = dc.tblUsers.Where(u=>u.email==email).FirstOrDefault();
            if (obj!=null)
            {
                Random r = new Random();
                int otp=r.Next(1000,9999);
                Session["otp"] = otp;
                SendEmail(email, "Reset password OTP", $"Reset password OTP: {otp}");
                return RedirectToAction("ValidateOTP");
            }
            ViewBag.error = "User Not Found.!";
            return View();
        }
        public ActionResult ValidateOTP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ValidateOTP( FormCollection fc)
        {
            string otp = fc["textotp"];
            
            if (otp==Session["otp"].ToString())
            {
                return RedirectToAction("ResetPassword");
            }
            ViewBag.error = "OTP did mot match.!";
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(FormCollection fc)
        {
            string emailotp = Session["emailotp"].ToString();
            var obj = dc.tblUsers.Where(u => u.email == emailotp).FirstOrDefault();
            obj.password = fc["pass"];
            dc.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            dc.SaveChanges();
            
            return RedirectToAction("Login");
        }

        public static void SendEmail(string toEmail, string subject, string msgBody)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("edemo5866@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = msgBody;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("edemo5866@gmail.com", "xjrtrnksiiedohpp");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}