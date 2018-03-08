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

using System.Linq;

namespace Shinobytes.XzaarScript.Assembly
{
    public class InstructionVariableCollection : Collection<MemberReference>
    {
        public string ToString(string del)
        {
            var value = base.InternalItems.Select(GetStringRepresentation);
            return string.Join(del, value.ToArray());
        }

        private static string GetStringRepresentation(MemberReference i)
        {
            if (i == null) return null;
            var c = i as Constant;
            if (c != null)
            {
                if (c.ArrayIndex != null)
                {
                    var value = GetStringRepresentation(c.ArrayIndex as VariableReference);
                    if (value != null)
                    {
                        return (c.Value is string ? "\"" + c.Value + "\"" : c.Value) + "[" + value + "]";
                    }
                }

                return c.Value is string ? "\"" + c.Value + "\"" : (c.Value + "");
            }
            var vref = i as VariableReference;
            if (vref != null)
            {
                var value = GetStringRepresentation(vref.ArrayIndex as VariableReference);
                if (value != null)
                {
                    return i.Name + "[" + value + "]";
                }
            }
            return i.Name;
        }
    }
}