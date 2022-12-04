using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Canditato
{
    public class Vereador : Canditado
    {//gera um Id aleatorio assim que inicia o objeto
        public Vereador() { Id = Guid.NewGuid(); }
    }
}
