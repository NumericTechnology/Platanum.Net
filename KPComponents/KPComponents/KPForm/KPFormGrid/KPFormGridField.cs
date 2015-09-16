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

using KPComponents.Generic;
using KPEnumerator.KPComponents;
using KPGlobalization;
using System;
using System.ComponentModel;

namespace KPComponents
{
    /// <summary>
    /// This is a field for a form grid, used as a column in the component <see cref="KPFormGridModel">KPFormGridModel</see>.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="Generic.StateManagedItem"/>
    public class KPFormGridField : StateManagedItem
    {
        /// <summary>
        /// The Field of the component
        /// </summary>
        public string Field
        {
            get
            {
                object o = ViewState["Field"];
                return o == null ? null : (String)o;
            }
            set { ViewState["Field"] = value; }
        }

        /// <summary>
        /// The Header Name of the component
        /// </summary>
        public string HeaderName
        {
            get
            {
                object o = ViewState["HeaderName"];
                return o == null ? null : (String)o;
            }
            set { ViewState["HeaderName"] = KPGlobalizationLanguage.GetString(value); }
        }

        /// <summary>
        /// The width of the component
        /// </summary>
        public int Width
        {
            get
            {
                object o = ViewState["Width"];
                return o == null ? 100 : (int)o;
            }
            set { ViewState["Width"] = value; }
        }

        /// <summary>
        /// Mask for the field.
        /// </summary>
        /// <seealso cref="KPEnumerator.KPComponents.KPMaskTypeClassEnum"/>
        [DefaultValue(KPMaskTypeClassEnum.ALPHANUMERIC)]
        public KPMaskTypeClassEnum Mask
        {
            get;
            set;
        }
    }
}
