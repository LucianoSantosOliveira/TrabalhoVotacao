using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Partido
{
    public class Partido
    {
        public Partido() { PartidoId = Guid.NewGuid(); }

        public Guid PartidoId { get; set; }
        public string Nome { get; set; }
    }
}
