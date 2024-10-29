using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public class AddUser
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string EmailAddress { get; set; }

    [Required]
    public List<Role> Roles { get; set; }

    [Required]
    public UserStatus UserStatus { get; set; }
}
