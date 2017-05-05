using System;
using System.Reflection.Emit;
using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions.ExceptionBlock
{
    /// <summary>
    /// Represents a catch block in a protected region
    /// </summary>
    public sealed class CatchBlock : IDisposable
    {
        private readonly XsILGenerator generator;
        private readonly Label endLabel;

        internal CatchBlock(XsILGenerator generator, Label endLabel)
        {
            this.generator = generator;
            this.endLabel = endLabel;
        }

        /// <summary>
        /// Rethrows the exception caught by this catch block
        /// </summary>
        
        public XsILGenerator Rethrow() => generator.FluentEmit(OpCodes.Rethrow);

        /// <summary>
        /// Jumps to the instruction immediately after this protected region (after any finally block executes)
        /// </summary>
        
        public XsILGenerator Leave() => generator.Leave(endLabel);
        
        /// <summary>
        /// End the catch block
        /// </summary>
        public void Dispose()
        {
        }
    }
}