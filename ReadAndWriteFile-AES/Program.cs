using System;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using ReadAndWriteFile_AES;


namespace ReadAndWriteFile_AES
{
    class Program
    {

        static void Main(string[] args)
        {

            string password = "ThePasswordToDecryptAndEncryptTheFile";
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            Console.WriteLine("Informe o local do arquivo: ");
            string inputFile = Console.ReadLine();
            Console.WriteLine("Deseja:" +
                "\n1 para criptografar" +
                "\n2 para descriptografar " +
                "\nDigite:");
            int funcao = int.Parse(Console.ReadLine());
            if (funcao == 1)
            {
                byte[] salt = criptografando.GenerateRandomSalt();
                FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);
                RijndaelManaged AES = new RijndaelManaged();
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Padding = PaddingMode.PKCS7;
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                fsCrypt.Write(salt, 0, salt.Length);
                CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                byte[] buffer = new byte[1048576];
                int read;
                try
                {
                    while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cs.Write(buffer, 0, read);
                    }

                    // Close up
                    fsIn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    cs.Close();
                    fsCrypt.Close();
                }
                Console.WriteLine("The given password is surely nothing: " + password);
                Console.WriteLine("Arquivo criptografado com sucesso! ");
            }
            else if (funcao == 2)
            {
                Console.WriteLine("Deseja descriptografar ? S/N");
                string validacao = Console.ReadLine().ToUpper();
                while (validacao == "S")
                {
                    byte[] salt = new byte[32];
                    string outPutFile = "1" + inputFile;
                    FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Open);
                    fsCrypt.Read(salt, 0, salt.Length);

                    RijndaelManaged AES = new RijndaelManaged();
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.PKCS7;

                    CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

                    FileStream fsOut = new FileStream(outPutFile, FileMode.Create);
                    byte[] buffer = new byte[1048576];
                    int read;


                    try
                    {
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fsOut.Write(buffer, 0, read);
                        }
                    }
                    catch (CryptographicException ex_CryptographicException)
                    {
                        Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                    try
                    {
                        cs.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
                    }
                    finally
                    {
                        fsOut.Close();
                        fsCrypt.Close();
                    }
                }
            }
            else
            {
                Console.WriteLine("OK");
            }
            Console.WriteLine("Pronto");
            Console.ReadKey();
        }

    }

}



