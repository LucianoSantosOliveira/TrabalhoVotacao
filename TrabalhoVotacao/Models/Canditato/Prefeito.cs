using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Canditato
{
    public class Prefeito : Canditado
    {//gera um Id aleatorio assim que inicia o objeto
        public Prefeito() { Id = Guid.NewGuid(); }

    }
}
