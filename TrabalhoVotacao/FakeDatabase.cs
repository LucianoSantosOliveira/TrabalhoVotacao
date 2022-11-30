﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoVotacao.Models.Canditato;
using TrabalhoVotacao.Models.Partido;

namespace TrabalhoVotacao
{
    public class FakeDatabase
    {
        public FakeDatabase() 
        {
            partidos = new List<Partido>();
            canditados = new List<Canditado>();
            vereadores = new List<Vereador>();
            prefeitos = new List<Prefeito>();
        }

        public List<Partido> partidos { get; set; }
        public List<Canditado> canditados { get; set; }
        public List<Prefeito> prefeitos { get; set; }
        public List<Vereador> vereadores { get; set; }
    }
}
