using System.Reflection;
using System.Runtime.ConstrainedExecution;
using TrabalhoVotacao;
using TrabalhoVotacao.Models.Canditato;
using TrabalhoVotacao.Models.Eleicoes;
using TrabalhoVotacao.Models.Partido;

var fakeDataBase = new FakeDatabase() { };
main(null);

void cabecalho(string parteDoSistema, int linhaHorizontals)
{
    Console.WriteLine(parteDoSistema);
    linhaHorizontal(linhaHorizontals);
}
void main(string? mensagem)
{
    Console.Clear();

    cabecalho("Menu principal", 50);

    if (!string.IsNullOrEmpty(mensagem))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(mensagem);
        Console.ReadKey();
        Console.ResetColor();
        Console.Clear();
        cabecalho("Menu principal", 50);
    }

    Console.WriteLine("1 - Para selecionar Modulo Executivo");
    Console.WriteLine("2 - Para selecionar Modulo Legislativo");
    Console.WriteLine("3 - Para cadastrar Canditato");
    Console.WriteLine("4 - Para cadastrar Partido");
    Console.WriteLine("5 - Lista de candidatos");

    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            ModuloExecutivo();
            break;
        case "2":
            ModuloLegislativo();
            break;
        case "3":
            CadastrarCandidato();
            break;
        case "4":
            CadastrarPartido();
            break;
        case "5":
            ListarCandidatos();
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
}

void ListarCandidatos()
{
    Console.Clear();
    cabecalho("Listar Candidatos", 50);
    if (fakeDataBase.prefeitos.Count() != 0)
    {
        Console.WriteLine("Prefeitos | Partidos ");
        foreach (var prefeito in fakeDataBase.prefeitos)
        {
            var dados = $"{prefeito.nome} --- {fakeDataBase.partidos.First(p => p.PartidoId == prefeito.PartidoId).Nome}";
            Console.WriteLine(dados);
        }
    }
    linhaHorizontal(50);
    if(fakeDataBase.vereadores.Count() != 0)
    {
        Console.WriteLine("Vereadores | Partidos ");
        foreach (var vereador in fakeDataBase.vereadores)
        {
            var dados = $"{vereador.nome} --- {fakeDataBase.partidos.First(p => p.PartidoId == vereador.PartidoId).Nome}";
            Console.WriteLine(dados);
        }
    }
    Console.ReadKey();
    main(null);
}

void ModuloLegislativo()
{
    Console.Clear();
    cabecalho("Modulo legislativo", 50);
    Console.WriteLine("1 - Eleições para vereador");
    Console.WriteLine("2 - Eleições para deputados estaduais");
    Console.WriteLine("3 - Eleições para deputados federais");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            EleicoesVereador();
            break;
        case "2":
            EleicoesDeputadoEstadual();
            break;
        case "3":
            EleicoesDeputadoFederal();
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
};

void ModuloExecutivo()
{
    Console.Clear();
    Console.WriteLine("Modulo Executivo");
    linhaHorizontal(50);
    Console.WriteLine("1 - Eleições para Prefeito");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            EleicoesPrefeito();
            break;
        case "2":
            EleicoesPresidente();
            break;
        case "3":
            EleicoesGovernador();
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
};

void InstrucoesArquivoTxt()
{
    Console.Clear();
    Console.ForegroundColor= ConsoleColor.Green;
    Console.WriteLine("Os dados devem ser sepadaros por ';'.\n " +
                      "serão 2 parametros: nome do candidato; quantidadeDevotosValidos \n" +
                      "o nome do candidato deve ser igual ao cadastrado.\n" +
                      "Criar arquivo dentro de bin/debug/net6/arquivo.txt com o nome eleicao.\n" +
                      "Aperte qualquer tecla para continuar");
    Console.ReadLine();
    Console.ResetColor();
}

void EleicoesGovernador()
{
    Console.Clear();
    if (fakeDataBase.presidentes.Count() == 0)
        main("Deve conter candidatos cadastrados.");
    cabecalho("Eleição para prefeito", 50);
    Console.WriteLine("1 - Importar votos por arquivo .txt");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            InstrucoesArquivoTxt();
            LerArquivoTxtGovernador();
            main(null);
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
}

void LerArquivoTxtGovernador()
{
    var path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
    path += @"\eleicao.txt";
    try
    {
        using (StreamReader sr = new StreamReader(path))
        {
            String linha;
            while ((linha = sr.ReadLine()) != null)
            {
                var candidato = linha.Split(';');
                var nome = candidato[0];
                var votos = candidato[1];

                var governador = fakeDataBase.governadores.First(p => p.nome == nome);
                fakeDataBase.governadores[fakeDataBase.governadores.IndexOf(governador)].QuantidadeDeVotos = Int32.Parse(votos);
            }
        }
        CalcularResultadoGovernador();
    }
    catch
    {
        main("Arquivo inválido ou parametro de candidato está incorreto");
    }
}

void CalcularResultadoGovernador()
{
    var totalDevotos = fakeDataBase.governadores.Select(p => p.QuantidadeDeVotos).Sum();

    var votosValidos = fakeDataBase.governadores.Where(p => p.QuantidadeDeVotos > 0)
                                             .Select(x => x.QuantidadeDeVotos).Sum();

    foreach (var prefeito in fakeDataBase.governadores)
    {
        if (prefeito.QuantidadeDeVotos > 0)
        {
            prefeito.PorcentagemDeVotos = prefeito.QuantidadeDeVotos * 100 / votosValidos;
        }
    }

    var prefeitosOrdenadosPorVotos = fakeDataBase.governadores.OrderByDescending(p => p.PorcentagemDeVotos);

    Console.Clear();
    cabecalho("Resultado da eleição para prefeito", 50);

    foreach (var prefeitos in prefeitosOrdenadosPorVotos)
    {
        Console.WriteLine($"Candidato: {prefeitos.nome}, PORCENTAGEM DE VOTOS: {prefeitos.PorcentagemDeVotos}%, QUANTIDADE DE VOTOS: {prefeitos.QuantidadeDeVotos}");
        linhaHorizontal(100);
    }
    Console.WriteLine("Aperte qualquer tecla para continuar");
    Console.ReadKey();
}

void EleicoesPresidente()
{
    Console.Clear();
    if (fakeDataBase.presidentes.Count() == 0)
        main("Deve conter candidatos cadastrados.");
    cabecalho("Eleição para prefeito", 50);
    Console.WriteLine("1 - Importar votos por arquivo .txt");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            InstrucoesArquivoTxt();
            LerArquivoTxtPresidente();
            main(null);
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
}

void LerArquivoTxtPresidente()
{
    var path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
    path += @"\eleicao.txt";
    try
    {
        using (StreamReader sr = new StreamReader(path))
        {
            String linha;
            while ((linha = sr.ReadLine()) != null)
            {
                var candidato = linha.Split(';');
                var nome = candidato[0];
                var votos = candidato[1];

                var presidente = fakeDataBase.presidentes.First(p => p.nome == nome);
                fakeDataBase.presidentes[fakeDataBase.presidentes.IndexOf(presidente)].QuantidadeDeVotos = Int32.Parse(votos);
            }
        }
        CalcularResultadoPresidente();
    }
    catch
    {
        main("Arquivo inválido ou parametro de candidato está incorreto");
    }
}

void CalcularResultadoPresidente()
{
    var totalDevotos = fakeDataBase.presidentes.Select(p => p.QuantidadeDeVotos).Sum();

    var votosValidos = fakeDataBase.presidentes.Where(p => p.QuantidadeDeVotos > 0)
                                             .Select(x => x.QuantidadeDeVotos).Sum();

    foreach (var prefeito in fakeDataBase.presidentes)
    {
        if (prefeito.QuantidadeDeVotos > 0)
        {
            prefeito.PorcentagemDeVotos = prefeito.QuantidadeDeVotos * 100 / votosValidos;
        }
    }

    var prefeitosOrdenadosPorVotos = fakeDataBase.presidentes.OrderByDescending(p => p.PorcentagemDeVotos);

    Console.Clear();
    cabecalho("Resultado da eleição para prefeito", 50);

    foreach (var prefeitos in prefeitosOrdenadosPorVotos)
    {
        Console.WriteLine($"Candidato: {prefeitos.nome}, PORCENTAGEM DE VOTOS: {prefeitos.PorcentagemDeVotos}%, QUANTIDADE DE VOTOS: {prefeitos.QuantidadeDeVotos}");
        linhaHorizontal(100);
    }
    Console.WriteLine("Aperte qualquer tecla para continuar");
    Console.ReadKey();
}

void EleicoesPrefeito()
{
    Console.Clear();
    if(fakeDataBase.prefeitos.Count() == 0)
        main("Deve conter candidatos cadastrados.");
    cabecalho("Eleição para prefeito", 50);
    Console.WriteLine("1 - Importar votos por arquivo .txt");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            InstrucoesArquivoTxt();
            LerArquivoTxtPrefeito();
            main(null);
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
}


void EleicoesDeputadoFederal()
{
    Console.Clear();
    if (fakeDataBase.deputadosFederais.Count() == 0)
        main("Deve conter candidatos cadastrados.");
    cabecalho("Eleição para Deputado Federal", 50);

    Console.WriteLine("Digite a quantidade de vagas disponiveis");

    int qdtVagas = 0;
    try
    {
        var quantidadeDeVagas = Console.ReadLine();
        qdtVagas = int.Parse(quantidadeDeVagas);
    }
    catch
    {
        main("Digite uma quantidade válida");
    }
    Console.WriteLine("1 - Importar votos por arquivo .txt");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            InstrucoesArquivoTxt();
            LerArquivoTxtDeputadoFederal(qdtVagas);
            main(null);
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
}

void EleicoesDeputadoEstadual()
{
    Console.Clear();
    if (fakeDataBase.vereadores.Count() == 0)
        main("Deve conter candidatos cadastrados.");
    cabecalho("Eleição para deputados Estaduais", 50);

    Console.WriteLine("Digite a quantidade de vagas disponiveis");

    int qdtVagas = 0;
    try
    {
        var quantidadeDeVagas = Console.ReadLine();
        qdtVagas = int.Parse(quantidadeDeVagas);
    }
    catch
    {
        main("Digite uma quantidade válida");
    }
    Console.WriteLine("1 - Importar votos por arquivo .txt");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            InstrucoesArquivoTxt();
            LerArquivoTxtDepEstadual(qdtVagas);
            main(null);
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
}

void LerArquivoTxtDepEstadual(int quantidadeVagas)
{
    var path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
    path += @"\eleicao.txt";
    try
    {
        using (StreamReader sr = new StreamReader(path))
        {
            String linha;
            while ((linha = sr.ReadLine()) != null)
            {
                var candidato = linha.Split(';');
                var nome = candidato[0];
                var votos = candidato[1];

                var depEstadual = fakeDataBase.deputadoEstaduals.First(p => p.nome == nome);
                fakeDataBase.deputadoEstaduals[fakeDataBase.deputadoEstaduals.IndexOf(depEstadual)].QuantidadeDeVotos = Int32.Parse(votos);
            }
        }
        CalculaResultadoEleicaoDepEstadual(quantidadeVagas);
    }
    catch
    {
        main("Arquivo inválido ou parametro de candidato está incorreto");
    }
}


void CalculaResultadoEleicaoDepEstadual(int quantidadeVagas)
{
    var quantidadeTotalDeVotos = fakeDataBase.deputadoEstaduals.Select(v => v.QuantidadeDeVotos).Sum();
    float quocienteEleitoral = quantidadeTotalDeVotos / quantidadeVagas;
    var vereadoresPartidoId = fakeDataBase.deputadoEstaduals.Select(v => v.PartidoId).ToList();
    var partidos = fakeDataBase.partidos.Where(p => vereadoresPartidoId.Contains(p.PartidoId)).ToList();

    foreach (var vereador in fakeDataBase.deputadoEstaduals)
    {
        partidos.First(p => p.PartidoId == vereador.PartidoId).QuantidadeDeVotos += vereador.QuantidadeDeVotos;
    }

    foreach (var partido in partidos)
    {
        var arredondamento = Math.Round(partido.QuantidadeDeVotos / quocienteEleitoral);
        var Decimal = (partido.QuantidadeDeVotos / quocienteEleitoral).ToString().Split(',')[0];
        var vagasObtidas = int.Parse(Decimal);
        if (vagasObtidas > 0)
        {
            partido.VagasObtidas = vagasObtidas;
        }
        else
        {
            partido.VagasObtidas = 0;
        }
    }

    foreach (var partido in partidos)
    {
        var mediaSobras = partido.QuantidadeDeVotos / (partido.VagasObtidas + 1);
        partido.mediaSobras = mediaSobras;
    }

    var total = partidos.Select(p => p.VagasObtidas).Sum();
    var sobras = quantidadeVagas - total;
    var partidosOrdenadosPorMediaDeSobras = partidos.OrderByDescending(x => x.mediaSobras).Where(p => p.VagasObtidas > 0);

    foreach (var partido in partidosOrdenadosPorMediaDeSobras)
    {
        sobras = sobras - 1;
        if (sobras < 0)
            break;
        partido.VagasObtidas += 1;
    }

    cabecalho("Resultado eleições para vereador", 50);
    foreach (var partido in partidos)
    {
        var resultado = $"{partido.Nome}," +
                        $"\nNumero de vagas obtidas: {partido.VagasObtidas}\n";

        Console.WriteLine(resultado);

        linhaHorizontal(50);
    }

    Console.WriteLine("Digite qualquer tecla para continuar");
    Console.ReadKey();
    main(null);
}


void EleicoesVereador()
{
    Console.Clear();
    if (fakeDataBase.vereadores.Count() == 0)
        main("Deve conter candidatos cadastrados.");
    cabecalho("Eleição para vereador", 50);

    Console.WriteLine("Digite a quantidade de vagas disponiveis");

    int qdtVagas = 0;
    try
    {
        var quantidadeDeVagas = Console.ReadLine();
        qdtVagas = int.Parse(quantidadeDeVagas);
    }
    catch
    {
        main("Digite uma quantidade válida");
    }
    Console.WriteLine("1 - Importar votos por arquivo .txt");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            InstrucoesArquivoTxt();
            LerArquivoTxtVereador(qdtVagas);
            main(null);
            break;
        default:
            main("Selecione uma opção válida");
            break;
    }
}

void LerArquivoTxtDeputadoFederal(int quantidadeVagas)
{
    var path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
    path += @"\eleicao.txt";
    try
    {
        using (StreamReader sr = new StreamReader(path))
        {
            String linha;
            while ((linha = sr.ReadLine()) != null)
            {
                var candidato = linha.Split(';');
                var nome = candidato[0];
                var votos = candidato[1];

                var deputadoFederal = fakeDataBase.deputadosFederais.First(p => p.nome == nome);
                fakeDataBase.vereadores[fakeDataBase.deputadosFederais.IndexOf(deputadoFederal)].QuantidadeDeVotos = Int32.Parse(votos);
            }
        }
        CalculaResultadoDeputadoFederal(quantidadeVagas);
    }
    catch
    {
        main("Arquivo inválido ou parametro de candidato está incorreto");
    }
}

void CalculaResultadoDeputadoFederal(int quantidadeVagas)
{
    var quantidadeTotalDeVotos = fakeDataBase.deputadosFederais.Select(v => v.QuantidadeDeVotos).Sum();
    float quocienteEleitoral = quantidadeTotalDeVotos / quantidadeVagas;
    var vereadoresPartidoId = fakeDataBase.deputadosFederais.Select(v => v.PartidoId).ToList();
    var partidos = fakeDataBase.partidos.Where(p => vereadoresPartidoId.Contains(p.PartidoId)).ToList();

    foreach (var vereador in fakeDataBase.deputadosFederais)
    {
        partidos.First(p => p.PartidoId == vereador.PartidoId).QuantidadeDeVotos += vereador.QuantidadeDeVotos;
    }

    foreach (var partido in partidos)
    {
        var arredondamento = Math.Round(partido.QuantidadeDeVotos / quocienteEleitoral);
        var Decimal = (partido.QuantidadeDeVotos / quocienteEleitoral).ToString().Split(',')[0];
        var vagasObtidas = int.Parse(Decimal);
        if (vagasObtidas > 0)
        {
            partido.VagasObtidas = vagasObtidas;
        }
        else
        {
            partido.VagasObtidas = 0;
        }
    }

    foreach (var partido in partidos)
    {
        var mediaSobras = partido.QuantidadeDeVotos / (partido.VagasObtidas + 1);
        partido.mediaSobras = mediaSobras;
    }

    var total = partidos.Select(p => p.VagasObtidas).Sum();
    var sobras = quantidadeVagas - total;
    var partidosOrdenadosPorMediaDeSobras = partidos.OrderByDescending(x => x.mediaSobras).Where(p => p.VagasObtidas > 0);

    foreach (var partido in partidosOrdenadosPorMediaDeSobras)
    {
        sobras = sobras - 1;
        if (sobras < 0)
            break;
        partido.VagasObtidas += 1;
    }

    cabecalho("Resultado eleições para vereador", 50);
    foreach (var partido in partidos)
    {
        var resultado = $"{partido.Nome}," +
                        $"\nNumero de vagas obtidas: {partido.VagasObtidas}\n";

        Console.WriteLine(resultado);

        linhaHorizontal(50);
    }

    Console.WriteLine("Digite qualquer tecla para continuar");
    Console.ReadKey();
    main(null);
}

void LerArquivoTxtPrefeito()
{
    var path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
     path += @"\eleicao.txt";
    try
    {
        using (StreamReader sr = new StreamReader(path))
        {
            String linha;
            while ((linha = sr.ReadLine()) != null)
            {
                var candidato = linha.Split(';');
                var nome = candidato[0];
                var votos = candidato[1];

                var prefeito = fakeDataBase.prefeitos.First(p => p.nome == nome);
                fakeDataBase.prefeitos[fakeDataBase.prefeitos.IndexOf(prefeito)].QuantidadeDeVotos = Int32.Parse(votos);
            }
        }
        CalcularResultadoEleicaoPrefeito();
    }
    catch
    {
        main("Arquivo inválido ou parametro de candidato está incorreto");
    }
}

void LerArquivoTxtVereador(int quantidadeVagas)
{
    var path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
    path += @"\eleicao.txt";
    try
    {
        using (StreamReader sr = new StreamReader(path))
        {
            String linha;
            while ((linha = sr.ReadLine()) != null)
            {
                var candidato = linha.Split(';');
                var nome = candidato[0];
                var votos = candidato[1];

                var vereador = fakeDataBase.vereadores.First(p => p.nome == nome);
                fakeDataBase.vereadores[fakeDataBase.vereadores.IndexOf(vereador)].QuantidadeDeVotos = Int32.Parse(votos);
            }
        }
        CalculaResultadoEleicaoVereador(quantidadeVagas);
    }
    catch
    {
        main("Arquivo inválido ou parametro de candidato está incorreto");
    }
}

void CalculaResultadoEleicaoVereador(int quantidadeVagas)
{
    var quantidadeTotalDeVotos = fakeDataBase.vereadores.Select(v => v.QuantidadeDeVotos).Sum();
    float quocienteEleitoral = quantidadeTotalDeVotos / quantidadeVagas;
    var vereadoresPartidoId = fakeDataBase.vereadores.Select(v => v.PartidoId).ToList();
    var partidos = fakeDataBase.partidos.Where(p => vereadoresPartidoId.Contains(p.PartidoId)).ToList();

    foreach (var vereador in fakeDataBase.vereadores)
    {
        partidos.First(p => p.PartidoId == vereador.PartidoId).QuantidadeDeVotos += vereador.QuantidadeDeVotos;
    }

    foreach (var partido in partidos)
    {
        var arredondamento = Math.Round(partido.QuantidadeDeVotos / quocienteEleitoral);
        var Decimal = (partido.QuantidadeDeVotos / quocienteEleitoral).ToString().Split(',')[0];
        var vagasObtidas = int.Parse(Decimal);
        if(vagasObtidas > 0)
        {           
            partido.VagasObtidas = vagasObtidas;
        }
        else
        {
            partido.VagasObtidas = 0;
        }
    }

    foreach(var partido in partidos)
    {
        var mediaSobras = partido.QuantidadeDeVotos / (partido.VagasObtidas + 1);
        partido.mediaSobras = mediaSobras;
    }

    var total = partidos.Select(p => p.VagasObtidas).Sum();
    var sobras = quantidadeVagas - total;
    var partidosOrdenadosPorMediaDeSobras = partidos.OrderByDescending(x => x.mediaSobras).Where(p => p.VagasObtidas > 0);

    foreach (var partido in partidosOrdenadosPorMediaDeSobras)
    {
        sobras = sobras - 1;
        if (sobras < 0)
            break;
        partido.VagasObtidas += 1;
    }

    cabecalho("Resultado eleições para vereador", 50);
    foreach(var partido in partidos)
    {
        var resultado = $"{partido.Nome}," +
                        $"\nNumero de vagas obtidas: {partido.VagasObtidas}\n";

        Console.WriteLine(resultado);

        linhaHorizontal(50);
    }

    Console.WriteLine("Digite qualquer tecla para continuar");
    Console.ReadKey();
    main(null);
}

void CalcularResultadoEleicaoPrefeito()
{
    var totalDevotos = fakeDataBase.prefeitos.Select(p => p.QuantidadeDeVotos).Sum();

    var votosValidos = fakeDataBase.prefeitos.Where(p => p.QuantidadeDeVotos > 0)
                                             .Select(x => x.QuantidadeDeVotos).Sum();

    foreach (var prefeito in fakeDataBase.prefeitos)
    {
        if(prefeito.QuantidadeDeVotos > 0)
        {
            prefeito.PorcentagemDeVotos = prefeito.QuantidadeDeVotos * 100 / votosValidos; 
        }
    }

    var prefeitosOrdenadosPorVotos = fakeDataBase.prefeitos.OrderByDescending(p => p.PorcentagemDeVotos);

    Console.Clear();
    cabecalho("Resultado da eleição para prefeito", 50);

    foreach(var prefeitos in prefeitosOrdenadosPorVotos)
    {
        Console.WriteLine($"Candidato: {prefeitos.nome}, PORCENTAGEM DE VOTOS: {prefeitos.PorcentagemDeVotos}%, QUANTIDADE DE VOTOS: {prefeitos.QuantidadeDeVotos}");
        linhaHorizontal(100);
    }
    Console.WriteLine("Aperte qualquer tecla para continuar");
    Console.ReadKey();
}

//----------------------------------------------
void CadastrarPartido()
{
    var partido = new Partido();
    Console.Clear();
    Console.WriteLine("Cadastrar novo partido: \n");
    linhaHorizontal(50);
    Console.WriteLine("Digite o nome do Partido:");
    var nome = Console.ReadLine();
    partido.Nome = string.IsNullOrEmpty(nome) ? "" : nome;
    fakeDataBase.partidos.Add(partido);
    main(null);
}

void CadastrarCandidato()
{
    Console.Clear();
    if(fakeDataBase.partidos.Count == 0)
        main("Cadastre pelo menos um partido antes de cadastrar um candidato");
    Console.WriteLine("Selecione o tipo do canditato: \n");
    linhaHorizontal(50);

    Console.WriteLine("1-Prefeito \n" +
                      "2-Vereador");

    var op = Console.ReadLine();

    switch (op)
    {
        case "1":
            CadatrarPrefeito();
            break;
        case "2":
            CadatrarVereador();
            break;
        default:
            main(null);
            break;
    }



    main(null);
}

void CadatrarVereador()
{
    var vereador = new Vereador();
    Console.Clear();
    linhaHorizontal(50);
    Console.WriteLine("Digite o nome do Candidato:");
    var nome = Console.ReadLine();
    vereador.nome = string.IsNullOrEmpty(nome) ? "" : nome;

    Console.Clear();
    Console.Write("Selecione qual partido esse candidato pertence: \n \n");

    foreach (var partido in fakeDataBase.partidos)
    {
        var dadosPartido = $"Nome do partido: {partido.Nome} - Numero do partido: {fakeDataBase.partidos.IndexOf(partido)}";
        Console.WriteLine(dadosPartido);
    }

    var partidoSelecionado = Console.ReadLine();
    if (string.IsNullOrEmpty(partidoSelecionado))
    {
        main("Obrigatorio selecionar um partido");
    }
    else
    {
        if (IsNumeric(partidoSelecionado))
            vereador.PartidoId = fakeDataBase.partidos[Convert.ToInt32(partidoSelecionado)].PartidoId;
    }

    fakeDataBase.vereadores.Add(vereador);
}

void CadatrarPrefeito()
{
    var prefeito = new Prefeito();
    Console.Clear();
    linhaHorizontal(50);
    Console.WriteLine("Digite o nome do Candidato:");
    var nome = Console.ReadLine();
    prefeito.nome = string.IsNullOrEmpty(nome) ? "" : nome;

    Console.Clear();
    Console.Write("Selecione qual partido esse candidato pertence: \n \n");

    foreach (var partido in fakeDataBase.partidos)
    {
        var dadosPartido = $"Nome do partido: {partido.Nome} - Numero do partido: {fakeDataBase.partidos.IndexOf(partido)}";
        Console.WriteLine(dadosPartido);
    }

    var partidoSelecionado = Console.ReadLine();
    if (string.IsNullOrEmpty(partidoSelecionado))
    {
        main("Obrigatorio selecionar um partido");
    }
    else
    {
        if (IsNumeric(partidoSelecionado))
        {
            try
            {
                prefeito.PartidoId = fakeDataBase.partidos[Convert.ToInt32(partidoSelecionado)].PartidoId;
            }
            catch { main("Opção invalida"); }
        }
            
    }

    fakeDataBase.prefeitos.Add(prefeito);
}

bool IsNumeric(string numero)
{
    string s = numero;

    int n;
    bool isNumeric = int.TryParse(s, out n);

    Console.WriteLine(n);
    return isNumeric;
}
void linhaHorizontal(int tamanho) { for (var i = 0; i <= tamanho; i++) { Console.Write("_"); } Console.WriteLine("\n"); }