using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NotificacionApp.Models
{
    public class Notas
    {
        
        //public int? NotaId { get; set; } 
        public int UsuarioId { get; set; }
         
        public int? NroNota { get; set; }
      
        public string Titulo { get; set; }
       
        public string Contenido { get; set; }

       
        public DateTime FechaCreacion { get; set; }


        [StringLength(200)]
        public string? Etiquetas { get; set; }
    }
}
