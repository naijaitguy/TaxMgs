using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxManagementSystem.Models;
using System.Security.Principal;
using System.Web.Security;
using System.Data.SqlClient;

namespace TaxManagementSystem.Controllers
{

   
    public class AccountController : Controller
    {
        private TaxDbContex db = new TaxDbContex();



        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {


            return View();
        }

        [HttpGet]
        public ActionResult About()
        {


            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {


            return View();
        }

        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Contact( ContactInfo model)
        {
            if (Session["UserName"] == null) { }
            else
            {

                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    TempData["C_mgs"] = "Successfully. Thank You For Feedback ";

                    try
                    {

                        db.contactInfos.Add(model);
                        db.SaveChanges();

                    }

                    catch { ModelState.AddModelError("", " Feedback Not Sent "); }


                    return View();
                }

            }

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TaxRegistration obj)
        {
            if (ModelState.IsValid) {
            

                var EmailResult = db.TaxRegistrations.Where(m => m.Email.Equals(obj.Email)).FirstOrDefault();
                if (EmailResult != null) { ModelState.AddModelError("", "Email Already Exist "); return View(); }

                var UseNameResult = db.TaxRegistrations.Where(m => m.UserName.Equals(obj.UserName)).FirstOrDefault();
                if (UseNameResult != null) { ModelState.AddModelError("", " User Name already Exist"); return View(); }

           
                db.TaxRegistrations.Add(obj);
                db.SaveChanges();
            
                TempData["Registered"] = " Registration Successful,";
            
            }


            return View();
        }

        [HttpGet]
        public ActionResult Home()
        {
            if (Session["UserName"] != null)
            {

                return View();
            }
            else { return RedirectToAction("Login", "Account"); }
      
            }
       
        

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login() {


            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Login model) {

            if (ModelState.IsValid)
            {

                var Obj = db.TaxRegistrations.Where( m => m.UserName.Equals(model.UserName) && m.Password.Equals(model.Password)).FirstOrDefault();

                if (Obj != null)
                {

                    Session["UserName"] = Obj.UserName.ToString();
                    return RedirectToAction("Home", "Account");

                }


                else { ModelState.AddModelError("", " Invalid UserName and Password Combination"); }

            }

            return View(model);
  
        }


        [HttpGet]
        public ActionResult LogOff() {

            if (ModelState.IsValid)
            {

                return RedirectToAction("Login", "Account");
            }
            else
            {


                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(string returulr)
        {

            try
            {

                FormsAuthentication.SignOut();
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                Session.Clear();
               System.Web.HttpContext.Current.Session.RemoveAll();

                return RedirectToAction("Login");

            }

            catch
            {
                throw;


            }
        
            
        
        }


      
        [HttpGet]
        public ActionResult CalculateTax()
        {
            if (Session["UserName"] != null)
            {
                Session["TaxableIncom"] = null;
                return View();
            }
            else return RedirectToAction("Login", "Account");

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalculateTax(CalcutaeTax Obj)
        {
            if (ModelState.IsValid)
            {

                Session["TaxableIncom"] = "good";

                int GrossIncome = Convert.ToInt32(Obj.GrossIncome);


                TempData["GrossIncome"] = GrossIncome.ToString("C");
                double ConsAllowance = 0.01 * GrossIncome;

                TempData["ConsolidatedAllowance"] = ConsAllowance.ToString("C");

                int taxrelief = Convert.ToInt32(Obj.TaxRelief);
                TempData["TaxRelief"] = taxrelief.ToString("C");
                

                int TaxableIncome = GrossIncome - Convert.ToInt32( ConsAllowance) - Convert.ToInt32( Obj.TaxRelief);

                TempData["TaxableIncom"] = TaxableIncome.ToString("C");



                int NonTaxableIcome = Convert.ToInt32(ConsAllowance) + Convert.ToInt32(Obj.TaxRelief);

                TempData["NonTaxableIncome"] = NonTaxableIcome.ToString("C");


                 double Tax = 0.24 * TaxableIncome;

                TempData["Tax"] = Tax.ToString("C");



            }
                  

                     return View();

        }


        //get 
        [HttpGet]
        
        public ActionResult ManageAccount( string id)
        {
            if(Session["UserName"] != null)
            {
                string username = Session["UserName"].ToString();
                if (id == null && id != username)
                {

                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                TaxRegistration User = db.TaxRegistrations.Where(m => m.UserName.Equals(id)).FirstOrDefault();

                if (User == null) { return HttpNotFound(); }
                return View(User);

            }
            else return RedirectToAction("Login", "Account");
           

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageAccount(TaxRegistration obj)
        {

            if (Session["UserName"] == null) { return RedirectToAction("Login", "Account"); }

            else
            {

                ModelState.Remove("UserName");
                ModelState.Remove("Password");
                ModelState.Remove("Email");
               
                ModelState.Remove("PassworConfirm");

                if (ModelState.IsValid) {
                    TempData["Mgs"] = " Your Update was Successful";

                    try
                    {
                        
                        string username = Session["UserName"].ToString();
                        var user = db.TaxRegistrations.Where(a => a.UserName.Equals(username)).FirstOrDefault();

                        if (user == null) { ModelState.AddModelError("", " User Not Found"); }
                        else
                        {

                            user.Address = obj.Address;
                            user.FullName = obj.FullName;
                            user.PhoneNo = obj.PhoneNo;
                            db.SaveChanges();

                            TempData["Mgs"] = " Your Update was Successful";
                            return View();


                        }



                    }

                    catch (Exception) { ModelState.AddModelError("", " User Not Found"); }

               



                }


            }
            return View();
        }

        [HttpGet]

        public ActionResult MyProfile( string id)
        {
            if (Session["UserName"] != null)
            {

                if ( id == null)
            {

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            TaxRegistration User  = db.TaxRegistrations.Where(m => m.UserName.Equals(id)).FirstOrDefault();

            if (User == null) { return HttpNotFound(); }
            return View(User);

            }
            else return RedirectToAction("Login", "Account");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult MyProfile()
        {
            if (Session["UserName"] != null)
            {



                return View();

        }
            else return RedirectToAction("Login", "Account");

    }

        [HttpGet]
        public ActionResult Manage( string id ) {

            if (Session["UserName"] != null)
            {

                return View();

            }
            else return RedirectToAction("Login", "Account");

        }


        [HttpPost]
        public ActionResult Manage(string id, Manage model)
        {

            if (Session["UserName"] != null)
            {

                if (ModelState.IsValid) {
                    string username = Session["UserName"].ToString();
                    TaxRegistration FindPasword = db.TaxRegistrations.Where(m => m.Password.Equals(model.OldPassword) && m.UserName.Equals(username)).FirstOrDefault();
                    if (FindPasword == null) { ModelState.AddModelError("", "Your Old Password Does Not Exist"); }
                    else {

                        FindPasword.Password = model.NewPassword;
                        FindPasword.PassworConfirm = model.ConfirmPassword;
                        db.SaveChanges();
                        TempData["P"] = " Password Succesfully Updated ";
                        Session["UserName"] = null;
                        return View();
                        

                    }

                }

                return View();

            }
            else return RedirectToAction("Login", "Account");

        }




        [HttpGet]
        public ActionResult ApplyForTax() {

            if (Session["UserName"] != null)
            {

                return View();

            }
            else return RedirectToAction("Login", "Account");
       
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyForTax(ApplyForTax model)
        {

            if (Session["UserName"] != null)
            { 

                ModelState.Remove("UserTin");
                ModelState.Remove("TaxAmount");
                ModelState.Remove("Payment_Status");
                if (ModelState.IsValid) {

                    try
                    {

                        var FindEmail = db.ApplyForTaxes.Where(m => m.Email.Equals(model.Email)).FirstOrDefault();
                        if (FindEmail == null) {
                            Session["P_Email"] = model.Email;
                            Session["CompanyName"] = model.CompnayName;
                            Session["CompanyAddress"] = model.CompanyPhsicalAddress;
                            Session["CompanyContcat"] = model.CompanyContactNumber;
                            Session["Position"] = model.CurrentPosition;
                            Session["Salary"] = model.CurrentSalary;
                            Session["StaffId"] = model.StaffId;
                            Session["CompanyWebsite"] = model.CompanyWebsite;
                            Session["BVN"] = model.BVN;
                            Session["TaxAmount"] = 0;
                            Session["Payment_Status"] = 0;
                            
                            return RedirectToAction("PreviewApplication", "Account");

                        } else { ModelState.AddModelError("", " User with the same Email Already Exist"); }

                    }
                    catch { }
                    
                }

           
                return View(model);

            }
            else return RedirectToAction("Login", "Account");

        }


        [HttpGet]
        public ActionResult PreviewApplication( ) {


            if (Session["UserName"] != null)
            {

   

            }
            else return RedirectToAction("Login", "Account");


            return View();

        }


        [HttpPost]
       
        public ActionResult PreviewApplication(ApplyForTax model)



        {


            if (Session["UserName"] != null)
            {
                try
                {
                Random random = new Random();
                int randnum = random.Next(0, 1000000);
                string Tin = randnum.ToString("000000");
                string UserTin = " FIRS-" +Tin;


               
                model.CompnayName = Session["CompanyName"].ToString();
                model.CompanyPhsicalAddress = Session["CompanyAddress"].ToString();
                model.CompanyContactNumber = Session["CompanyContcat"].ToString();
                model.CurrentPosition = Session["Position"].ToString();
                model.CurrentSalary = Session["Salary"].ToString();
                model.StaffId = Session["StaffId"].ToString();
                model.CompanyWebsite = Session["CompanyWebsite"].ToString();
                model.BVN = Session["BVN"].ToString();
                model.UserTin = UserTin;
                model.TaxAmount = Session["TaxAmount"].ToString();
                model.Email = Session["P_Email"].ToString();
                    model.Payment_Status = "0";


                
                    var FindTin = db.ApplyForTaxes.Where(m => m.BVN.Equals(model.BVN)).FirstOrDefault();
                    var FindEmail = db.TaxRegistrations.Where(m => m.Email.Equals(model.Email)).FirstOrDefault();

                    if (FindEmail == null) { ModelState.AddModelError("", " Email Not the same as Registration Email"); }
                    else
                    {

                        if (FindTin != null) { ModelState.AddModelError("", " User With The Same BVN Already Exits"); }
                        else
                        {

                            db.ApplyForTaxes.Add(model);
                            db.SaveChanges();

                            var Update = db.TaxRegistrations.Where(m => m.Email.Equals(model.Email)).FirstOrDefault();

                            Update.BVN = model.BVN;

                            db.SaveChanges();
                            Session["UserTin"] = UserTin;
                            return RedirectToAction("Tinpage", "Account");
                        }

                    }
                }
                catch { ModelState.AddModelError("", " Application Failed"); }

            }

            
            else return RedirectToAction("Login", "Account");


            return View();

        }


        [HttpGet]
        public ActionResult Tinpage() {

            if (Session["UserName"] != null)
            {
                return View();

            }

            else {  return RedirectToAction("Login", "Account"); }


        }

        [HttpGet]
        
        public ActionResult PayTax()
        {

            if (Session["UserName"] != null)
            {

                return View();

            }
            else return RedirectToAction("Login", "Account");


         
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayTax(PayTax payTax_Obj)
        {

            if (Session["UserName"] != null)
            {

                if (ModelState.IsValid)
                {

                    try {

                        ApplyForTax User = db.ApplyForTaxes.Where(m => m.UserTin.Equals(payTax_Obj.Tin)).FirstOrDefault();

                        if (User == null) { ModelState.AddModelError("", " TIN Record Not Found "); }
                        else if (User.Payment_Status == "1" ) { ModelState.AddModelError("", " You Have Already Made Payment ");  }

                        else {

                            Session["Tin"] = payTax_Obj.Tin;
                            int Income = Convert.ToInt32(User.CurrentSalary);
                            Session["S"] = User.CurrentSalary;
                            int taxre = Convert.ToInt32(payTax_Obj.TaxRelief);

                            int TaxableIncome = Income - taxre;

                            Session["T"] = TaxableIncome * 0.24;

                            User.TaxAmount = (TaxableIncome * 0.24).ToString("C");
                            User.Payment_Status = "0";

                            db.SaveChanges();

                            return RedirectToAction("UserDetails", "Account");

                        }


                    }

                    catch { ModelState.AddModelError("", " Fail to Connect to Database "); }
                    
                   
                }

                return View();

            }
            else return RedirectToAction("Login", "Account");



        }

        [HttpGet]
        public ActionResult UserDetails()
        {

            if (Session["UserName"] != null)
            {

                string Tin = Session["Tin"].ToString();
             

                try
                {

                    
                    var CombineModel = (from ep in db.ApplyForTaxes
                                        join e in db.TaxRegistrations on ep.BVN equals e.BVN
                                        where ep.UserTin == Tin
                                        select new TaxRegApplication
                                        {
                                            ApplyForTaxs = ep,
                                            TaxRegistrations = e

                                        }).ToList();
                    return View(CombineModel);


                }

                catch  (Exception Ex) { ModelState.AddModelError(Ex.Message, " Could Not Load Data"); }


            }
            else { return RedirectToAction("Login", "Account"); }


            return View();
        }

        [HttpGet]
        public ActionResult PayNow()
        {
            if (Session["UserName"] != null)
            {

          string Tin = Session["Tin"].ToString();


                try
                {

                    ApplyForTax User = db.ApplyForTaxes.Where(m => m.UserTin.Equals(Tin)).FirstOrDefault();
                    if (User != null)
                    {



                        Session["BVN"] = User.BVN;
                        Session["TaxAmount"] = User.TaxAmount;



                        TaxRegistration Reg = db.TaxRegistrations.Where(m => m.BVN.Equals(User.BVN)).FirstOrDefault();
                        if (Reg != null) {

                            Session["Name"] = Reg.FullName;

                        }




                    }



                    return View();


                }

                catch (Exception Ex) { ModelState.AddModelError(Ex.Message, " Could Not Load Data"); }


            }
            else { return RedirectToAction("Login", "Account"); }


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayNow(PaymentRecord model)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    string Username = Session["UserName"].ToString();
                    string Tin = Session["Tin"].ToString();
                    model.BVN = Session["BVN"].ToString();
                    model.Amount = Session["TaxAmount"].ToString();
                    model.Date = DateTime.Now.ToString();

                 try
                    {
                 var User = db.TaxRegistrations.Where(m => m.UserName == Username).FirstOrDefault();
                 var status = db.ApplyForTaxes.Where(m => m.UserTin == Tin).FirstOrDefault();
                        if (User != null && status.Payment_Status == "0")
                        {
                            model.UserId = User.UserId;
                            db.PaymentRecords.Add(model);
                            db.SaveChanges();

                            if (status != null)
                            {

                                status.Payment_Status = "1";
                                db.SaveChanges();

                               TempData["PS"] = "Payment Successful !";
                            }


                        }
                        else { ModelState.AddModelError("", " You Have Already Make Payment"); }

                    }

                    catch (Exception Ex) { ModelState.AddModelError(Ex.Message, " Could Not Load Data"); }


                }
               
            }



            return View();
        }
            
        


    }



}