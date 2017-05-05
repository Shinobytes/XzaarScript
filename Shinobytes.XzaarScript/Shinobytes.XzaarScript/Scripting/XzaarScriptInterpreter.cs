/*******************************************************************\
* Copyright (c) 2016 Solidicon AB (556695-1249), Gothenburg, Sweden.*
* Any usage of the content of this file, in part or whole, without  *
* a written agreement from Solidicon AB, will be considered a       *
* violation against international copyright law.                    *
\*******************************************************************/

using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Scripting.Compilers;
using Shinobytes.XzaarScript.Scripting.Compilers.Linq;

namespace Shinobytes.XzaarScript.Scripting
{
    public class XzaarScriptInterpreter : XzaarScriptInterpreterBase
    {
        public XzaarScriptInterpreter(IXzaarScriptCompiler compiler, params KeyValuePair<string, Type>[] parameters)
            : base(
                  new XzaarScriptLexer(),
                  new XzaarScriptParser(),
                  new XzaarScriptTransformer(),
                  compiler)
        {
            RegisterVariables(parameters);
        }

        public XzaarScriptInterpreter(params KeyValuePair<string, Type>[] parameters)
            : base(
                  new XzaarScriptLexer(), 
                  new XzaarScriptParser(), 
                  new XzaarScriptTransformer(), 
                  new DotNetXzaarScriptCompiler())
        {
            RegisterVariables(parameters);
        }
    }
}