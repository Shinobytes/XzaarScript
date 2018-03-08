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

using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript
{
    internal class XzaarTypeBuilder : XzaarType
    {
        private readonly List<XzaarMethodBase> methods = new List<XzaarMethodBase>();
        private readonly List<XzaarFieldInfo> fields = new List<XzaarFieldInfo>();

        private readonly bool isArray;
        private readonly XzaarType arrayElementType;
        private bool isByRef;

        public XzaarTypeBuilder(string name, XzaarType declaringType, XzaarType underlyingSystemType, XzaarType baseType, XzaarType elementType = null)
        {
            Name = name;
            MemberType = XzaarMemberTypes.TypeInfo;
            DeclaringType = declaringType;
            UnderlyingSystemType = underlyingSystemType;
            BaseType = baseType;
            if (elementType != null)
            {
                isArray = true;
                arrayElementType = elementType;
            }
        }

        public void AddMethod(XzaarMethodBase method)
        {
            this.methods.Add(method);
        }

        public void AddField(XzaarFieldInfo item)
        {
            this.fields.Add(item);
        }

        public void AddField(XzaarType type, string name)
        {
            this.fields.Add(new XzaarFieldBuilder(name, type, this));
        }

        public override string Name { get; }

        public override XzaarType GetXzaarType()
        {
            return this;
        }

        public override XzaarMemberTypes MemberType { get; }

        public override XzaarType DeclaringType { get; }

        public override XzaarMethodBase[] GetMethods()
        {
            return methods.ToArray();
        }
        
        public override XzaarFieldInfo[] GetFields()
        {
            return fields.ToArray();
        }

        public override XzaarMethodBase GetMethod(string name)
        {
            return methods.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
        }

        public override XzaarFieldInfo GetField(string name)
        {            
            return fields.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
        }
        
        protected override bool IsArrayImpl()
        {
            return isArray;
        }

        public override XzaarType UnderlyingSystemType { get; }

        protected override XzaarMethodBase GetMethodImp(string name, XzaarType[] types)
        {
            var mt = methods.Where(m => m.Name == name && m.GetParameters().Length == types.Length).ToList();
            foreach (var m in mt)
            {
                var param = m.GetParameters();
                if (ParamTypeSequenceMatch(param, types)) return m;
            }
            return null;
        }

        private bool ParamTypeSequenceMatch(XzaarParameterInfo[] param, XzaarType[] types)
        {
            if (param.Length != types.Length) return false;
            for (var i = 0; i < param.Length; i++) if (param[i].ParameterType != types[i]) return false;
            return true;
        }

        public override XzaarType GetElementType()
        {
            return arrayElementType;
        }

        public override XzaarType BaseType { get; }

        protected override bool IsByRefImpl()
        {
            return isByRef;
        }
    }
}