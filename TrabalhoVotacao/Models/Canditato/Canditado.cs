﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoVotacao.Models.Canditato
{
    public class Canditado
    {//Classe que é herdada por todos os canditados
        public Guid Id { get; set; }
        public Guid PartidoId { get; set; }
        public string nome { get; set; }
        public int QuantidadeDeVotos { get; set; }
        public decimal PorcentagemDeVotos { get; set; }

    }

}
