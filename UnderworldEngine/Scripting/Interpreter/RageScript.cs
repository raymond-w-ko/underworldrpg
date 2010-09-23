using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnderworldEngine.Scripting
{
    class RageScript
    {
        string source;
        public MemoryStream stream;

        public RageScript(string source)
        {
            this.source = source;
            process();
        }

        private void process()
        {
            stream = new MemoryStream(Encoding.ASCII.GetBytes(source));
        }

        
        public void Unload()
        {
            this.stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
