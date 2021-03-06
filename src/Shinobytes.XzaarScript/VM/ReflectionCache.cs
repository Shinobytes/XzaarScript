﻿/* 
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shinobytes.XzaarScript.VM
{
    public class ReflectionCache
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
        private readonly VirtualMachineInstructionInterpreter instructionInterpreter;
        private readonly VirtualMachine virtualMachine;

        public ReflectionCache(VirtualMachineInstructionInterpreter instructionInterpreter, VirtualMachine virtualMachine)
        {
            this.instructionInterpreter = instructionInterpreter;
            this.virtualMachine = virtualMachine;
        }

        private BindingFlags GetAccessibilityBindingFlags()
        {
            if (virtualMachine.Runtime != null)
            {
                if (virtualMachine.Runtime.Settings.CanAccessPrivateMembers)
                {
                    return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
                }
            }
            return BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
        }

        public MethodInfo[] GetMethods(Type type)
        {
            if (!typeMethodInfoArrayCache.ContainsKey(type))
            {
                typeMethodInfoArrayCache.Add(type,
                    type.GetMethods(GetAccessibilityBindingFlags()));
            }

            return typeMethodInfoArrayCache[type];
        }

        public FieldInfo[] GetFields(Type type)
        {
            if (!typeFieldInfoArrayCache.ContainsKey(type))
            {
                typeFieldInfoArrayCache.Add(type,
                    type.GetFields(GetAccessibilityBindingFlags()));
            }
            return typeFieldInfoArrayCache[type];
        }

        public PropertyInfo[] GetProperties(Type type)
        {
            if (!typePropertyInfoArrayCache.ContainsKey(type))
            {
                typePropertyInfoArrayCache.Add(type,
                    type.GetProperties(GetAccessibilityBindingFlags()));
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