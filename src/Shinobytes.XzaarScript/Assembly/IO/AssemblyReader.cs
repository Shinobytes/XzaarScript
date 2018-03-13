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
 
using System;
using System.IO;

namespace Shinobytes.XzaarScript.Assembly.IO
{
    public class AssemblyReader
    {
        private AssemblyReader()
        {
        }

        public static XzaarAssembly ReadBytes(byte[] binary)
        {
            var reader = new AssemblyReader();

            return reader.Read(binary);
        }

        public static XzaarAssembly ReadFile(string file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            return ReadBytes(File.ReadAllBytes(file));
        }

        private XzaarAssembly Read(byte[] binary)
        {
            var assembly = new XzaarAssembly();
            var ctx = new AssemblyReaderContext(binary, assembly);
            return this.Read(ctx);
        }

        private XzaarAssembly Read(AssemblyReaderContext ctx)
        {
            return ctx.Assembly;
        }
    }
}
