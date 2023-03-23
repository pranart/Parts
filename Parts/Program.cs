using System;
using CommandLine;
using System.IO;

namespace Parts
{
	class MainClass
	{
		static byte[] buffer = new byte[1000000000];
		public static void Main(string[] args)
		{
			Options options = new Options();
			// l == length
			// s == split
			// m == merge
			// major 3
			if(CommandLine.Parser.Default.ParseArguments(args,options))
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(options.BigFile) && options.SplitSize > 0)
					{
						if (options.SplitSize > 1000000000)
						{
							options.SplitSize = 1000000000;
						}
						Console.WriteLine("Splitting at {0} bytes", options.SplitSize);
						Split(options.BigFile, options.SplitSize);
					}
					else if (!string.IsNullOrWhiteSpace(options.FinalFile))
					{
						Console.WriteLine("Merging File {0}", options.FinalFile);
						Merge(options.FinalFile);
					}
					else
					{
					}
				}
				catch (Exception exc)
				{
					Console.WriteLine(exc.ToString());
					Console.ReadKey();
				}
			}
		}
		static void Split(string bigFileName, long partSize)
		{
			using (FileStream SourceStream = new FileStream(bigFileName, FileMode.Open))
			{
				long bigSize = SourceStream.Length;

				long position = 0;
				int partCount = 0;

				long left = bigSize - position;

				while (left > partSize)
				{
					WriteAPart(SourceStream, bigFileName, partCount, position,partSize);

					position += partSize;
					partCount++;
					left = bigSize - position;
				}
				WriteAPart(SourceStream, bigFileName, partCount, position, left);

				SourceStream.Close();
				Console.WriteLine("Splitting Process succeed");
			}
		}
		static void WriteAPart(FileStream sourceStream, string bigFilename, long partCount, long position, long partSize)
		{
			string destinationFilename = bigFilename + "." + partCount.ToString();

			using (FileStream DestinationStream = new FileStream(destinationFilename,FileMode.OpenOrCreate))
			{
				Console.WriteLine("Writing {0}", destinationFilename);
				sourceStream.Read(buffer,0,(int)partSize);
				DestinationStream.Write(buffer, 0, (int)partSize);
				DestinationStream.Close();
				Console.WriteLine("Write {0} successfully", destinationFilename);
			}
		}
		static void Merge(string FinalFilename)
		{
			int i = 0;
			using (FileStream FinalFile = new FileStream(FinalFilename, FileMode.OpenOrCreate))
			{
				while (true)
				{
					string partFilename = FinalFilename + "." + i.ToString();
					try
					{
						using (FileStream eachFile = new FileStream(partFilename, FileMode.Open))
						{
							Console.WriteLine("Merging File {0}", partFilename);
							eachFile.Read(buffer,0, (int)eachFile.Length);
							FinalFile.Write(buffer, 0, (int)eachFile.Length);
							eachFile.Close();
							Console.WriteLine("Merge {0} Success", partFilename);
						}
					}
					catch (Exception)
					{
						FinalFile.Close();
						Console.WriteLine("Merge all successfully");
						return;
					}
					i++;
				}
			}
		}
	}
}
