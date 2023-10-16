using System;
using System.IO;
using System.Security.Cryptography;

namespace SomeBlog.Infrastructure
{
	public class HashHelper
	{
		public string CurrentHash(string fileName)
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

		public string ChangeHash(string fileName)
		{
			Random random = new Random();

			int num = random.Next(2, 7);
			byte[] extraByte = new byte[num];
			for (int j = 0; j < num; j++)
			{
				extraByte[j] = (byte)0;
			}
			long fileSize = new FileInfo(fileName).Length;

			using (FileStream fileStream = new FileStream(fileName, FileMode.Append))
			{
				fileStream.Write(extraByte, 0, extraByte.Length);
			}

			int bufferSize = fileSize > 1048576L ? 1048576 : 4096;
			string md5hash = "";
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream2 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
				{
					md5hash = BitConverter.ToString(md.ComputeHash(fileStream2)).Replace("-", "");
				}
			}

			return md5hash;
		}
	}
}
