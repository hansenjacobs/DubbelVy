using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dubbelvy.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class EditViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string NameFirst { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string NameMiddle { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string NameLast { get; set; }

        [Required]
        [Display(Name = "Supervisor")]
        public string SupervisorId { get; set; }

        public List<ApplicationUser> Supervisors { get; set; }

        [Display(Name = "Service Date")]
        public System.DateTime ServiceDateTime { get; set; }

        [Display(Name = "Termination Date")]
        public System.DateTime? TerminationDateTime { get; set; }

        [Required]
        public string Role { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string NameFirst { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string NameMiddle { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string NameLast { get; set; }

        [Required]
        [Display(Name = "Supervisor")]
        public string SupervisorId { get; set; }

        public List<ApplicationUser> Supervisors { get; set; }

        [Display(Name = "Service Date")]
        public System.DateTime ServiceDateTime { get; set; }

        [Display(Name = "Termination Date")]
        public System.DateTime? TerminationDateTime { get; set; }

        [Required]
        public string Role { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
