using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models;

public class ProductCategory
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
