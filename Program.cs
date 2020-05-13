using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Win32.SafeHandles;

namespace ControleOficina
{
    
    class Program
    {
        public static void showDados(string[,] dados, int qtdLinhas, int qtdColunas)
        {
            Console.WriteLine(dados[0, 0]);
            Console.WriteLine("Número         |CPF/CNPJ       |Veiculo        |Placa          |Data Início    |Previsão       |Fim real       |Status         |Descrição      ");
            Console.WriteLine(new string('_', 142));
            for (int i = 0; i < qtdLinhas; i++)
            {
                for (int j = 0; j < qtdColunas; j++)
                {
                    if (dados[i, j].Length > 14)
                    {
                        Console.Write(dados[i, j].Substring(0, 12));
                        Console.Write("...|");
                    }
                    else
                    {
                        Console.Write(dados[i, j]);
                        Console.Write(new string(' ', 15 - dados[i, j].Length) + "|");
                    }

                }
                Console.WriteLine();
            }
        }
        public static void showDados(string[,] dados, int qtdLinhas, int qtdColunas, string status)
        {
            Console.WriteLine(dados[0, 0]);
            Console.WriteLine("Número         |CPF/CNPJ       |Veiculo        |Placa          |Data Início    |Previsão       |Fim real       |Status         |Descrição      ");
            Console.WriteLine(new string('_', 142));
            for (int i = 0; i < qtdLinhas; i++)
            {
                if (dados[i, qtdColunas - 2] == status)
                {
                    for (int j = 0; j < qtdColunas; j++)
                    {
                        if (dados[i, j].Length > 14)
                        {
                            Console.Write(dados[i, j].Substring(0, 12));
                            Console.Write("...|");
                        }
                        else
                        {
                            Console.Write(dados[i, j]);
                            Console.Write(new string(' ', 15 - dados[i, j].Length) + "|");
                        }

                    }
                    Console.WriteLine();
                }
                
            }
        }
        static void Main(string[] args)
        {
            bool menu = true;
            StreamReader bdR;
            string pathOs = Directory.GetCurrentDirectory() + "\\bases\\baseOS.txt";
            string pathClient = Directory.GetCurrentDirectory() + "\\bases\\baseClientes.txt";
            int qtdColunas = 9;
            string opcao;
            
            string[] tipoVeiculo = new string[2] { "Carro", "Moto" };
            string[] status = new string[4] { "Ag. Peças", "Ag. Execução", "Em Execução", "Finalizada" };

            while (menu)
            {
                Console.WriteLine("Escolha uma das Opções:" +
                    "\n 1 - Criar nova Ordem de Serviço" +
                    "\n 2 - Consultar Ordem de Serviço" +
                    "\n 3 - Editar Ordem de Serviço" +
                    "\n 4 - Cadastrar Cliente" +
                    "\n 5 - Sair");
                opcao = Console.ReadLine();
                if (opcao == "1")
                {
                    string leitura;
                    bool error = true;
                    os novaOS = new os(pathOs);
                    int number = os.getNextNumber(pathOs);
                    Console.WriteLine("Ordem de Serviço Número: {0}!", number);

                    Console.WriteLine("Informe o CPF ou CNPJ (Somente os números):");
                    leitura = Console.ReadLine();
                    error = client.documentInvalid(leitura);
                    while (error)
                    {
                        Console.WriteLine("Informe o CPF ou CNPJ (Somente os números):");
                        leitura = Console.ReadLine();
                        error = client.documentInvalid(leitura);
                    }

                    if (client.documentExist(leitura, pathClient) == false)
                    {
                        Console.WriteLine("Cliente não Cadastrado!");
                        Console.WriteLine("Deseja cadastra-lo?\n 1 - Sim\n 2 - Não");
                        opcao = Console.ReadLine();
                        if (opcao == "1")
                        {
                            client cliente = new client();
                            client.createClient(cliente, pathClient, leitura);
                        }
                        else
                        {
                            Console.WriteLine("O cliente precisa estar cadastrado para dar continuidade à abertura da OS!");
                            opcao = "g";
                        }

                    }
                    else
                    {
                        os.createOS(pathOs, novaOS, leitura, tipoVeiculo, status);
                    }
                }
                else
                if (opcao == "2")
                {
                    Console.WriteLine("Escolha uma das Opções:" +
                        "\n 1 - Consultar todas as OS's" +
                        "\n 2 - Filtrar por Número" +
                        "\n 3 - Filtrar por Cliente" +
                        "\n 4 - Filtrar por Status");
                    opcao = Console.ReadLine();
                    if (opcao == "1")
                    {
                        int qtdLinhas = File.ReadLines(pathOs).Count();
                        string[,] dados = new string[qtdLinhas, qtdColunas];

                        bdR = File.OpenText(pathOs);

                        while (bdR.EndOfStream != true)
                        {
                            for (int i = 0; i < qtdLinhas; i++)
                            {
                                string[] linha = System.Text.RegularExpressions.Regex.Split(bdR.ReadLine(), ",");
                                int j = 0;
                                foreach (var element in linha)
                                {
                                    dados[i, j] = element;
                                    j++;
                                }
                            }
                        }

                        showDados(dados, qtdLinhas, qtdColunas);

                        bdR.Close();
                    }
                    else
                    if (opcao == "2")
                    {
                        string id;
                        Console.WriteLine("Digite o número da OS:");
                        id = Console.ReadLine();
                        os.consultOSComplet(pathOs, id);

                    }
                    else
                    if (opcao == "3")
                    {
                        bool error = true;
                        string leitura = "";
                        int cont = 0;
                        Console.WriteLine("Digite o CPF ou CNPJ do cliente (Somente números):");
                        
                        while (error)
                        {
                            leitura = Console.ReadLine();
                            if (leitura.Length == 14 || leitura.Length == 11)
                            {
                                error = false;
                                if (leitura.Length == 14)
                                {
                                    leitura = leitura.Substring(0, 2) + "." + leitura.Substring(2, 3) + "." + leitura.Substring(5, 3) + "/" + leitura.Substring(8, 4) + "-" + leitura.Substring(12, 2);
                                }
                                else
                                {
                                    leitura = leitura.Substring(0, 3) + "." + leitura.Substring(3, 3) + "." + leitura.Substring(6, 3) + "-" + leitura.Substring(9, 2);
                                }
                            }
                            else
                            {
                                Console.WriteLine("CPF ou CNPJ inválido");
                                error = true;
                            }
                        }

                        string[] bd = File.ReadAllLines(pathOs);
                        foreach (var element in bd)
                        {
                            string[] line = element.Split(",");
                            if (line[1].Contains(leitura))
                            {
                                cont++;
                            }
                        }
                        if (cont == 0)
                        {
                            Console.WriteLine("Nenhum OS encontrada para o cliente!");
                        }
                        else
                        {
                            string[,] bd2 = new string[cont, 9];
                            int cont2 = 0;
                            foreach (var element in bd)
                            {
                                string[] line = element.Split(",");
                                if (line[1].Contains(leitura))
                                {
                                    for(int i = 0; i < 9; i++)
                                    {
                                        bd2[cont2, i] = line[i];
                                    }
                                    cont2++;
                                }
                            }

                            showDados(bd2, cont, 9);
                        }
                    }
                    else
                    if (opcao == "4")
                    {
                        Console.WriteLine("Escolha o Status:" +
                            "\n 1 - {0}" +
                            "\n 2 - {1}" +
                            "\n 3 - {2}" +
                            "\n 4 - {3}", status[0], status[1], status[2], status[3]);
                        int leitura = Convert.ToInt32(Console.ReadLine());

                        int qtdLinhas = File.ReadLines(pathOs).Count();
                        string[,] dados = new string[qtdLinhas, qtdColunas];

                        bdR = File.OpenText(pathOs);

                        while (bdR.EndOfStream != true)
                        {
                            for (int i = 0; i < qtdLinhas; i++)
                            {
                                string[] linha = System.Text.RegularExpressions.Regex.Split(bdR.ReadLine(), ",");
                                int j = 0;
                                foreach (var element in linha)
                                {
                                    dados[i, j] = element;
                                    j++;
                                }
                            }
                        }

                        showDados(dados, qtdLinhas, qtdColunas, status[leitura - 1]);

                        bdR.Close();
                    }
                }
                else
                if (opcao == "3")
                {
                    string id;
                    Console.WriteLine("Digite o número da OS:");
                    id = Console.ReadLine();
                    os.editOS(pathOs, id, status);
                }
                else
                if (opcao == "4")
                {
                    bool error = true;
                    string leitura;
                    Console.WriteLine("Informe o CPF ou CNPJ (Somente os números):");
                    leitura = Console.ReadLine();
                    error = client.documentInvalid(leitura);
                    while (error)
                    {
                        Console.WriteLine("Informe o CPF ou CNPJ (Somente os números):");
                        leitura = Console.ReadLine();
                        error = client.documentInvalid(leitura);
                    }

                    if (client.documentExist(leitura, pathClient) == true)
                    {
                        Console.WriteLine("Cliente já Cadastrado!");
                    }
                    else
                    {
                        client cliente = new client();
                        client.createClient(cliente, pathClient, leitura);
                    }
                }else
                if(opcao == "5")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Opção Inválida!");
                }
            }
        }
    }
}
