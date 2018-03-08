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

using System;
using System.Collections.Generic;

namespace Shinobytes.XzaarScript
{
    [Serializable]
    public abstract class XzaarTypeInfo : XzaarType
    {
        internal XzaarTypeInfo() { }

        public virtual XzaarType AsType()
        {
            return this;
        }

        public virtual XzaarFieldInfo GetDeclaredField(string name)
        {
            return GetField(name);
        }

        public virtual XzaarMethodBase GetDeclaredMethod(string name)
        {
            return GetMethod(name);
        }
        
        public virtual IEnumerable<XzaarFieldInfo> GetDeclaredFields(string name)
        {
            foreach (XzaarFieldInfo field in GetFields())
            {
                if (field.Name == name)
                    yield return field;
            }
        }

        public virtual IEnumerable<XzaarMethodBase> GetDeclaredMethods(string name)
        {
            foreach (XzaarMethodBase method in GetMethods())
            {
                if (method.Name == name)
                    yield return method;
            }
        }
    }
}