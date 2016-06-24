using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrpParser
{
    public class Options
    {
        [Option('f', "file", Required = true,
          HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('v', "verbose", DefaultValue = false,
          HelpText = "Give verbose output of parsed values.")]
        public bool Verbose { get; set; }

        [Option('s', "savecontent",
          HelpText = "Save parsed values")]
        public bool SaveFiles { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
