using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Web_Api_Practica.Models
{

    public class equiposContext : DbContext
    {
        public equiposContext(DbContextOptions<equiposContext>options):base(options)
        {

        
        }

        public DbSet<equipos> equipos { get; set; }

    }
}
