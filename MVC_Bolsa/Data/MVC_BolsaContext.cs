using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_Bolsa.Models;

namespace MVC_Bolsa.Models
{
    public class MVC_BolsaContext : DbContext
    {
        public MVC_BolsaContext (DbContextOptions<MVC_BolsaContext> options)
            : base(options)
        {
        }

        public DbSet<MVC_Bolsa.Models.Acao> Acao { get; set; }

        public DbSet<MVC_Bolsa.Models.AcaoUsuario> AcaoUsuario { get; set; }

        public DbSet<MVC_Bolsa.Models.Usuario> Usuario { get; set; }
    }
}
