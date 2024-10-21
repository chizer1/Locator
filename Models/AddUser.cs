using System.ComponentModel.DataAnnotations;

namespace Locator.Models;

public class AddUser
{
    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(50, ErrorMessage = "First Name max length is 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(50, ErrorMessage = "First Name max length is 50 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email Address is required.")]
    [StringLength(50, ErrorMessage = "Emaill Address max length is 100 characters.")]
    public string EmailAddress { get; set; }

    [Required, MinLength(1, ErrorMessage = "Atleast one role is required.")]
    public int[] RoleIds { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Client is required.")]
    public int ClientId { get; set; }
}
