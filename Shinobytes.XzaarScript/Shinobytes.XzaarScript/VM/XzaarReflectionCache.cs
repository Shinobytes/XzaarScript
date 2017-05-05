using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shinobytes.XzaarScript.VM
{
    public class XzaarReflectionCache
    {
        /*
            Since reflection is slow, we wanna cache all lookups to find what we are looking for much faster
            This class will help us do that :-)
         */

        private readonly Dictionary<Type, MethodInfo[]> typeMethodInfoArrayCache = new Dictionary<Type, MethodInfo[]>();
        private readonly Dictionary<Type, FieldInfo[]> typeFieldInfoArrayCache = new Dictionary<Type, FieldInfo[]>();
        private readonly Dictionary<Type, PropertyInfo[]> typePropertyInfoArrayCache = new Dictionary<Type, PropertyInfo[]>();
        private readonly Dictionary<string, MethodInfo> fullNameMethodInfoCache = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<string, FieldInfo> fullNameFieldInfoCache = new Dictionary<string, FieldInfo>();
        private readonly Dictionary<string, PropertyInfo> fullNamePropertyInfoCache = new Dictionary<string, PropertyInfo>();
        private readonly XzaarVirtualMachineInstructionInterpreter ii;
        private readonly XzaarVirtualMachine vm;

        public XzaarReflectionCache(XzaarVirtualMachineInstructionInterpreter ii, XzaarVirtualMachine vm)
        {
            this.ii = ii;
            this.vm = vm;
        }

        public MethodInfo[] GetMethods(Type type)
        {
            if (!typeMethodInfoArrayCache.ContainsKey(type))
            {
                typeMethodInfoArrayCache.Add(type,
                    type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                    BindingFlags.Instance));
            }

            return typeMethodInfoArrayCache[type];
        }



        public FieldInfo[] GetFields(Type type)
        {
            if (!typeFieldInfoArrayCache.ContainsKey(type))
            {
                typeFieldInfoArrayCache.Add(type,
                    type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                   BindingFlags.Static));
            }
            return typeFieldInfoArrayCache[type];
        }

        public PropertyInfo[] GetProperties(Type type)
        {
            if (!typePropertyInfoArrayCache.ContainsKey(type))
            {
                typePropertyInfoArrayCache.Add(type,
                    type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                       BindingFlags.Static));
            }
            return typePropertyInfoArrayCache[type];
        }

        public MethodInfo GetMethod(Type type, string name)
        {
            var fullName = (type.FullName + "." + name).ToLower();
            if (!fullNameMethodInfoCache.ContainsKey(fullName))
            {
                var methods = GetMethods(type);
                var method = methods.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
                fullNameMethodInfoCache.Add(fullName, method);
            }
            return fullNameMethodInfoCache[fullName];
        }

        public FieldInfo GetField(Type type, string name)
        {
            var fullName = (type.FullName + "." + name).ToLower();
            if (!fullNameFieldInfoCache.ContainsKey(fullName))
            {
                var fields = GetFields(type);
                var field = fields.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
                fullNameFieldInfoCache.Add(fullName, field);
            }
            return fullNameFieldInfoCache[fullName];
        }

        public PropertyInfo GetProperty(Type type, string name)
        {
            var fullName = (type.FullName + "." + name).ToLower();
            if (!fullNamePropertyInfoCache.ContainsKey(fullName))
            {
                var properties = GetProperties(type);
                var property = properties.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
                fullNamePropertyInfoCache.Add(fullName, property);
            }
            return fullNamePropertyInfoCache[fullName];
        }
    }
}