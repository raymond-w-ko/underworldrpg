using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace RageScriptExtension
{
    class RageScript
    {
        private string source;
        public string Source
        {
            get
            {
                return source;
            }
        }

        public RageScript(string source)
        {
            this.source = source;
        }
    }
}
