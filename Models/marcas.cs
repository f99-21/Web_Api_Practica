using System.ComponentModel.DataAnnotations;

namespace Web_Api_Practica.Models
{
    public class marcas
    {
        [Key]
        public int id_marcas {  get; set; }

        public string nombre_marca { get; set; }

        public string estados {  get; set; }
    }
}
