using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;

namespace ConsoleApplication1
{
    class Options
    {
        [Option('f', "file", Required = true,
          HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo(Assembly.GetExecutingAssembly().GetName().Name),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine(String.Format("Usage: {0}.exe -f FileToRead", Assembly.GetExecutingAssembly().GetName().Name));
            help.AddOptions(this);
            return help;
        }
    }
    

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                CrpDeserializer deserializer = new CrpDeserializer(options.InputFile);
                deserializer.parseFile();
            }
        }
    }
}
