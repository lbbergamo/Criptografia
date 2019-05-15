using System;

namespace Criptografia_AES
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Informe o texto/senha a ser criptografado: ");
            String valor = Console.ReadLine();
            String valorCriptografado = Criptografia.Criptografar(valor);
            String valorDescriptografado = Criptografia.Descriptografar(valorCriptografado);
            Console.Write("Texto para ser criptografado: " + valor);//Novo valor
            Console.Write("Texto criptografado: " + valorCriptografado); //jM3IT3OZEy+1ha5XNL3Wfg==
            Console.Write("Texto descriptografado: " + valorDescriptografado);//Novo valor
            Console.ReadLine();
        }
    }
}
