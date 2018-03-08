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

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public sealed class LabelTarget
    {
        private readonly XzaarType type;
        private readonly string name;

        internal LabelTarget(XzaarType type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public string Name => name;

        public XzaarType Type => type;

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? "UnamedLabel" : Name;
        }
    }

    public partial class XzaarExpression
    {
        public static LabelTarget Label()
        {
            return Label(XzaarBaseTypes.Void, null);
        }

        public static LabelTarget Label(string name)
        {
            return Label(XzaarBaseTypes.Void, name);
        }

        public static LabelTarget Label(XzaarType type)
        {
            return Label(type, null);
        }

        public static LabelTarget Label(XzaarType type, string name)
        {
            return new LabelTarget(type, name);
        }
    }
}