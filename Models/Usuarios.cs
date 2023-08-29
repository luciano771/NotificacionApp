using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificacionApp.Models
{
    public class Usuarios
    {
         
        public string NombreUsuario { get; set; }

         
        public string CorreoElectronico { get; set; }
 
        public string Contraseña { get; set; }

    }

}
