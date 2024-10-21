using System.ComponentModel.DataAnnotations;

namespace Locator.Models;

public class UpdateProfile
{
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name max length is 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name max length is 50 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email Address is required")]
    [StringLength(100, ErrorMessage = "Email Address max length is 100 characters.")]
    public string EmailAddress { get; set; }
}
