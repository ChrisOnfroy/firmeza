using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class CustomersController(FirmezaDbContext db) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await db.Customers.AsNoTracking().ToListAsync());

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Customer customer)
    {
        if (!ModelState.IsValid) return View(customer);
        db.Customers.Add(customer);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var customer = await db.Customers.FindAsync(id);
        return customer is null ? NotFound() : View(customer);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        if (!ModelState.IsValid) return View(customer);
        db.Entry(customer).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
