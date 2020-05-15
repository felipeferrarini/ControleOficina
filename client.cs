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
        public static void createClient(client cliente, string path, string doc)
        {
            bool error = true;
            StreamWriter bdW;
            string leitura;
            Console.WriteLine("Digite o nome:");
            cliente.Name = Console.ReadLine();

            if (doc.Length == 14)
            {
                cliente.Document = Convert.ToUInt64(doc).ToString(@"00\.000\.000\/0000\-00");
            }
            else
            {
                cliente.Document = Convert.ToUInt64(doc).ToString(@"000\.000\.000\-00");
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
                    cliente.NumberFone = Convert.ToUInt64(leitura).ToString(@"\(00\)00000\-0000");
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
