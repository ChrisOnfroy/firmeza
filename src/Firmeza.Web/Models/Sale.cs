using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models;

public class Sale
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string SaleNumber { get; set; } = string.Empty;

    public DateTime SaleDate { get; set; } = DateTime.UtcNow;

    public SaleStatus Status { get; set; } = SaleStatus.Pending;

    [MaxLength(500)]
    public string? DeliveryAddress { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Subtotal { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Tax { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Total { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public ICollection<SaleDetail> Details { get; set; } = new List<SaleDetail>();
}
