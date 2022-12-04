using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Canditato
{
    public class DeputadoFederal : Canditado
    {
        //gera um Id aleatorio assim que inicia o objeto
        public DeputadoFederal() { Id = Guid.NewGuid(); }
    }
}
