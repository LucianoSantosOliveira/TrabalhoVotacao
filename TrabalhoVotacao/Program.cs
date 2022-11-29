using TrabalhoVotacao.Models.Eleicoes;

Console.WriteLine("Hello, World!");
var opcao = Console.ReadLine();

switch (opcao)
{
    case "1":
        ModuloExecutivo();
        break;
    case "2":
        ModuloLegislativo();
        break ;
    default:
        break;
}

void ModuloLegislativo()
{
    Console.Clear();
    var eleicaoLegislativo = new EleicaoLegislativo();
    Console.WriteLine("Modulo legislativo");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            Console.WriteLine("Eleições para Vereador");
            break;
        default:
            break;
    }
    Console.ReadLine();
};

void ModuloExecutivo()
{
    Console.Clear();
    var eleicaoExecutivo = new EleicaoExecutivo();
    Console.WriteLine("Modulo legislativo");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            Console.WriteLine("Eleições para Prefeito");
            break;
        default:
            break;
    }
    Console.ReadLine();
};