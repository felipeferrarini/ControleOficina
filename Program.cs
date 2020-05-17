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
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using Microsoft.VisualBasic;

namespace ControleOficina
{
    
    class Program
    {
        public static void showDados(string[,] dados, int qtdLinhas, int qtdColunas, string version)
        {
            Console.Clear();
            Console.WriteLine(version);
            Console.WriteLine("-> Menu de Consulta\n");
            Console.WriteLine("\n\n PARA MELHOR VISUALIZAÇÃO UTILIZE A TELA MAXIMIZADA!\n");
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
        public static void showDados(string[,] dados, int qtdLinhas, int qtdColunas, string status, string version)
        {
            Console.Clear();
            Console.WriteLine(version);
            Console.WriteLine("-> Menu de Consulta\n");
            Console.WriteLine("\n\n PARA MELHOR VISUALIZAÇÃO UTILIZE A TELA MAXIMIZADA!\n");
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

            string version = "Controle De Oficina 0.8\n";
            Console.Clear();
            Console.Write("Carregando Sistema");
            for (int i =0; i<7; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
            Console.Write("Carregado!");
            Thread.Sleep(500);
            bool menu = true;
            string menuback;
            string pathOs = Directory.GetCurrentDirectory() + "\\baseOS.txt";
            string pathClient = Directory.GetCurrentDirectory() + "\\baseClientes.txt";
            if (!File.Exists(pathOs))
            {
                StreamWriter x;
                x = File.CreateText(pathOs);
                x.Close();
            }
            if (!File.Exists(pathClient))
            {
                StreamWriter x;
                x = File.CreateText(pathClient);
                x.Close();
            }
            int qtdColunas = 9;
            string opcao;
            
            string[] tipoVeiculo = new string[2] { "Carro", "Moto" };
            string[] status = new string[4] { "Ag. Peças", "Ag. Execução", "Em Execução", "Finalizada" };

            while (menu)
            {
                Console.Clear();
                Console.WriteLine(version);
                Console.WriteLine("-> Menu Principal\n");
                Console.WriteLine("Escolha uma das Opções:" +
                    "\n 1 - Criar nova Ordem de Serviço" +
                    "\n 2 - Consultar Ordem de Serviço" +
                    "\n 3 - Editar Ordem de Serviço" +
                    "\n 4 - Gerenciar Clientes" +
                    "\n 5 - Gerar Comprovante de Pagamento"+
                    "\n 6 - Sair");
                opcao = Console.ReadLine();
                //Criar nova Ordem de Serviço
                if (opcao == "1")
                {
                    Console.Clear();
                    Console.WriteLine(version);
                    Console.WriteLine("-> Criar Ordem de Serviço\n");
                    string leitura = "";
                    bool error = true;
                    os novaOS = new os(pathOs);
                    int number = os.getNextNumber(pathOs);
                    Console.WriteLine("Ordem de Serviço Número: {0}!", number);

                    while (error)
                    {
                        Console.Write("Informe o CPF ou CNPJ do cliente (Somente os números): ");
                        leitura = Console.ReadLine();
                        Console.WriteLine("");
                        error = client.documentInvalid(leitura);
                    }

                    if (client.documentExist(leitura, pathClient) == false)
                    {
                        Console.WriteLine("Cliente não Cadastrado!\n");
                        Console.WriteLine("Deseja cadastra-lo?\n 1 - Sim\n 2 - Não");
                        opcao = Console.ReadLine();
                        if (opcao == "1")
                        {
                            client cliente = new client();
                            client.createClient(cliente, pathClient, leitura, version);
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
                //Consultar Ordem de Serviço
                if (opcao == "2")
                {
                    Console.Clear();
                    Console.WriteLine(version);
                    Console.WriteLine("-> Menu de Consulta\n");
                    Console.WriteLine("Escolha uma das Opções:" +
                        "\n 1 - Consultar todas as OS's" +
                        "\n 2 - Filtrar por Número" +
                        "\n 3 - Filtrar por Cliente" +
                        "\n 4 - Filtrar por Status");
                    opcao = Console.ReadLine();
                    //Consultar todas as OS's
                    if (opcao == "1")
                    {
                        int qtdLinhas = File.ReadLines(pathOs).Count();
                        if(qtdLinhas == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Nenhuma Ordem de serviço localizada!");
                            Console.WriteLine("\n\n Precione qualquer tecla para voltar ao menu inicial...");
                            menuback = Console.ReadLine();
                        }
                        else
                        {
                            string[,] dados = new string[qtdLinhas, qtdColunas];
                            string[] bd = File.ReadAllLines(pathOs);
                            int i = 0, j = 0;
                            foreach (var element in bd)
                            {
                                string[] line = element.Split(",");
                                foreach (var element2 in line)
                                {
                                    dados[i, j] = line[j];
                                    j++;
                                }
                                i++;
                                j = 0;
                            }
                            Console.Clear();
                            showDados(dados, qtdLinhas, qtdColunas, version);
                            Console.WriteLine("\nPrecione qualquer tecla para voltar ao menu inicial...");
                            menuback = Console.ReadLine();
                        }                        
                    }
                    else
                    //Filtrar por Número
                    if (opcao == "2")
                    {
                        bool error = true;
                        string id;

                        while (error)
                        {
                            Console.WriteLine("Digite o número da OS ou 0 para voltar ao menu inicial:");
                            id = Console.ReadLine();
                            if (id == "0")
                            {
                                error = false;
                            }
                            else
                            {
                                
                                string[] linha = os.returnAllAtributes(pathOs, id);
                                if (linha[0] == "nd")
                                {
                                    Console.WriteLine("Ordem de serviço não encontrada!");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine(version);
                                    Console.WriteLine("-> Menu de Consulta\n");
                                    Console.WriteLine("\nDados da OS:\n");
                                    Console.WriteLine("     Número: {0}", linha[0]);
                                    Console.WriteLine("     Documento do Cliente: {0}", linha[1]);
                                    Console.WriteLine("     Tipo de Veiculo: {0}", linha[2]);
                                    Console.WriteLine("     Placa: {0}", linha[3]);
                                    Console.WriteLine("     Início do Serviço: {0}", linha[4]);
                                    Console.WriteLine("     Previsão de Conclusão: {0}", linha[5]);
                                    Console.WriteLine("     Fim Real do Serviço: {0}", linha[6]);
                                    Console.WriteLine("     Status: {0}", linha[7]);
                                    Console.WriteLine("     Descrição completa: {0}", linha[8]);
                                    Console.WriteLine("\n\n Digite qualquer tecla para voltar ao menu inicial...");
                                    id = Console.ReadLine();
                                    error = false;
                                }
                            }                            
                        }                        
                    }
                    else
                    //Filtrar por Cliente
                    if (opcao == "3")
                    {
                        bool error = true;
                        string leitura = "";
                        int cont = 0;

                        while (error)
                        {
                            Console.WriteLine("Digite o CPF ou CNPJ do cliente (Somente números) ou 0 para voltar ao menu inicial:");
                            leitura = Console.ReadLine();
                            if (leitura == "0")
                            {
                                error = false;
                            }
                            else
                            {
                                leitura = leitura = Regex.Replace(leitura, "[\\,\\/\\-\\ \\.]", "");
                                if (leitura.Length == 14 || leitura.Length == 11)
                                {
                                    error = false;
                                    if (leitura.Length == 14)
                                    {
                                        leitura = Convert.ToUInt64(leitura).ToString(@"00\.000\.000\/0000\-00");
                                    }
                                    else
                                    {
                                        leitura = Convert.ToUInt64(leitura).ToString(@"000\.000\.000\-00");
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
                                        Console.Clear();
                                        Console.WriteLine("\n\n     Nenhum OS encontrada para o cliente!");
                                        Console.WriteLine("\n\n Precione qualquer tecla para voltar ao menu inicial!");
                                        leitura = Console.ReadLine();
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
                                                for (int i = 0; i < 9; i++)
                                                {
                                                    bd2[cont2, i] = line[i];
                                                }
                                                cont2++;
                                            }
                                        }

                                        showDados(bd2, cont, 9, version);
                                        Console.WriteLine("\n\n Precione qualquer tecla para voltar ao menu inicial...");
                                        leitura = Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("CPF ou CNPJ inválido");
                                    error = true;
                                }
                            }
                        }                        
                    }
                    else
                    //Filtrar por Status
                    if (opcao == "4")
                    {
                        bool error = true;
                        string ler;
                        int qtdLinhas = 0;
                        Console.Clear();
                        Console.WriteLine(version);
                        Console.WriteLine("-> Menu de Consulta\n");
                        Console.WriteLine("Escolha o Status:" +
                            "\n 1 - {0}" +
                            "\n 2 - {1}" +
                            "\n 3 - {2}" +
                            "\n 4 - {3}", status[0], status[1], status[2], status[3]);
                        while (error)
                        {
                            int leitura = Convert.ToInt32(Console.ReadLine());
                            if (leitura < 0 || leitura > 4)
                            {
                                Console.WriteLine("\nOpção Inválida!\n");
                            }
                            else
                            {
                                error = false;
                                string[] bd = File.ReadAllLines(pathOs);
                                foreach (var element in bd)
                                {
                                    if (element.Contains(status[leitura - 1]))
                                    {
                                        qtdLinhas++;
                                    }
                                }
                                if (qtdLinhas == 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\n\n     Nenhuma Ordem de Serviço encontrada no status selecionado!");
                                    Console.WriteLine("\n\n Precione qualquer tecla para voltar ao menu inicial...");
                                    ler = Console.ReadLine();
                                }
                                else
                                {
                                    string[,] dados = new string[qtdLinhas, qtdColunas];
                                    int i = 0;
                                    foreach (var element in bd)
                                    {
                                        if (element.Contains(status[leitura - 1]))
                                        {
                                            string[] line = element.Split(",");
                                            for (int j = 0; j < 9; j++)
                                            {
                                                dados[i, j] = line[j];
                                            }
                                            i++;
                                        }
                                    }


                                    showDados(dados, qtdLinhas, qtdColunas, status[leitura - 1]);
                                    Console.WriteLine("\n\n Precione qualquer tecla para voltar ao menu inicial...");
                                    ler = Console.ReadLine();
                                }
                            }
                        }
                        
                        
                    }
                }
                else
                //Editar Ordem de Serviço
                if (opcao == "3")
                {
                    Console.Clear();
                    Console.WriteLine(version);
                    Console.WriteLine("-> Editar Ordem de Serviço\n");
                    bool error = true;
                    string leitura;
                    Console.Write("Digite o número da OS: ");
                    while (error)
                    {
                        leitura = Console.ReadLine();
                        if (leitura.Length > 0)
                        {
                           
                            os.editOS(pathOs, leitura, status, version);
                            error = false;
                        }
                        else
                        {
                            Console.WriteLine("Digite um número válido!");
                        }
                    }
                    
                }
                else
                //Cadastrar Cliente
                if (opcao == "4")
                {
                    Console.Clear();
                    Console.WriteLine(version);
                    Console.WriteLine("-> Gerenciar Clientes\n");
                    Console.WriteLine("Escolha uma das Opções:" +
                    "\n 1 - Cadastrar Cliente" +
                    "\n 2 - Consultar Todos os Clientes" +
                    "\n 3 - Voltar ao menu inicial");
                    opcao = Console.ReadLine();
                    if (opcao == "1")
                    {
                        string ler;
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
                            Console.Clear();
                            Console.WriteLine("     \n\nCliente já Cadastrado!\n");
                            string[] dados = client.returnAllAtributes(pathClient, leitura);
                            Console.WriteLine("   Nome do cliente: {0}", dados[0]);
                            Console.WriteLine("   CPF/CNPJ: {0}", dados[1]);
                            Console.WriteLine("   Endereço: {0}", dados[2]);
                            Console.WriteLine("   E-mail: {0}", dados[3]);
                            Console.WriteLine("   Telefone: {0}", dados[4]);
                            Console.WriteLine("\nDigite qualquer tecla para voltar ao menu inicial...");
                            ler = Console.ReadLine();


                        }
                        else
                        {
                            client cliente = new client();
                            client.createClient(cliente, pathClient, leitura, version);
                        }
                    }
                    else
                    if (opcao == "2"){
                        string espaco = "                        ";
                        Console.Clear();
                        Console.WriteLine(version);
                        Console.WriteLine("-> Lista dos Clientes Cadastrados\n");
                        Console.WriteLine("\n\n PARA MELHOR VISUALIZAÇÃO UTILIZE A TELA MAXIMIZADA!\n");
                        Console.WriteLine("Nome                          |CPF/CNPJ          |E-mail                        |Telefone       ");
                        Console.WriteLine("________________________________________________________________________________________________");
                        string[] bd = File.ReadAllLines(pathClient);
                        foreach(var element in bd)
                        {
                            string[] line = element.Split(",");
                            Console.Write(line[0].Length >= 30 ? line[0].Substring(0, 30) : line[0] + espaco.Substring(0, 30 - line[0].Length));
                            Console.Write("|");
                            Console.Write(line[1].Length > 15 ? line[1] : line[1] + espaco.Substring(0, 18 - line[1].Length));
                            Console.Write("|");
                            Console.Write(line[3].Length >= 30 ? line[3].Substring(0, 30) : line[3] + espaco.Substring(0, 30 - line[3].Length));
                            Console.Write("|");
                            Console.Write(line[4]);
                            Console.WriteLine();
                        }
                        Console.WriteLine("\n\n Precione qualquer tecla para voltar ao menu inicial...");
                        opcao = Console.ReadLine();
                    }
                }else
                if(opcao == "5")
                {
                    bool error = true;
                    string leitura = "";
                    double valor;
                    while (error)
                    {
                        Console.Clear();
                        Console.WriteLine(version);
                        Console.WriteLine("-> Emitir Comprovante de Pagamento\n");
                        Console.WriteLine("Informe o número da OS para gerar o comprovante ou 0 para sair: ");
                        leitura = Console.ReadLine();
                        if(leitura == "0")
                        {
                            error = false;
                        }
                        else
                        {
                            string[] dados = os.returnAllAtributes(pathOs, leitura);
                            if (dados[0] == "nd")
                            {
                                Console.Clear();
                                Console.WriteLine("     \n\nOrdem de Serviço Não Localizada!");
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                Comprovante recibo = new Comprovante(Convert.ToInt32(dados[0]), dados[1], dados[2], dados[3], DateTime.Parse(dados[4]), DateTime.Parse(dados[5]), DateTime.Parse(dados[6]), dados[7], dados[8]);
                                Console.WriteLine("Informe a Forma de pagamento: ");
                                leitura = Console.ReadLine();
                                recibo.PaymentForm = leitura;
                                while (error)
                                {
                                    Console.WriteLine("Informe o Valor do Serviço (R$): ");
                                    leitura = Console.ReadLine().Replace(".", ",");
                                    if (double.TryParse(leitura, out valor))
                                    {
                                        error = false;
                                        valor = double.Parse(leitura, NumberStyles.Float);
                                        recibo.Valor = valor;
                                        if (recibo.printComprovante(pathClient))
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\n\n     Recibo Criado Com Sucesso! Vá até a pasta C:\\Comprovantes");
                                            Console.WriteLine("\n\n Precione qualquer tecla para voltar ao menu inicial...");
                                            leitura = Console.ReadLine();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Ops, Algo deu Errado!");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Valor Inválido");
                                    }
                                }
                            }
                        }
                    }
                }else
                if(opcao == "6")
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
