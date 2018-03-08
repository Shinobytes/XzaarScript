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

using System.Text;

namespace Shinobytes.XzaarScript.Utilities
{
    public class XzaarCodeWriter
    {
        private readonly StringBuilder sb = new StringBuilder();

        public void Write(object text, int currentIndent = 0)
        {
            Indent(currentIndent);
            sb.Append(text);
        }

        public void Write(string text, int currentIndent = 0)
        {
            Indent(currentIndent);
            sb.Append(text);
        }

        public void WriteLine(string text, int currentIndent = 0)
        {
            Indent(currentIndent);
            sb.Append(text);
            NewLine();
        }

        public void NewLine()
        {
            sb.AppendLine();
        }

        private void Indent(int num)
        {
            for (var i = 0; i < num; i++)
            {
                sb.Append("  ");
            }
        }

        public override string ToString()
        {
            return GetSourceCode();
        }

        public string GetSourceCode()
        {
            return sb.ToString();
        }
    }
}