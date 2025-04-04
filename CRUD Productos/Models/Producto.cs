namespace CRUD_Productos.Models
{
    public class Producto
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }

        //Relación con Categoría
        public int CategoriaID { get; set; }
        public Categoria Categoria { get; set; }

        //Relación con Proveedor
        public int ProveedorID { get; set; }
        public Proveedores Proveedores { get; set; }
        
        //Referencia de relación de uno a uno
        public Inventario Inventario { get; set; }
    }
}
