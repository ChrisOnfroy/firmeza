using Microsoft.AspNetCore.Identity;

namespace Firmeza.Web.Models;

public class ApplicationUser : IdentityUser
{
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
}
