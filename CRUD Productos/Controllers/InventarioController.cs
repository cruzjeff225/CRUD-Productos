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
            var productos = await _context.Inventarios
                .Include(p => p.Producto)
                .ToListAsync();

            return View(productos);
        }

        public IActionResult Create()
        {
            CargarListasDesplegables();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create( int ProductoId, int Cantidad)
        {
            var inventario = new Inventario
            {
                ProductoID = ProductoId,
                Cantidad = Cantidad
            };

            _context.Inventarios.Add(inventario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private void CargarListasDesplegables()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            CargarListasDesplegables();
            return View(inventario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int ProductoId, int Cantidad)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            inventario.ProductoID = ProductoId;
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
                if (!InventarioExists(inventario.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var inventario = await _context.Inventarios
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(p => p.ID == id);
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

