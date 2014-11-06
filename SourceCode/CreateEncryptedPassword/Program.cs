using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CreateEncryptedPassword
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input password...");
            string password = Console.ReadLine();

            string encryptedPassword = EncryptPassword(password);

            Console.WriteLine("Encrypted password is : " + encryptedPassword);
            Console.WriteLine("Press any key to save");
            Console.ReadLine();

            string filePath = Environment.CurrentDirectory + @"\password.txt";

            StreamWriter f = new StreamWriter(filePath, false);
            f.WriteLine(encryptedPassword);
            f.Close();
            Console.WriteLine("Save successfully");
            Console.ReadKey();
        }

        private static string EncryptPassword(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //将要加密的字符串转换为字节数组
            byte[] palindata = Encoding.Default.GetBytes(password);
            //将字符串加密后也转换为字符数组
            byte[] encryptdata = md5.ComputeHash(palindata);
            //将加密后的字节数组转换为加密字符串
            return Convert.ToBase64String(encryptdata);
        }

    }
}
