using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models;

public class Customer
{
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string DocumentType { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Address { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

    [EmailAddress, MaxLength(150)]
    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
