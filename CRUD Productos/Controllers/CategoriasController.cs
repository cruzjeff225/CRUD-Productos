using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD_Productos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_Productos.Db;

namespace CRUD_Productos.Controllers;

public class CategoriasController : Controller
{
    private readonly ILogger<CategoriasController> _logger;
    private readonly ApplicationDbContext _context;

    public CategoriasController(ILogger<CategoriasController> logger, ApplicationDbContext context)
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
        var categorias = await _context.Categorias.ToListAsync();
        return View(categorias);
    }

    // Crear nueva categoría
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(string Nombre)
    {
        var categoria = new Categoria
        {
            Nombre = Nombre
        };
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return View(categoria);
    }

    // Editar categoría existente
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var categorias = await _context.Categorias.FindAsync(id);
        if (categorias == null)
        {
            return NotFound();
        }
        return View(categorias);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(int id, string Nombre)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
        {
            return NotFound();
        }
        categoria.Nombre = Nombre;

        try
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Categoría actualizada correctamente.";
            return RedirectToAction("Index");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoriaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    private bool CategoriaExists(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.ID == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Categoría eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}

