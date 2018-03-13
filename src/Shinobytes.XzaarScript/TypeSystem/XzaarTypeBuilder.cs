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