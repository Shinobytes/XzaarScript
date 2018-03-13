/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.  
 **/
 
using Shinobytes.XzaarScript.Assembly.IO;

namespace Shinobytes.XzaarScript.Assembly
{
    public class XzaarAssembly
    {
        private string name;
        private string filename;

        public static XzaarAssembly ReadFromFile(string filename)
        {
            return AssemblyReader.ReadFile(filename);
        }

        public void WriteToFile(string filename)
        {
            AssemblyWriter.WriteToFile(this, filename);
        }

        public static XzaarAssembly CreateAssembly(string name)
        {
            return new XzaarAssembly(name, null);
        }

        public static XzaarAssembly CreateAssembly(string name, string outputFilename)
        {
            return new XzaarAssembly(name, null);
        }

        internal XzaarAssembly(string name, string filename)
        {
            this.name = name;
            this.filename = filename;
            Types = new TypeCollection();
            GlobalVariables = new GlobalVariableCollection();
            GlobalInstructions = new GlobalInstructionCollection();
            GlobalMethods = new MethodCollection();
        }

        internal XzaarAssembly() : this(null, null)
        {
            Types = new TypeCollection();
            GlobalVariables = new GlobalVariableCollection();
            GlobalInstructions = new GlobalInstructionCollection();
            GlobalMethods = new MethodCollection();
        }

        internal void SetName(string name)
        {
            this.name = name;
        }

        internal void SetFilename(string filename)
        {
            this.filename = filename;
        }

        public string Name => name;

        public string Filename => filename;

        public TypeCollection Types { get; }
        public GlobalVariableCollection GlobalVariables { get; }
        public GlobalInstructionCollection GlobalInstructions { get; }
        public MethodCollection GlobalMethods { get; }
    }
}
