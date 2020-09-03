using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxManagementSystem.Controllers
{

    public class HomeController : Controller
    {
   
        public ActionResult About()
        {
            if (Session["UserName"] != null)
            {


                ViewBag.Message = "Your application description page.";

                return View();
            }
            else { return RedirectToAction("Login","Account"); }
        }

       
        public ActionResult Contact()
        {

             if (Session["UserName"] != null)
            {


              ViewBag.Message = "Your contact page.";

                return View();
            }
            else { return RedirectToAction("Login","Account"); }
        }

           
    }


}