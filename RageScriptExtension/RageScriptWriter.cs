using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

// TODO: replace this with the type you want to write out.
using TWrite = System.String;

namespace RageScriptExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    class RageScriptWriter : ContentTypeWriter<RageScript>
    {
        protected override void Write(ContentWriter output, RageScript value)
        {
            // TODO: write the specified value to the output ContentWriter.
            output.WriteRawObject<String>(value.Source);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(RageScript).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "UnderworldEngine.Scripting.RageScriptReader, UnderworldEngine";
        }
    }
}
