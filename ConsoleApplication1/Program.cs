using CommandLine;
using CrpParser;
using System;
using System.IO;

namespace ConsoleApplication1
{
	class Program
    {
        static void Main(string[] args)
        {
			var options = new Options();
			/*#if DEBUG
						options.Verbose = false;
						options.SaveFiles = true;
						CrpDeserializer deserializer = new CrpDeserializer("C:\\Program Files (x86)\\Steam\\steamapps\\workshop\\content\\255710\\721098648\\San Minato LUT V1.3.crp");
						 deserializer.parseFile(options);
			#else*/
			if (args.Length == 1)
            {
                options.InputFile = args[0];
                options.Verbose = false;
                options.SaveFiles = true;

				try
				{
					CrpDeserializer deserializer = new CrpDeserializer(options.InputFile);
					deserializer.parseFile(options);
				}
				catch (IOException exception)
				{
					HardErrorToConsole(exception);
				}
            }

            else if (Parser.Default.ParseArguments(args, options))
            {
				try
				{
					CrpDeserializer deserializer = new CrpDeserializer(options.InputFile);
					deserializer.parseFile(options);
				}
				catch (IOException exception)
				{
					HardErrorToConsole(exception);
				}
			}
			//#endif
		}

		/// <summary>
		/// Alerts the user to a fatal runtime error and terminates the program.
		/// </summary>
		/// <param name="exception">An Exception object containing additional information about the error.</param>
		static void HardErrorToConsole(Exception exception)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("Error: ");
			Console.ResetColor();
			Console.WriteLine(exception.Message);
			Console.ReadKey();
			Environment.Exit(-1);
		}
    }
}
