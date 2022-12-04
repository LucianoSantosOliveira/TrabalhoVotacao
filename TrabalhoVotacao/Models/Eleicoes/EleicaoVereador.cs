using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Eleicoes
{
    public class EleicaoVereador : Eleicoes
    {//gera um Id aleatorio assim que inicia o objeto
        public EleicaoVereador() { EleicaoId = Guid.NewGuid(); }
    }
}
