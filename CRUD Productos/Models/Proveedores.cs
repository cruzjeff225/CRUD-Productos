﻿namespace CRUD_Productos.Models
{
    public class Proveedores
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}
