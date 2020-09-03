using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxManagementSystem.Models
{
    public class TaxRegistration
    {
        [Key]
        public int UserId { get; set; }

        public string BVN { get; set; }

        [ForeignKey("BVN")]

        public ApplyForTax ApplyForTax { get; set; }

        [Required]
        [Display(Name = " Email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [Display(Name = " User Name")]
        [MaxLength(20, ErrorMessage = " User Name Can not be more than 20 Characters"), MinLength(3, ErrorMessage = "User Name Can Not be less than 3 Characters")]
        public string UserName { get; set; }


        [Required]
        [MaxLength(20, ErrorMessage = " Password Can not be more than 20 Characters"), MinLength(6, ErrorMessage = "Password Can Not be less than 6 Characters")]
        [Display(Name = " Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = " Password Can not be more than 20 Characters"), MinLength(6, ErrorMessage = "Password Can Not be less than 6 Characters")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do Not Match")]

        public string PassworConfirm { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNo { get; set; }



   
    }

    public partial class Login
    {


        [Required]

        [Display(Name = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Enter Your User Name")]
        public string UserName { get; set; }


    }


    public partial class CalcutaeTax
    {
        [Required(ErrorMessage = " Gross Income field is required")]
        [Display(Name = "Gross Income (Total Amount Earned)")]
        [DataType(DataType.Currency)]
        public string GrossIncome { get; set; }


        [DataType(DataType.Currency)]
        [Required(ErrorMessage = " Tax Relief field is required ")]
        [Display(Name = "Tax Relief e.g Pension, Insurance  ")]
        public string TaxRelief { get; set; }


        public string ConsolidatedAllowance { get; set; }


        public string TaxableIcome { get; set; }













    }

    public partial class ManageAccount
    {


        [Required]
        [Display(Name = " Enter New Password")]
        public string NewPassword { get; set; }


        [Required]
        [Display(Name = " Enter Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Password do Not Match")]
        public string ConfirmPassword { get; set; }


    }


    public partial class Manage
    {
        [Display(Name ="Password")]
        [Required]
        [DataType(DataType.Password)]

        public string OldPassword { get; set; }

        [Display(Name = " New Password")]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = " Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password do Not Match")]
        public string ConfirmPassword { get; set; }

    }


    public partial class ApplyForTax {

       
       

        [Required]
        public string UserTin { get; set; }

        [Required]
        [Display(Name = " Email ( Same with Email use for Registration)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        

        public string TaxAmount { get; set; }

        [Required]
        [Display(Name = " Compnay Name")]
        public string CompnayName {get; set;}

        [Key]
        [Required]
        [Display(Name = " Bank Verification Number (BVN)")]
        public string BVN { get; set; }


        [Required]
        [Display(Name = "Company Phsical Address")]

        public string CompanyPhsicalAddress { get; set; }


        [Required]
        [Display(Name = " Staff Id")]

        public string StaffId { get; set; }


        [Required]
        [Display(Name = " CurrentPosition")]
        public string CurrentPosition { get; set; }


        [Required]
        [Display(Name = " Current Salary ")]
        public string CurrentSalary { get; set; }


        [Required]
        [Display(Name = " Company Phone Number")]
        public string CompanyContactNumber { get; set; }


        [Required]
        [Display(Name = " Company Website")]
        public string CompanyWebsite { get; set; }

        [Required]
        public string Payment_Status { get; set; }
    }

    public partial class PayTax {

        [Required]
        [Display(Name ="Pls Enter Your Tax Identification Number (TINs)")]
        public string Tin { get; set; }


        [Required]
        [Display(Name = "Tax Relief Amounts")]
        public string TaxRelief { get; set; }

    }



    public partial class AdminUser


    {

        [Key]
        public int AdminId { get; set; }

        [Required]
        [Display(Name ="Enter Your User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string LastLogin { get; set; }



        public string FullName { get; set; }
    }



    public class TaxRegApplication {

        public ApplyForTax ApplyForTaxs { get; set; }

        public TaxRegistration TaxRegistrations { get; set; }

        public PaymentRecord PaymentRecords { get; set; }

    }


    public class ContactInfo {


        [Key]
        public int Id  {get; set;}

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string FeedBack { get; set; }

       

    }

    public class PaymentRecord {

        [Key]
  
       public int ID { get; set; }

        [Required]
        [Display(Name ="Enter Card Number")]
        [StringLength(maximumLength: 18, MinimumLength = 18, ErrorMessage = " CV2 Must be 18 Digit Number")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = " Card Number Must be a Number.")]
        public string CardNumber { get; set; }

        [Required]
        
        [Display(Name = "Enter Expiring Date")]
        [DataType(DataType.DateTime)]
        public string Expired_Date { get; set; }

        [Required]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = " CV2 Must be three Digit")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = " Cv2 Must be a Number.")]
        public string Cv2 { get; set; }
        [Required]
        [StringLength(maximumLength:4,MinimumLength =4,ErrorMessage = " Pin Must be Four Digit")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Pin Must be a Number.")]
        public string Pin { get; set; }

        public string Date { get; set; }

        public string Amount { get; set; }

        public string BVN { get; set; }

        [ForeignKey("BVN")]

        public ApplyForTax ApplyForTax { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]

        public TaxRegistration TaxRegistration { get; set; }
        

    }

}

