using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Bolsa.Models
{
    public class AcaoUsuario
    {
        [Key]
        public long Id { get; set; }
        
        public long IdUsuarioForeignKey { get; set; }
        [ForeignKey("IdUsuarioForeignKey")]
        [Display(Name = "Usuário")]
        public Usuario IdUsuario { get; set; }

        public long IdAcaoForeignKey { get; set; }
        [ForeignKey("IdAcaoForeignKey")]
        [Display(Name  = "Ação")]
        public Acao IdAcao { get; set; }

        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [Display(Name = "Valor Total")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

    }
}
