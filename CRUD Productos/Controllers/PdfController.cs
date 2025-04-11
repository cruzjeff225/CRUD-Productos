using Microsoft.AspNetCore.Mvc;
using CRUD_Productos.Models;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;
using CRUD_Productos.Db;

namespace CRUD_Productos.Controllers
{
    public class PdfController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PdfController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Productos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedores)
                .Include(p => p.Inventario)
                .ToListAsync();

            return new ViewAsPdf("ProductosPdf", productos)
            {
                FileName = "ReporteProductos.pdf"
            };
        }
    

        public async Task<IActionResult> Categorias()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Productos)
                .ToListAsync();

            return new ViewAsPdf("CategoriasPdf", categorias)
            {
                FileName = "ReporteCategorias.pdf"
            };
        }

        public async Task<IActionResult> Proveedores()
        {
            var proveedores = await _context.Proveedores
                .Include(p => p.Productos)
                .ToListAsync();
            return new ViewAsPdf("ProveedoresPdf", proveedores)
            {
                FileName = "ReporteProveedores.pdf"
            };
        }

        public async Task<IActionResult> Inventarios()
        {
            var inventarios = await _context.Inventarios
                .Include(i => i.Producto)
                .ThenInclude(p => p.Categoria)
                .Include(i => i.Producto)
                .ThenInclude(p => p.Proveedores)
                .ToListAsync();
            return new ViewAsPdf("InventarioPdf", inventarios)
            {
                FileName = "ReporteInventarios.pdf"
            };
        }
    }
}

