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
using System.Text.RegularExpressions;

namespace ControleOficina
{
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
        public client(string nome, string doc, string add,string emaill, string number)
        {
            name = nome;
            document = doc;
            address = add;
            email = emaill;
            numberFone = number;
        }

        //Funções publicas
        public string Name
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
        public static string[] returnAllAtributes(string path, string doc)
        {
            if (!doc.Contains("."))
            {
                if (doc.Length == 14)
                {
                    doc = Convert.ToUInt64(doc).ToString(@"00\.000\.000\/0000\-00");
                }
                else
                {
                    doc = Convert.ToUInt64(doc).ToString(@"000\.000\.000\-00");
                }
            }
            string[] bd = File.ReadAllLines(path);
            foreach(var element in bd)
            {
                string[] line = element.Split(",");
                if (line[1] == doc)
                {
                    return line;
                }
            }
            string[] erro = { "nd" };
            return erro;
        }
        public static void createClient(client cliente, string path, string doc, string version)
        {
            Console.Clear();
            Console.WriteLine(version);
            Console.WriteLine("-> Cadastro de Cliente\n");
            bool error = true;
            StreamWriter bdW;
            string leitura;
            Console.Write("Digite o nome: ");
            leitura = Console.ReadLine();
            leitura = leitura = Regex.Replace(leitura, "[\\,]", "");
            cliente.Name = leitura;

            if (doc.Length == 14)
            {
                cliente.Document = Convert.ToUInt64(doc).ToString(@"00\.000\.000\/0000\-00");
            }
            else
            {
                cliente.Document = Convert.ToUInt64(doc).ToString(@"000\.000\.000\-00");
            }

            Console.Write("\nDigite o Endereço (Rua, Número, Complemento, Bairro, Estado abrev.):");
            leitura = Console.ReadLine();
            leitura = leitura.Replace(",", ";");
            cliente.Address = leitura;

            error = true;
            while (error)
            {
                Console.Write("\nDigite o endereço de e-mail (Exemplo: exemplo@email.com):");
                leitura = Console.ReadLine();
                leitura = leitura.Replace(",", "");
                if (leitura.Contains("@") == false || leitura.Contains(".") == false)
                {
                    Console.WriteLine("\nE-mail inválido!");
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
                Console.Write("\nDigite o número de telefone(Com DDD): ");
                leitura = Console.ReadLine();
                leitura = leitura = Regex.Replace(leitura, "[\\(\\)\\-\\ ]", "");
                if (leitura.Length != 11)
                {
                    Console.WriteLine("\nNúmero de telefone inválido. Digite no seguinte formato: 27996342390");
                }
                else
                {
                    
                    cliente.NumberFone = Convert.ToUInt64(leitura).ToString(@"\(00\)00000\-0000");
                    error = false;
                }
            }


            bdW = File.AppendText(path);
            bdW.WriteLine(cliente.Name + "," + cliente.Document + "," + cliente.Address + "," + cliente.Email + "," + cliente.NumberFone);
            bdW.Close();
            Console.Clear();
            Console.WriteLine("\n\n     Cliente Cadastrado com Sucesso!");
            Thread.Sleep(1500);
        }

        public static bool documentInvalid(string doc)
        {
            if (doc.Length > 11)
            {
                if (doc.Length != 14)
                {
                    Console.WriteLine("CNPJ Inválido!\n");
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
                    Console.WriteLine("CPF Inválido!\n");
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
                string[] linha = bdR.ReadLine().Split(",");
                foreach (var element in linha)
                {
                    if (element == doc)
                    {
                        return true;
                    }
                }
            }
            bdR.Close();
            return false;
        }


    }

    class cpf : client
    {

    }

    class cnpj : client
    {

    }

}
