using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Inventario
{
    internal class Estudiantes
    {
        public string IdentificacionEstudiante { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Estrato { get; set; }
        public string Programa { get; set; }
        public string Universidad { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Estudiantes(string identificacion, string nombre, int edad, int estrato, string programa, string universidad, DateTime fechaRegistro)
        {
            IdentificacionEstudiante = identificacion;
            Nombre = nombre;
            Edad = edad;
            Estrato = estrato;
            Programa = programa;
            Universidad = universidad;
            FechaRegistro = fechaRegistro;
        }
    }
}
