using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Eleicoes
{
    public class EleicaoVereador : Eleicoes
    {
        public EleicaoVereador() { EleicaoId = Guid.NewGuid(); }
    }
}
