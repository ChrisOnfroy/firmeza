using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class ProductsController(FirmezaDbContext db) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await db.Products.Include(product => product.ProductCategory).AsNoTracking().ToListAsync());

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await db.ProductCategories.AsNoTracking().ToListAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await db.ProductCategories.AsNoTracking().ToListAsync();
            return View(product);
        }

        db.Products.Add(product);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();
        ViewBag.Categories = await db.ProductCategories.AsNoTracking().ToListAsync();
        return View(product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await db.ProductCategories.AsNoTracking().ToListAsync();
            return View(product);
        }

        db.Entry(product).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
