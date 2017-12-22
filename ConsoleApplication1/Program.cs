using CommandLine;
using CrpParser;

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

                CrpDeserializer deserializer = new CrpDeserializer(options.InputFile);
                deserializer.parseFile(options);
            }

            else if (Parser.Default.ParseArguments(args, options))
            {
                CrpDeserializer deserializer = new CrpDeserializer(options.InputFile);
                deserializer.parseFile(options);
            }

//#endif

        }
    }
}
