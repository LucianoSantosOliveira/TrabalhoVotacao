using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Canditato
{
    public class Governador : Canditado
    {
        public Governador() { Id = Guid.NewGuid(); }
    }
}
