using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.XzaarScript
{
    internal class XzaarTypeBuilder : XzaarType
    {
        private readonly List<XzaarMethodInfo> methods = new List<XzaarMethodInfo>();
        private readonly List<XzaarFieldInfo> fields = new List<XzaarFieldInfo>();

        private readonly bool _isArray;
        private readonly XzaarType _arrayElementType;
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
                _isArray = true;
                _arrayElementType = elementType;
            }
        }

        public void AddMethod(XzaarMethodInfo method)
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

        public override XzaarMethodInfo[] GetMethods()
        {
            return methods.ToArray();
        }
        
        public override XzaarFieldInfo[] GetFields()
        {
            return fields.ToArray();
        }

        public override XzaarMethodInfo GetMethod(string name)
        {
            return methods.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
        }

        public override XzaarFieldInfo GetField(string name)
        {            
            return fields.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
        }
        
        protected override bool IsArrayImpl()
        {
            return _isArray;
        }

        public override XzaarType UnderlyingSystemType { get; }

        protected override XzaarMethodInfo GetMethodImp(string name, XzaarType[] types)
        {
            var mt = methods.Where(m => m.Name == name && m.GetParameters().Count() == types.Length).ToList();
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
            return _arrayElementType;
        }

        public override XzaarType BaseType { get; }

        protected override bool IsByRefImpl()
        {
            return isByRef;
        }
    }
}