using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazorControls.EDM;

namespace RazorControls.Controllers
{
    public class DefaultController : Controller
    {
        CompanyEntities dc = new CompanyEntities();
        // GET: Default
        public ActionResult Index()
        {
            return View(dc.tblemployees.ToList());
        }

        void FillStates()
        {
            //var data = dc.tblstates.ToList();
            //List<SelectListItem> li = new List<SelectListItem>();
            //foreach (var item in data)
            //{
            //    li.Add(new SelectListItem { Text = item.state_name, Value = item.state_id.ToString() });
            //}
            //ViewBag.states = li;

            var data = from s in dc.tblstates
                       select new SelectListItem
                       {
                           Text = s.state_name,
                           Value=s.state_id.ToString()
                       };

            ViewBag.states = data.ToList();
        }
        public ActionResult Create()
        {
            FillStates();
            return View();
        }
        [HttpPost]
        public ActionResult Create(tblemployee obj,FormCollection fc,HttpPostedFileBase file)
        {
            bool Reading = Convert.ToBoolean(fc["Reading"].Split(',')[0]);
            bool Playing = Convert.ToBoolean(fc["Playing"].Split(',')[0]);
            bool Swimming = Convert.ToBoolean(fc["Swimming"].Split(',')[0]);
            string hoby = "";
            if (Reading)
            {
                hoby += "Reading,";
            }
            if (Playing)
            {
                hoby += "Playing,";
            }
            if (Swimming)
            {
                hoby += "Swimming";
            }
            obj.hobbies = hoby;

            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string fullpath = Path.Combine(Server.MapPath("~/Docs"),filename);
                file.SaveAs(fullpath);
                obj.profileimg = filename;
            }

            dc.tblemployees.Add(obj);
            dc.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}