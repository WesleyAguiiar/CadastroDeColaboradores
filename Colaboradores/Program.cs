using Microsoft.Data.SqlClient;
using Dapper;

namespace Gerenciador
{
    class Principal
    {
        public static void Main(String[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=Colaboradores;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True";
            using SqlConnection connection = new SqlConnection(connectionString);

            Colaborador colaborador = new Colaborador();
            colaborador.Menu(connection);
        }
        public class Colaborador
        {
            public int Id { get; set; } = 0;
            public string Nome { get; set; }
            public string Setor { get; set; }
            public double Pagamento { get; set; }

            public Colaborador(int id, string nome, string setor, double pagamento)
            {
                Id = id;
                Nome = nome;
                Setor = setor;
                Pagamento = pagamento;
            }
            public Colaborador()
            {

            }

            public void Cadastrar(SqlConnection connection)
            {
                Console.WriteLine("------------- Menu de cadastro -------------");
                Console.Write("Digite o nome do colaborador: ");
                var nome = Console.ReadLine();

                Console.Write("Digite o setor do colaborador: ");
                var setor = Console.ReadLine();

                Console.Write("Digite o pagamento do colaborador: ");
                double? pagamento = double.Parse(Console.ReadLine());

                if(nome != null || setor != null || pagamento != null)
                {
                    var insertSql = "INSERT INTO [lista_colaboradores] VALUES (@Nome, @Setor, @pagamento)";
                    var rows = connection.Execute(insertSql, new
                    {
                        nome, 
                        setor, 
                        pagamento
                    });
                }
                else
                {
                    Console.WriteLine("Não é permitido inserir valores nulos.");
                }
                Menu(connection);
            }
            public void Consultar(SqlConnection connection)
            {
                Console.WriteLine("------------- Menu de Consulta -------------");
                Console.Write("1 - Consultar todos\n2 - Consultar por id\n3 - Consultar por setor\n4 - Voltar ao menu principal\n>> ");
                var opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    var colaboradores = connection.Query<Colaborador>("SELECT * FROM [lista_colaboradores]");
                    foreach (var item in colaboradores)
                    {
                        Console.WriteLine($"\nID: {item.Id}");
                        Console.WriteLine($"Nome: {item.Nome}");
                        Console.WriteLine($"Setor: {item.Setor}");
                        Console.WriteLine($"Pagamento: {item.Pagamento}");
                    }
                }
                if( opcao == "2")
                {
                    Console.Write("Digite o id do colaborador: ");
                    var id = int.Parse(Console.ReadLine());

                    var colaboradores = connection.Query<Colaborador>("SELECT * FROM [lista_colaboradores] WHERE [Id]=@id", new {id});
                    foreach (var item in colaboradores)
                    {
                        Console.WriteLine($"\nID: {item.Id}");
                        Console.WriteLine($"Nome: {item.Nome}");
                        Console.WriteLine($"Setor: {item.Setor}");
                        Console.WriteLine($"Pagamento: {item.Pagamento}");
                    }
                }
                if( opcao == "3")
                {
                    Console.Write("Digite o setor do colaborador: ");
                    var setor = Console.ReadLine();

                    var colaboradores = connection.Query<Colaborador>("SELECT * FROM [lista_colaboradores] WHERE [Setor]=@setor", new {setor});
                    foreach (var item in colaboradores)
                    {
                        Console.WriteLine($"\nID: {item.Id}");
                        Console.WriteLine($"Nome: {item.Nome}");
                        Console.WriteLine($"Setor: {item.Setor}");
                        Console.WriteLine($"Pagamento: {item.Pagamento}");
                    }
                }
                if( opcao == "4")
                {
                    Menu(connection);
                }
            }
            public void Remover(SqlConnection connection)
            {
                Console.WriteLine("------------- Menu de Remoção -------------");
                Console.Write("Digite o id do colaborador: ");
                var id = int.Parse(Console.ReadLine());

                var rows = connection.Execute("DELETE FROM [lista_colaboradores] WHERE [Id]=@id", new { id });
                Console.WriteLine($"{rows} linhas removidas.");
            }
            public void Atualizar(SqlConnection connection)
            {
                Console.WriteLine("------------- Menu de Atualização -------------");
                Console.Write("Digite o id do colaborador: ");
                int? id = int.Parse(Console.ReadLine());

                if( id == null)
                {
                    Console.WriteLine("O valor inserido é inválido.");
                    Menu(connection);
                } else
                {
                    Console.Write("Digite o novo nome do colaborador: ");
                    var nome = Console.ReadLine();

                    Console.Write("Digite o novo setor do colaborador: ");
                    var setor = Console.ReadLine();

                    Console.Write("Digite o novo pagamento do colaborador: ");
                    double pagamento = double.Parse(Console.ReadLine());

                    var updateSql = "UPDATE lista_colaboradores SET [Nome] = @nome, [Setor] = @setor, [pagamento] = @pagamento WHERE Id=2";
                    var rows = connection.Execute(updateSql, new
                    {
                        nome,
                        setor,
                        pagamento
                    });
                }
                Menu(connection);
            }
            public void Menu(SqlConnection connection)
            {
                while (true)
                {
                    Console.WriteLine("------------- Menu Principal -------------");
                    Console.Write("1 - Cadastrar colaborador\n2 - Consultar colaborador\n3 - Remover colaborador\n4 - Atualizar colaborador\n5 - Sair\n>> ");
                    var opcao = Console.ReadLine();

                    if (opcao == "1")
                    {
                        Cadastrar(connection);
                    }
                    if (opcao == "2")
                    {
                        Consultar(connection);
                    }
                    if(opcao == "3")
                    {
                        Remover(connection);
                    }
                    if(opcao == "4")
                    {
                        Atualizar(connection);
                    }
                    if( opcao == "5")
                    {
                        System.Environment.Exit(0);
                    }
                }
            }
        }
    }
}