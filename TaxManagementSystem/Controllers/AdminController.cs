using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaxManagementSystem.Models;

namespace TaxManagementSystem.Controllers
{
    public class AdminController : Controller
    {

        private TaxDbContex Db = new TaxDbContex();


        // GET: Admin

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // post: admin-index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AdminUser adminUser) {


            adminUser.LastLogin = DateTime.Now.ToString();
            ModelState.Remove("FullName");
            ModelState.Remove("LastLogin");

            if (ModelState.IsValid)
            {

                try {

                    var Result = Db.AdminUsers.Where(m => m.UserName.Equals(adminUser.UserName) && m.Password.Equals(adminUser.Password)).FirstOrDefault();
                    if (Result != null)
                    {
                      
                        Session["AdminUser"] = Result.UserName;

                        Session["AdminLastLogin"] = Result.LastLogin;
                        Result.LastLogin = adminUser.LastLogin;
                        Db.SaveChanges();

                        return RedirectToAction("AdminPanel", "Admin");

                    }
                    else { ModelState.AddModelError("", " Invalid User Name And Password Combination"); }

                }

                catch { ModelState.AddModelError("", " Connection Failed"); }


            }
            else { ModelState.AddModelError("", " Validation Failed"); }


            return View();

        }


        //get 
        [HttpGet]
        public ActionResult AdminPanel(ApplyForTax model)
        {

            if (Session["AdminUser"] == null) { return RedirectToAction("Index", "Admin"); }
            else
            {
               var Result = Db.ApplyForTaxes.ToList();

                if (Result.Count == 0) {

                    TempData["Mgs1"] = " Record Not Found ";

                    return View(Result);
                }
                else
                {

                    return View(Result);
                }
            }

            
        }        
    

        //post 
        [HttpPost]
        public ActionResult AdminPanel( AdminUser adminUser)
        {
            if (Session["AdminUser"] == null) { return RedirectToAction("Index", "Admin"); }
            else
            {


            }

            return View();

        
        }

    
        [HttpGet]
       
        public ActionResult LogOff(string returulr)
        {

            try
            {

                FormsAuthentication.SignOut();
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();

                return RedirectToAction("Index","Admin");

            }

            catch
            {
                throw;


            }



        }


        [HttpGet]

        public ActionResult Details(string id) {

            if (Session["AdminUser"] != null)
            {

                if (id == null)
                {

                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                ApplyForTax User = Db.ApplyForTaxes.Where(m => m.BVN.Equals(id)).FirstOrDefault();

                if (User == null) { return HttpNotFound(); }
                return View(User);

            }
            else return RedirectToAction("Index", "Admin");


           
        }




        [HttpGet]

        public ActionResult Edit(string id)
        {

            if (Session["AdminUser"] != null)
            {

                if (id == null)
                {

                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                ApplyForTax User = Db.ApplyForTaxes.Where(m => m.BVN.Equals(id)).FirstOrDefault();

                if (User == null) { return HttpNotFound(); }
                return View(User);

            }
            else return RedirectToAction("Index", "Admin");



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id,ApplyForTax model)
        {

            if (Session["AdminUser"] != null)
            {

                ModelState.Remove("UserTin");
                ModelState.Remove("Email");
                ModelState.Remove("BVN");
                ModelState.Remove("Payment_Status");
                ModelState.Remove("TaxAmount");

                if (ModelState.IsValid)
                {
                    if (id == null)
                    {

                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                    }
                    ApplyForTax User = Db.ApplyForTaxes.Where(m => m.BVN.Equals(id)).FirstOrDefault();

                   
                    if (User == null) { return HttpNotFound(); }
                    else
                    {
                        User.CompanyContactNumber = model.CompanyContactNumber;
                        User.CompanyPhsicalAddress = model.CompanyPhsicalAddress;
                        User.CompanyWebsite = model.CompanyWebsite;
                        User.CompnayName = model.CompnayName;
                        User.CurrentPosition = model.CurrentPosition;
                        User.CurrentSalary = model.CurrentSalary;
                        User.StaffId = model.StaffId;
                         Db.SaveChanges();
                        TempData["Suc"] = " Update was Successfully";

                    }
                    
                }

                return View();
            }
            else { return RedirectToAction("Index", "Admin"); }



        }



        [HttpGet]

        public ActionResult Delete(string id, ApplyForTax model)
        {

            if (Session["AdminUser"] != null)
            {

                if (id == null)
                {

                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    ApplyForTax User = Db.ApplyForTaxes.Where(m => m.BVN.Equals(id)).FirstOrDefault();
                    TaxRegistration TAxUser = Db.TaxRegistrations.Where(m => m.BVN == id).FirstOrDefault();
                    if (User == null && TAxUser == null) { return HttpNotFound(); }
                    else
                    {
                        TAxUser.BVN = null;
                        Db.SaveChanges();
                        Db.ApplyForTaxes.Remove(User);
                        Db.SaveChanges();

                        TempData["D"] = " User Removed Successfully";
                        return RedirectToAction("AdminPanel", "Admin");

                    }


                }
            }
            else

            { return RedirectToAction("Index", "Admin"); }


          
        }






    }





}