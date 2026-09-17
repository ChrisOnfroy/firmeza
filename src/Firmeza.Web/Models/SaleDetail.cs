using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models;

public class SaleDetail
{
    public int Id { get; set; }

    [Range(0.001, double.MaxValue)]
    public decimal Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Subtotal { get; set; }

    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
