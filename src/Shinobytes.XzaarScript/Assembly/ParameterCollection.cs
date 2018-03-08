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
    public class ParameterCollection : Collection<ParameterDefinition>
    {
        public ParameterCollection(MethodReference method)
        {
            Method = method;
        }

        public MethodReference Method { get; internal set; }

        public void AddParameters(ParameterDefinition[] parameters)
        {
            foreach (var p in parameters) Add(p);
        }

        public override void Add(ParameterDefinition item)
        {
            item.Method = this.Method;
            base.Add(item);
        }
    }
}