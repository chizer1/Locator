using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public abstract class UpdateUser(
    int userId,
    string firstName,
    string lastName,
    string emailAddress,
    UserStatus userStatus,
    List<Role> roles
)
{
    [Required]
    public int UserId { get; } = userId;

    [Required]
    public string FirstName { get; } = firstName;

    [Required]
    public string LastName { get; } = lastName;

    [Required]
    public string EmailAddress { get; } = emailAddress;

    public string Password { get; set; }

    [Required]
    public UserStatus UserStatus { get; } = userStatus;

    [Required]
    public List<Role> Roles { get; } = roles;
}
