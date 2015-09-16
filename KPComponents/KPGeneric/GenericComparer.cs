/*
 * Copyright 2011-2015 Numeric Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KPComponents.KPGeneric
{
    /// <summary>
    /// Generic Comparer Objects
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class GenericComparer : IComparer<Object>
    {
        /// <summary>
        /// Property comparer
        /// </summary>
        public string PropertyComparer { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyComparer">Property comparer</param>
        public GenericComparer(string propertyComparer)
        {
            PropertyComparer = propertyComparer;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <returns>
        /// Less than zero => x is less than y.
        /// Zero => x equals y.
        /// Greater than zero => x is greater than y.
        /// </returns>
        public int Compare(object x, object y)
        {
            if (!String.IsNullOrEmpty(PropertyComparer))
            {
                PropertyInfo propInfoX = null;
                PropertyInfo propInfoY = null;
                if (x != null)
                    propInfoX = x.GetType().GetProperty(PropertyComparer);

                if (y != null)
                    propInfoY = y.GetType().GetProperty(PropertyComparer);

                if (propInfoX != null && propInfoY != null)
                {
                    MethodInfo methodCompareToObj = propInfoX.PropertyType.GetMethod("CompareTo", new Type[] { typeof(Object) });
                    if (methodCompareToObj != null)
                    {
                        object valueX = propInfoX.GetValue(x, null);
                        object valueY = propInfoY.GetValue(y, null);

                        object retunComparer = methodCompareToObj.Invoke(valueX, new Object[] { valueY });
                        return (int)retunComparer;
                    }
                }
                else if (propInfoX != null)
                    return -1;
                else if (propInfoY != null)
                    return 1;

                return 0;
            }

            if (x != null)
            {
                MethodInfo methodCompareTo = x.GetType().GetMethod("CompareTo", new Type[] { typeof(Object) });
                if (methodCompareTo != null)
                {
                    object retunComparer = methodCompareTo.Invoke(x, new Object[] { y });
                    return (int)retunComparer;
                }
            }

            return 0;
        }
    }
}
