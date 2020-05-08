using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace ControleOficina
{
    class Program
    {
        public static void showDados(string[,] dados, int qtdLinhas, int qtdColunas)
        {
            Console.WriteLine(dados[0, 0]);
            Console.WriteLine("Número         |CPF/CNPJ       |Veiculo        |Placa          |Descrição      |Status         ");
            Console.WriteLine(new string('_', 95));
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
            Console.WriteLine("Número         |CPF/CNPJ       |Veiculo        |Placa          |Descrição      |Status         ");
            Console.WriteLine(new string('_', 95));
            for (int i = 0; i < qtdLinhas; i++)
            {
                if (dados[i, qtdColunas - 1] == status)
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
            StreamWriter bdW;
            StreamReader bdR;
            string Caminho = Directory.GetCurrentDirectory() + "\\bases\\baseOS.txt";
            int qtdColunas = 6;
            int opcao = 0;
            string cpf, placa;
            string[] tipoVeiculo = new string[2] { "Carro", "Moto" };
            string[] status = new string[4] { "Ag. Peças", "Ag. Execução", "Em Execução", "Finalizada" };

            Console.WriteLine("Escolha uma das Opções:\n 1 - Criar nova Ordem de Serviço\n 2 - Consultar Ordem de Serviço\n 3 - Editar Ordem de Serviço");
            opcao = Convert.ToInt32(Console.ReadLine());
            if (opcao == 1)
            {
                int qtdLinhas = File.ReadLines(Caminho).Count();
                Console.WriteLine("Ordem de Serviço Número: " + (qtdLinhas + 1) + "!");

                bdW = File.AppendText(Caminho);
                bdW.Write(qtdLinhas+1);
                bdW.Write(",");
                Console.WriteLine("Informe o CPF/CNPJ do cliente:");
                cpf = Console.ReadLine();
                bdW.Write(cpf);
                bdW.Write(",");
                Console.WriteLine("Escolha o tipo de veiculo:\n 1 - Carro\n 2 - Moto");
                bdW.Write(tipoVeiculo[Convert.ToInt32(Console.ReadLine()) - 1]);
                bdW.Write(",");
                Console.WriteLine("Digite a Placa do Veiculo:");
                placa = Console.ReadLine();
                bdW.Write(placa);
                bdW.Write(",");
                Console.WriteLine("Faça a Descrição do Serviço: ");
                bdW.Write(Console.ReadLine());
                bdW.Write(",");
                Console.WriteLine("Defina um Status para a OS:\n 1 - {0}\n 2 - {1}\n 3 - {2}\n 4 - {3}", status[0], status[1], status[2], status[3]);
                bdW.Write(status[Convert.ToInt32(Console.ReadLine()) - 1]);
                bdW.WriteLine();
                bdW.Close();
            }else
            if(opcao == 2)
            {
                Console.WriteLine("Escolha uma das Opções:\n 1 - Consultar todas as OS's\n 2 - Filtrar por Cliente\n 3 - Filtrar por Status");
                opcao = Convert.ToInt32(Console.ReadLine());
                if (opcao == 1)
                {
                    int qtdLinhas = File.ReadLines(Caminho).Count();
                    string[,] dados = new string[qtdLinhas, qtdColunas];

                    bdR = File.OpenText(Caminho);

                    while (bdR.EndOfStream != true)
                    {
                        for(int i = 0; i < qtdLinhas; i++)
                        {
                            string[] linha = System.Text.RegularExpressions.Regex.Split(bdR.ReadLine(), ",");
                            int j = 0;
                            foreach (var element in linha) {
                                dados[i, j] = element;
                                j++;
                            }
                            j = 0;
                            
                        }
                    }

                    showDados(dados, qtdLinhas, qtdColunas);

                    bdR.Close();
                }else
                if (opcao == 2)
                {

                }else
                if(opcao == 3)
                {              
                    Console.WriteLine("Escolha o Status:\n 1 - {0}\n 2 - {1}\n 3 - {2}\n 4 - {3}", status[0], status[1], status[2], status[3]);
                    int leitura = Convert.ToInt32(Console.ReadLine());

                    int qtdLinhas = File.ReadLines(Caminho).Count();
                    string[,] dados = new string[qtdLinhas, qtdColunas];

                    bdR = File.OpenText(Caminho);

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
                            j = 0;

                        }
                    }

                    showDados(dados, qtdLinhas, qtdColunas, status[leitura-1]);

                    bdR.Close();
                }
            }

        }
    }
}
