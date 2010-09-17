using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnderworldEngine.Audio;

namespace UnderworldEngine.Scripting
{
    class Load : IInterpretable
    {
        Dictionary<string, ILoader> functions;
        

        public Load()
        {
            functions = new Dictionary<string, ILoader>();
            functions["sndpkg"] = new SoundLoader();
        }

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);
            try
            {
                functions[command[1]].load(command[2]);
            }
            catch
            {
                throw new ArgumentException("Option " + command[1] + " is not valid");
            }
        }
    }
}
