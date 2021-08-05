using CrpParser;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(String[] args)
        {
            // using a container would make this easier
            var assetParser = AssetParser.MakeDefault();
            var crpDeserializer = new CrpDeserializer(assetParser);


            var rootCommand = new RootCommand
            {
                new Option<FileInfo>(
                    new String[] { "-f", "--file" },
                    "Input file to be processed.") { IsRequired = true },
                new Option<Boolean>(
                    new String[] { "-v", "--verbose" },
                    getDefaultValue: () => false,
                    description: "Give verbose output of parsed values."),
                new Option<Boolean>(
                    new String[] {"-s", "--savecontent"},
                    "Save parsed values."),

            };

            rootCommand.Description = "Parse a crp file";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create((FileInfo file, Boolean verbose, Boolean savecontent) => crpDeserializer.ParseFile(file, savecontent, verbose));

            // Parse the incoming args and invoke the handler
            rootCommand.Invoke(args);
           
        }
    }
}
