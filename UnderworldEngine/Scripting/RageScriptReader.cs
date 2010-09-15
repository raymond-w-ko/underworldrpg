using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// TODO: replace this with the type you want to read.
using TRead = System.String;

namespace UnderworldEngine.Scripting
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content
    /// Pipeline to read the specified data type from binary .xnb format.
    /// 
    /// Unlike the other Content Pipeline support classes, this should
    /// be a part of your main game project, and not the Content Pipeline
    /// Extension Library project.
    /// </summary>
    class RageScriptReader : ContentTypeReader<RageScript>
    {
        protected override RageScript Read(ContentReader input, RageScript existingInstance)
        {
            // TODO: read a value from the input ContentReader.
            int length = input.ReadInt32();
            input.ReadChars(2);
            string source = new String(input.ReadChars(length));

            return new RageScript(source);
        }
    }
}
