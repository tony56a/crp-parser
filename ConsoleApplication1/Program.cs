using CommandLine;
using CommandLine.Text;
using CrpParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;

namespace ConsoleApplication1
{

    

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
#if DEBUG
            
             CrpDeserializer deserializer = new CrpDeserializer("D:\\Dropbox\\dropbox\\csfileformat\\uk_sign.crp");
             deserializer.parseFile();
#else
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

#endif

        }
    }
}
