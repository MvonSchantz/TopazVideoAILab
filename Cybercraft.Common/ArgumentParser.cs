using System.Collections.Generic;

namespace Cybercraft.Common
{
    public abstract class ArgumentParser
    {
        private readonly string[] args;

        private readonly Dictionary<string, int> optionSpec = new Dictionary<string, int>();

        private readonly List<string> arguments = new List<string>();
        private readonly Dictionary<string, string[]> options = new Dictionary<string, string[]>();

        private bool isParsed;

        public void AddOption(string name, int numberOfArguments = 0)
        {
            optionSpec.Add(name, numberOfArguments);
        }

        public string[] Arguments
        {
            get
            {
                if (!isParsed)
                {
                    Parse();
                }
                return arguments.ToArray();
            }
        }

        public IReadOnlyDictionary<string, string[]> Options
        {
            get
            {
                if (!isParsed)
                {
                    Parse();
                }
                return options;
            }
        }

        protected bool ForceShowHelp { get; set; }

        public bool ShowHelp => Options.ContainsKey("help") || Options.ContainsKey("?") || ForceShowHelp;

        protected ArgumentParser(string[] args)
        {
            AddOption("help");
            AddOption("?");
            this.args = args;
        }

        private void Parse()
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                string orgArg = arg;
                if (arg.Length > 1 && (arg[0] == '/' || arg[0] == '-'))
                {
                    arg = arg.Substring(1);
                    if (arg.Length > 1 && arg[0] == '-')
                    {
                        arg = arg.Substring(1);
                    }
                    int numberOfOptionArguments;
                    if (!optionSpec.TryGetValue(arg, out numberOfOptionArguments))
                    {
                        ForceShowHelp = true;
                        arguments.Add(orgArg);
                        continue;
                    }
                    var optionArguments = new List<string>();
                    for (int j = 0; j < numberOfOptionArguments && j + i + 1 < args.Length; j++)
                    {
                        optionArguments.Add(args[j + i + 1]);
                    }
                    options.Add(arg, optionArguments.ToArray());
                    i += numberOfOptionArguments;
                }
                else
                {
                    arguments.Add(arg);
                }
            }
            isParsed = true;
        }
    }
}



