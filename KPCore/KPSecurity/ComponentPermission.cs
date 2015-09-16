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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KPCore.KPSecurity
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class ComponentPermission
    {
        public const string WINDOW_UNKNOW = "_UNKNOW_";
        public const string COMPONENT_ACTION_FORM_SAVE = "ACTION_FORM_SAVE";

        public ComponentPermission(int pageId, string pageEnum, int componentId, string componentNameId)
        {
            PageId = pageId;
            if (!String.IsNullOrWhiteSpace(pageEnum))
                PageEnum = pageEnum;
            else
                PageEnum = ComponentPermission.WINDOW_UNKNOW;

            ComponentId = componentId;
            if (!String.IsNullOrWhiteSpace(componentNameId))
                ComponentNameId = componentNameId;
            else
                ComponentNameId = ComponentPermission.WINDOW_UNKNOW;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int PageId { get; private set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string PageEnum { get; private set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int ComponentId { get; private set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ComponentNameId { get; private set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEnabled { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsVisible { get; set; }
    }
}
