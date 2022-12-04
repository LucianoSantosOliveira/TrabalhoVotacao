using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoVotacao.Models.Canditato;
using TrabalhoVotacao.Models.Partido;

namespace TrabalhoVotacao
{
    //Simulador do banco de dados 
    public class FakeDatabase
    {
        public FakeDatabase() 
        {
            partidos = new List<Partido>();
            canditados = new List<Canditado>();
            vereadores = new List<Vereador>();
            prefeitos = new List<Prefeito>();
            deputadoEstaduals = new List<DeputadoEstadual>();
            deputadosFederais = new List<DeputadoFederal>();
            presidentes= new List<Presidente>();
            governadores = new List<Governador>();

        }

        // Cada lista dessa representa uma tabela no banco
        public List<Partido> partidos { get; set; }
        public List<Canditado> canditados { get; set; }
        public List<Prefeito> prefeitos { get; set; }
        public List<Vereador> vereadores { get; set; }
        public List<DeputadoEstadual> deputadoEstaduals { get; set; }
        public List<DeputadoFederal> deputadosFederais { get; set; }
        public List<Presidente> presidentes { get; set; }
        public List<Governador> governadores { get; set; }
    }
}
