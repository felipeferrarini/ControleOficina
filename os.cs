using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;

namespace ControleOficina
{
    class os
    {
        //Atributos
        protected int numero;
        protected string cliente;
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
        // Construtor com parametros para edição e consulta de OS
        public os(int name, string cli, string tipo, string placa, DateTime inicio, DateTime prev, DateTime fim, string stat, string descript)
        {
            numero = name;
            cliente = cli;
            type = tipo;
            plate = placa;
            createdAt = inicio;
            doneIn = prev;
            doneAt = fim;
            status = stat;
            description = descript;
        }

        //Funções publicas
        public int getNumero
        {
            get { return numero; }
        }
        public string Client
        {
            get { return cliente; }
            set
            {
                if (value.Length == 14)
                {
                    cliente = value.Substring(0, 2) + "." + value.Substring(2, 3) + "." + value.Substring(5, 3) + "/" + value.Substring(8, 4) + "-" + value.Substring(12, 2);
                }
                else
                {
                    cliente = value.Substring(0, 3) + "." + value.Substring(3, 3) + "." + value.Substring(6, 3) + "-" + value.Substring(9, 2);
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
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
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
        public static string[] returnAllAtributes(os os)
        {
            string[] dados = new string[9];
            dados[0] = Convert.ToString(os.getNumero);
            dados[1] = os.cliente;
            dados[2] = os.Type;
            dados[3] = os.Plate;
            dados[4] = Convert.ToString(os.CreatedAt);
            dados[5] = Convert.ToString(os.getDoneIn);
            dados[6] = Convert.ToString(os.DoneAt);
            dados[7] = os.Status;
            dados[8] = os.Description;

            return dados;
        }
        public static string[] returnAllAtributes(string path,string id)
        {
            string[] bd = File.ReadAllLines(path);
            foreach(var element in bd)
            {
                string[] line = element.Split(",");
                if (line[0] == id)
                {
                    return line;
                }
            }
            string[] erro = { "nd", "0", "0", "0", "0", "0", "0", "0" };
            return erro;
        }
        public static void createOS(string pathOs, os novaOS, string cliente, string[] tipoVeiculo, string[] status)
        {
            bool error = true;
            string leitura;
            novaOS.Client = cliente;
            Console.WriteLine("Escolha o tipo de veiculo:\n 1 - Carro\n 2 - Moto");
            while (error)
            {
                leitura = Console.ReadLine();
                if (leitura == "1" || leitura == "2")
                {
                    error = false;
                    novaOS.Type = tipoVeiculo[Convert.ToInt32(leitura) - 1];
                }
                else
                {
                    error = true;
                    Console.WriteLine("Opção inválida!");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Digite a placa do Veiculo:");
            novaOS.Plate = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("Digite quantos dias irá durar o serviço (Somente uma Previsão):");
            novaOS.setDoneIn = Convert.ToInt32(Console.ReadLine());
            error = true;
            Console.WriteLine("");
            Console.WriteLine("Defina um Status para a OS:\n 1 - {0}\n 2 - {1}\n 3 - {2}\n 4 - {3}", status[0], status[1], status[2], status[3]);
            while (error)
            {
                leitura = Console.ReadLine();
                if (leitura == "1" || leitura == "2" || leitura == "3" || leitura == "4")
                {
                    error = false;
                    novaOS.Status = status[Convert.ToInt32(leitura) - 1];
                }
                else
                {
                    error = true;
                    Console.WriteLine("Opção inválida!");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Faça a Descrição do Serviço: ");
            leitura = Console.ReadLine();
            leitura.Replace(",", ";");
            novaOS.Description = leitura;

            StreamWriter bdW;
            bdW = File.AppendText(pathOs);
            bdW.WriteLine(string.Join(",",os.returnAllAtributes(novaOS)));
            bdW.Close();
            Console.Clear();
            Console.WriteLine("\n\n     Ordem de Serviço Criada!");
            Thread.Sleep(1000);
        }
        public static int getNextNumber(string path)
        {
            int nextNumber = File.ReadLines(path).Count();
            nextNumber++;
            return nextNumber;
        }

        public static void editOS(string path, string id, string[] status, string version)
        {
            bool error = true;
            string[] bd = File.ReadAllLines(path);
            foreach(var element in bd)
            {
                string[] line = element.Split(",");
                if (line[0].Contains(id))
                {
                    error = false;
                }
            }
            if (error == true)
            {
                Console.WriteLine("Ordem de Serviço não encontrada!");
            }
            else
            {
                bool edit = true;
                string option, leitura;
                string[] line = bd[Convert.ToInt32(id) - 1].Split(",");
                os osEdit = new os(Convert.ToInt32(line[0]), line[1], line[2], line[3], DateTime.Parse(line[4]), DateTime.Parse(line[5]), DateTime.Parse(line[6]), line[7], line[8]);
                while (edit)
                {
                    Console.Clear();
                    Console.WriteLine(version);
                    Console.WriteLine("-> Editar Ordem de Serviço\n");
                    Console.WriteLine("Dados da OS:");
                    Console.WriteLine("    Número: {0}", osEdit.getNumero);
                    Console.WriteLine("1 - Documento do Cliente: {0}", osEdit.cliente);
                    Console.WriteLine("2 - Tipo de Veiculo: {0}", osEdit.Type);
                    Console.WriteLine("3 - Placa: {0}", osEdit.Plate);
                    Console.WriteLine("4 - Início do Serviço: {0}", osEdit.CreatedAt);
                    Console.WriteLine("5 - Previsão de Conclusão: {0}", osEdit.getDoneIn);
                    Console.WriteLine("6 - Fim Real do Serviço: {0}", osEdit.DoneAt);
                    Console.WriteLine("7 - Status: {0}", osEdit.Status);
                    Console.WriteLine("8 - Descrição completa: {0}", osEdit.Description);
                    Console.WriteLine("Selecione o item que deseja editar (exceto o número) ou 0 para finalizar edição:");
                    option = Console.ReadLine();
                    switch (option)
                    {
                        case "0":
                            Console.WriteLine("Deseja salvar as alterações?\n 1 - sim\n 2 - não");
                            option = Console.ReadLine();
                            while (edit)
                            {
                                if (option == "1")
                                {
                                    bd[osEdit.getNumero-1] = String.Join(",", os.returnAllAtributes(osEdit));
                                    File.WriteAllLines(path, bd);
                                    Console.WriteLine("Alterações salvas com sucesso!");
                                    edit = false;
                                }
                                else if (option == "2")
                                {
                                    edit = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Opção inválida!");
                                }
                            }
                            break;

                        case "1":
                            error = true;
                            Console.WriteLine("Valor Anterior: {0}", osEdit.Client);
                            Console.WriteLine("Digite o novo valor (Somente números):");
                            while (error)
                            {
                                leitura = Console.ReadLine();
                                if (leitura.Length == 7 || leitura.Length == 14)
                                {
                                    osEdit.Client = leitura;
                                    error = false;
                                }
                                else
                                {
                                    Console.WriteLine("Digite um CPF ou CNPJ Válido! (Somente números)");
                                    error = true;
                                }
                            }
                            break;

                        case "2":
                            Console.WriteLine("Valor Anterior: {0}", osEdit.Type);
                            Console.WriteLine("Digite o novo valor (Somente números):");
                            leitura = Console.ReadLine();
                            osEdit.Type = leitura;
                            break;
                        case "3":
                            Console.WriteLine("Valor Anterior: {0}", osEdit.Plate);
                            Console.WriteLine("Digite o novo valor:");
                            leitura = Console.ReadLine();
                            osEdit.Plate = leitura;
                            break;
                        case "4":
                            Console.WriteLine("Valor Anterior: {0}", osEdit.CreatedAt);
                            Console.WriteLine("Digite o novo valor (Somente números):");
                            leitura = Console.ReadLine();
                            osEdit.CreatedAt = DateTime.Parse(leitura);
                            break;
                        case "5":
                            error = true;
                            int i;
                            Console.WriteLine("Valor Anterior: {0}", osEdit.getDoneIn);
                            Console.WriteLine("Digite quantos dias deseja adionar a partir de hoje (Somente número):");
                            while (error)
                            {
                                leitura = Console.ReadLine();
                                if (int.TryParse(leitura, out i))
                                {
                                    osEdit.setDoneIn = i;
                                    error = false;
                                }
                                else
                                {
                                    Console.WriteLine("Digite um valor válido! (Número de dias que deseja adicionar a partir de hoje)");
                                    error = true;
                                }
                            }                          
                            break;
                        case "6":
                            error = true;
                            Console.WriteLine("Valor Anterior: {0}", osEdit.DoneAt);
                            Console.WriteLine("Digite a data de Encerramento da OS (Formato: 00/00/000):");
                            DateTime leitura2;
                            while (error)
                            {
                                leitura = Console.ReadLine();
                                if (DateTime.TryParse(leitura, out leitura2))
                                {
                                    osEdit.DoneAt = leitura2;
                                    osEdit.Status = status[3];
                                    error = false;
                                }
                                else
                                {
                                    Console.WriteLine("Digite uma data válida! (Fomato: 00/00/0000");
                                    error = true;
                                }
                            }
                            break;
                        case "7":
                            error = true;
                            while (error)
                            {
                                Console.WriteLine("Valor Anterior: {0}", osEdit.Status);
                                Console.WriteLine("Escolha um novo status para a OS:" +
                                    "\n 1 - {0}" +
                                    "\n 2 - {1}" +
                                    "\n 3 - {2}" +
                                    "\n 4 - {3}", status[0], status[1], status[2], status[3]);
                                leitura = Console.ReadLine();
                                if (leitura == "1" || leitura == "2" || leitura == "3" || leitura == "4")
                                {
                                    if (leitura == "4"){
                                        osEdit.Status = status[Convert.ToInt32(leitura) - 1];
                                        osEdit.DoneAt = DateTime.Now;
                                        error = false;
                                    }
                                    else
                                    {
                                        osEdit.Status = status[Convert.ToInt32(leitura) - 1];
                                        error = false;
                                    }
                                    
                                }
                                else
                                {
                                    Console.WriteLine("Opção inválida!");
                                    error = true;
                                }
                            }
                            break;
                        case "8":
                            Console.WriteLine("Valor Anterior: {0}", osEdit.Description);
                            Console.WriteLine("Digite a nova descrição:");
                            leitura = Console.ReadLine();
                            osEdit.Description = leitura;
                            break;

                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }
                }
            }
        }
    }
}
