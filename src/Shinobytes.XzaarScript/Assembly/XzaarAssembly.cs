/* 
 *  This file is part of XzaarScript.
 *  Copyright © 2018 Karl Patrik Johansson, zerratar@gmail.com
 *
 *  XzaarScript is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  XzaarScript is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with XzaarScript.  If not, see <http://www.gnu.org/licenses/>. 
 *  
 */

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
