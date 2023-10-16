using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;

namespace SomeBlog.FileHashesGenerator
{   
    class Program
    {
        static void Main(string[] args)
        {
            //1- zip dosyasını al, aç
            //2- içerisinde gez ve buldugn her dosyas için name:hash olarak isimlendir BouncyCastle.Crypto.dll:E2D7D9A3850CD3CF0C77C5A285D61CBC
            if(System.IO.Directory.Exists("lobby"))
                System.IO.Directory.Delete("lobby", true);

            System.IO.Directory.CreateDirectory("lobby");

            var fileName = "_publish-ONEUI-20230813-2115";

            ZipFile.ExtractToDirectory($"{fileName}.zip", "lobby");
            var list = CheckHashAndUploadFtp("lobby");

            File.WriteAllLines($"{fileName}.txt", list);

            System.IO.Directory.Delete("lobby", true);

            Console.Write("done");
            Console.Read();
        }

        static List<string> CheckHashAndUploadFtp(string dirPath)
        {
            string[] files = Directory.GetFiles(dirPath, "*.*");
            string[] subDirs = Directory.GetDirectories(dirPath);

            var local_hashes = new List<string>();

            foreach (string file in files)
            {
                var fileName = (dirPath.Replace("lobby", "")
                    .TrimStart('\\') + "/" + Path.GetFileName(file))
                    .TrimStart('/');

                var current_hash = CurrentHash(file);
                local_hashes.Add(fileName + ":" + current_hash);
            }

            foreach (string subDir in subDirs)
            {
                local_hashes.AddRange(CheckHashAndUploadFtp(subDir));
            }

            return local_hashes;
        }

        static string CurrentHash(string fileName)
        {
            string md5hash = "";
            long fileSize = new FileInfo(fileName).Length;
            int bufferSize = fileSize > 1048576L ? 1048576 : 4096;

            using (MD5 md = MD5.Create())
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
                {
                    md5hash = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "");
                }
            }

            return md5hash;
        }
    }
}
