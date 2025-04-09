using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD_Productos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_Productos.Db;

namespace CRUD_Productos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Index()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Proveedores)
            .ToListAsync();

        return View(productos);
    }

    public IActionResult Create()
    {
        CargarListasDesplegables();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(string Nombre, decimal Precio, int CategoriaID, int ProveedorID)
    {
        var producto = new Producto
        {
            Nombre = Nombre,
            Precio = Precio,
            CategoriaID = CategoriaID,
            ProveedorID = ProveedorID
        };
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        CargarListasDesplegables(); 
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string Nombre, decimal Precio, int CategoriaID, int ProveedorID)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        producto.Nombre = Nombre;
        producto.Precio = Precio;
        producto.CategoriaID = CategoriaID;
        producto.ProveedorID = ProveedorID;

        try
        {
            _context.Update(producto);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Producto actualizado correctamente.";
            return RedirectToAction("Index");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
            
        }

    private bool ProductoExists(object id)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Proveedores)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private void CargarListasDesplegables()
    {
        ViewData["CategoriaID"] = new SelectList(_context.Categorias, "ID", "Nombre");
        ViewData["ProveedorID"] = new SelectList(_context.Proveedores, "ID", "Nombre");
    }
}
