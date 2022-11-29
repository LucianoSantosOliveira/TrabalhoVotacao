using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models
{
    public class Prefeito
    {
        public Prefeito() { }
        public Guid Id { get; set; }
        public string nome { get; set; }
        public int QuantidadeDeVotos { get; set; }
        public string Partido { get; set; }
    }
}
