using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD_Productos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_Productos.Db;

public class InventarioController : Controller
{
    private readonly ILogger<InventarioController> _logger;
    private readonly ApplicationDbContext _context;

    public InventarioController(ILogger<InventarioController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var inventarios = await _context.Inventarios
            .Include(i => i.Producto)
            .ThenInclude(p => p.Categoria)
            .ToListAsync();

        return View(inventarios);
    }

    public IActionResult Create()
    {
        ViewData["ProductoID"] = new SelectList(_context.Productos, "ID", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int ProductoID, int Cantidad)
    {
        var inventario = new Inventario
        {
            ProductoID = ProductoID,
            Cantidad = Cantidad
        };

        _context.Inventarios.Add(inventario);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var inventario = await _context.Inventarios.FindAsync(id);
        if (inventario == null) return NotFound();

        ViewData["ProductoID"] = new SelectList(_context.Productos, "ID", "Nombre", inventario.ProductoID);
        return View(inventario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, int ProductoID, int Cantidad)
    {
        var inventario = await _context.Inventarios.FindAsync(id);
        if (inventario == null) return NotFound();

        inventario.ProductoID = ProductoID;
        inventario.Cantidad = Cantidad;

        try
        {
            _context.Update(inventario);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "El inventario se ha actualizado correctamente";
            return RedirectToAction("Index");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InventarioExists(id)) return NotFound();
            else throw;
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var inventario = await _context.Inventarios
            .Include(i => i.Producto)
            .FirstOrDefaultAsync(i => i.ID == id);
        if (inventario == null)
        {
            return NotFound();
        }
        return View(inventario);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inventario = await _context.Inventarios.FindAsync(id);
        _context.Inventarios.Remove(inventario);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private bool InventarioExists(int id)
    {
        return _context.Productos.Any(e => e.ID == id);
    }
}

