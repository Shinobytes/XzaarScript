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

namespace Shinobytes.XzaarScript.Assembly
{
    public class VariableReference : MemberReference
    {
        private VariableReference reference;

        public VariableReference() { }

        public VariableReference(string name, TypeReference type)
        {
            Name = name;
            Type = type;
        }

        public TypeReference Type { get; set; }

        public object InitialValue { get; set; }

        public override MemberTypes MemberType => MemberTypes.Variable;

        public object ArrayIndex { get; set; }

        public VariableReference Reference
        {
            get => reference;
            set => reference = value;
        }

        public bool IsRef => this.reference != null;

        public override string ToString()
        {
            if (ArrayIndex != null)
            {
                return Name + "[" + ArrayIndex + "]";
            }
            return Name + ": " + Type.Name;
        }

        public virtual VariableReference Clone()
        {
            return new VariableReference
            {
                ArrayIndex = this.ArrayIndex,
                InitialValue = this.InitialValue,
                Reference = this.reference,
                Type = this.Type,
                Name = this.Name
            };
        }
    }

    public class FieldReference : VariableReference
    {
        public VariableReference Instance { get; set; }

        public override MemberTypes MemberType => MemberTypes.Field;

        public override string ToString()
        {
            if (ArrayIndex != null)
            {
                return Name + "[" + ArrayIndex + "]";
            }
            return Name + ": " + Type.Name;
        }

        public override VariableReference Clone()
        {
            return new FieldReference
            {
                Instance = this.Instance,
                Name = this.Name,
                Type = this.Type,
                InitialValue = this.InitialValue,
                ArrayIndex = this.ArrayIndex,
                Reference = this.Reference
            };
        }
    }
}