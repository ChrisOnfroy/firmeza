using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required, MaxLength(30)]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CurrentStock { get; set; }

    [Range(0, double.MaxValue)]
    public decimal MinimumStock { get; set; }

    public bool IsActive { get; set; } = true;

    public int ProductCategoryId { get; set; }
    public ProductCategory ProductCategory { get; set; } = null!;

    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
