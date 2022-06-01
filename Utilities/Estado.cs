using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Utilities
{
    public class Estado
    {
        public int IDEstado { get; }

        public string nombre { get; }
        public string descripcion { get; }

        public Estado(int iDEstado, string nombre, string descripcion)
        {
            this.IDEstado = iDEstado;
            this.nombre = nombre;
            this.descripcion = descripcion;
        }
    }
}
