using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Canditato
{
    public class Vereador : Canditado
    {
        public Vereador() { Id = Guid.NewGuid(); }
    }
}
