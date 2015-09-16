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
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KPExtension
{
    /// <summary>
    /// Class for Extension Methods Controls
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public static class KPControlExtension
    {
        /// <summary>
        /// Get the Object by Id into Container
        /// Navigate into parent object, searching child objects with id
        /// </summary>
        /// <typeparam name="T">Generic Type Control</typeparam>
        /// <param name="root">Parent object</param>
        /// <param name="id">Child object Id</param>
        /// <returns>If found, return Child Object, else return null</returns>
        public static T BetterFindControl<T>(this Control root, string id) where T : Control
        {
            if (root != null)
            {
                if (root.ID == id) return root as T;

                var foundControl = (T)root.FindControl(id);
                if (foundControl != null) return foundControl;

                foreach (Control childControl in root.Controls)
                {
                    foundControl = (T)BetterFindControl<T>(childControl, id);
                    if (foundControl != null) return foundControl as T;
                }
            }
            return null;
        }

        /// <summary>
        /// Get all objects WebControl by type 
        /// </summary>
        /// <param name="root">Parent object</param>
        /// <param name="controlType">Type searched</param>
        /// <returns>All objects WebControl found</returns>
        public static WebControl[] GetWebControlsType(this Control root, Type controlType)
        {
            List<WebControl> webControlList = new List<WebControl>();
            if (root != null)
            {
                if (controlType.IsAssignableFrom(root.GetType()) && root is WebControl)
                    webControlList.Add((WebControl)root);

                foreach (Control childControl in root.Controls)
                {
                    webControlList.AddRange(GetWebControlsType(childControl, controlType));
                }
            }
            return webControlList.ToArray();
        }
    }
}
