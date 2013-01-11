using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using System.Text.RegularExpressions;

namespace MiniHttp.Processors
{
    public class TemplateProcessor : Processor
    {
        private static readonly Regex ProcessorDirectiveExp = new Regex(@"^\s*@(\w+)\(([^)]*)\)\s*$");

        protected override IProcessingResult ProcessLine(Line line)
        {
            var match = ProcessorDirectiveExp.Match(line.Value);
            if (match.Success)
            {
                var command = match.Groups[1].Value;
                var argument = match.Groups[2].Value;

                switch (command)
                {
                    case "template":
                    case "import":
                        return Insert(line.CreateSource(argument));
                    case "content":
                        return Resume();
                }
            }

            return Identity();
        }
    }
}
