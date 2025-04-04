namespace CRUD_Productos.Models
{
    public class Inventario
    {
        public int ID { get; set; }
        public int ProductoID { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
