using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

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
            set
            {
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
        public static void createOS(string pathOs, os novaOS, string leitura, string[] tipoVeiculo, string[] status)
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
}
