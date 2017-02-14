using System;
using CommandLine;
namespace Parts
{
	public class Options
	{

		[Option('s', "split", Required = false, HelpText = "Input file to split")]
		public string BigFile { get; set; }

		[Option('l', "length", Required = false, HelpText = "Size of splitted part")]
		public long SplitSize { get; set; }

		[Option('m',"merge",Required = false, HelpText = "Output file to merge")]
		public string FinalFile { get; set; }

			
	}
}
