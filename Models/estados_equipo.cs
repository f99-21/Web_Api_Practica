using System.ComponentModel.DataAnnotations;

namespace Web_Api_Practica.Models
{
    public class estados_equipo
    {
        [Key]
        public int id_estados_equipo {  get; set; }

        public string descripcion { get; set; }

        public string estado {  get; set; }

    }
}
