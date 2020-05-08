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

    class os
    {
        //Atributos
        protected int numero;
        protected string client;
        protected string type;
        protected string plate;
        protected DateTime createdAt;
        protected DateTime doneIn;
        protected DateTime doneAt;
        protected string status;
        protected string description;

        //Contrutor vazio (Para criação de nova OS)
        public os(string path)
        {
            numero = getNextNumber(path);
            createdAt = DateTime.Now;
            doneIn = DateTime.Parse("01,01,1900");
            doneAt = DateTime.Parse("01,01,1900");
            description = "Sem dados";
        }

        //Funções publicas
        public int getNumero
        {
            get { return numero; }
        }
        public string Client
        {
            get { return client; }
            set {
                if (value.Length == 14)
                {
                    client = value.Substring(0, 2) + "." + value.Substring(2, 3) + "." + value.Substring(5, 3) + "/" + value.Substring(8, 4) + "-" + value.Substring(12, 2);
                }
                else
                {
                    client = value.Substring(0, 3) + "." + value.Substring(3, 3) + "." + value.Substring(6, 3) + "-" + value.Substring(9, 2);
                }
            }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Plate
        {
            get { return plate; }
            set { plate = value; }
        }
        public DateTime getCreatedAt
        {
            get { return createdAt; }
        }
        public DateTime getDoneIn
        {
            get { return doneIn; } 
        }
        public int setDoneIn
        {
            set { doneIn = DateTime.Now.AddDays(value); }
        }
        public DateTime DoneAt
        {
            get { return doneAt; }
            set { doneAt = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        //Funções Staticas
        public static void createOS( string pathOs,os novaOS, string leitura, string[] tipoVeiculo, string[] status)
        {
            novaOS.Client = leitura;
            Console.WriteLine("Escolha o tipo de veiculo:\n 1 - Carro\n 2 - Moto");
            novaOS.Type = tipoVeiculo[Convert.ToInt32(Console.ReadLine()) - 1];
            Console.WriteLine("Digite a placa do Veiculo (Semente letras e números):");
            novaOS.Plate = Console.ReadLine();
            Console.WriteLine("Digite quantos dias irá durar (Previsão. Digite somente número):");
            novaOS.setDoneIn = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Defina um Status para a OS:\n 1 - {0}\n 2 - {1}\n 3 - {2}\n 4 - {3}", status[0], status[1], status[2], status[3]);
            novaOS.Status = status[Convert.ToInt32(Console.ReadLine()) - 1];
            Console.WriteLine("Faça a Descrição do Serviço: ");
            novaOS.Description = Console.ReadLine();

            StreamWriter bdW;
            bdW = File.AppendText(pathOs);
            bdW.Write(novaOS.getNumero + "," + novaOS.Client + "," + novaOS.Type + "," + novaOS.Plate + "," + novaOS.getCreatedAt + "," + novaOS.getDoneIn);
            bdW.WriteLine("," + novaOS.DoneAt + "," + novaOS.Status + "," + novaOS.Description);
            bdW.Close();
        }
        public static int getNextNumber(string path)
        {
            int nextNumber = File.ReadLines(path).Count();
            nextNumber++;
            return nextNumber;
        }
    }

    class client
    {
        //atributos
        protected string name;
        protected string document;
        protected string address;
        protected string email;
        protected string numberFone;

        //Contrutor vazio
        public client()
        {
            name = "nome";
            document = "000.000.000-00";
            address = "Endereço";
            email = "example@email.com";
            numberFone = "(27) 99999-9999";
        }

        //Funções publicas
        public string  Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Document
        {
            get { return document; }
            set { document = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string NumberFone
        {
            get { return numberFone; }
            set { numberFone = value; }
        }

        ////Funções Staticas
        public static void createClient(client cliente, string path, string doc)
        {
            bool error = true;
            StreamWriter bdW;
            string leitura;
            Console.WriteLine("Digite o nome:");
            cliente.Name = Console.ReadLine();
            
            if (doc.Length == 14)
            {
                cliente.Document = doc.Substring(0, 2) + "." + doc.Substring(2, 3) + "." + doc.Substring(5, 3) + "/" + doc.Substring(8, 4) + "-" + doc.Substring(12, 2);
            }
            else
            {
                cliente.Document = doc.Substring(0, 3) + "." + doc.Substring(3, 3) + "." + doc.Substring(6, 3) + "-" + doc.Substring(9, 2);
            }

            Console.WriteLine("Digite o Endereço (Rua, Número, Complemento, Bairro, Estado abrev.):");
            cliente.Address = Console.ReadLine();

            error = true;
            while (error)
            {
                Console.WriteLine("Digite o endereço de e-mail (Exemplo: exemplo@email.com):");
                leitura = Console.ReadLine();
                if (leitura.Contains("@") != true)
                {
                    Console.WriteLine("E-mail inválido!");
                }
                else
                {
                    cliente.Email = leitura;
                    error = false;
                }
            }

            error = true;
            while (error)
            {
                Console.WriteLine("Digite o número de telefone(Somente os números com DDD):");
                leitura = Console.ReadLine();
                if (leitura.Length != 11)
                {
                    Console.WriteLine("Número de telefone inválido. Digite no seguinte formato: 27996342390");
                }
                else
                {
                    cliente.NumberFone = "(" + leitura.Substring(0, 2) + ")" + leitura.Substring(2, 5) + "-" + leitura.Substring(7, 4);
                    error = false;
                }
            }
            

            bdW = File.AppendText(path);
            bdW.WriteLine(cliente.Name + "," + cliente.Document + "," + cliente.Address + "," + cliente.Email + "," + cliente.NumberFone);
            bdW.Close();
        }

        public static bool documentInvalid(string doc)
        {
            if (doc.Length > 11)
            {
                if (doc.Length != 14)
                {
                    Console.WriteLine("CNPJ Inválido");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (doc.Length != 11)
                {
                    Console.WriteLine("CPF Inválido");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool documentExist(string doc, string path)
        {
            if (doc.Length == 14)
            {
                doc = doc.Substring(0, 2) + "." + doc.Substring(2, 3) + "." + doc.Substring(5, 3) + "/" + doc.Substring(8, 4) + "-" + doc.Substring(12, 2);
            }
            else
            {
                doc = doc.Substring(0, 3) + "." + doc.Substring(3, 3) + "." + doc.Substring(6, 3) + "-" + doc.Substring(9, 2);
            }
            StreamReader bdR;
            bdR = File.OpenText(path);

            while (bdR.EndOfStream != true)
            {
                string[] linha = System.Text.RegularExpressions.Regex.Split(bdR.ReadLine(), ",");
                foreach (var element in linha)
                {
                    if (element==doc)
                    {
                        return true;
                    }
                }
            }
            bdR.Close();
            return false;
        }


    }
    class Program
    {
        public static void showDados(string[,] dados, int qtdLinhas, int qtdColunas)
        {
            Console.WriteLine(dados[0, 0]);
            Console.WriteLine("Número         |CPF/CNPJ       |Veiculo        |Placa          |Descrição      |Status         ");
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
            bool menu = true;
            StreamWriter bdW;
            StreamReader bdR;
            string pathOs = Directory.GetCurrentDirectory() + "\\bases\\baseOS.txt";
            string pathClient = Directory.GetCurrentDirectory() + "\\bases\\baseClientes.txt";
            int qtdColunas = 9;
            string opcao;
            
            string[] tipoVeiculo = new string[2] { "Carro", "Moto" };
            string[] status = new string[4] { "Ag. Peças", "Ag. Execução", "Em Execução", "Finalizada" };

            while (menu)
            {
                Console.WriteLine("Escolha uma das Opções:\n 1 - Criar nova Ordem de Serviço\n 2 - Consultar Ordem de Serviço\n 3 - Editar Ordem de Serviço\n 4 - Cadastrar Cliente\n 5 - Sair");
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
                    Console.WriteLine("Escolha uma das Opções:\n 1 - Consultar todas as OS's\n 2 - Filtrar por Cliente\n 3 - Filtrar por Status");
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
                                j = 0;

                            }
                        }

                        showDados(dados, qtdLinhas, qtdColunas);

                        bdR.Close();
                    }
                    else
                    if (opcao == "2")
                    {

                    }
                    else
                    if (opcao == "3")
                    {
                        Console.WriteLine("Escolha o Status:\n 1 - {0}\n 2 - {1}\n 3 - {2}\n 4 - {3}", status[0], status[1], status[2], status[3]);
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
