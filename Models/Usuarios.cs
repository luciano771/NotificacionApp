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
       
        public string Nombre { get; set; }
        public string Contraseña { get; set; }

        public string Email { get; set; }

        public int? Edad { get; set; }

        public string TokenNotificacion { get; set; }
    }
}
