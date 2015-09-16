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
using System.Web.UI;
using KPComponents.KPDelegate;
using KPAttributes;
using KPExtension;

namespace KPComponents
{
    /// <summary>
    /// This is the CheckBox item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelField"/>
    [ToolboxData(@"<{0}:KPFormItemCheckBox runat=""server"" FieldName="""" />")]
    public class KPFormItemCheckBox : KPFormItemModelField
    {

        #region Properties

        /// <summary>
        /// The description propety to the check box.
        /// </summary>
        public string Description
        {
            get
            {
                object o = ViewState["Description"];
                return o == null ? null : (String) o;
            }
            set { ViewState["Description"] = value; }
        }

        /// <summary>
        /// Set this property to "true" if you don't need to create the space above the field, 
        /// for example when you need to create a group with checkboxes.
        /// 
        /// default = false (used by default in forms)
        /// </summary>
        public bool IsGroupStyleDisplay
        {
            get
            {
                object o = ViewState["IsGroupStyleDisplay"];
                return o == null ? false : (bool) o;
            }
            set { ViewState["IsGroupStyleDisplay"] = value; }
        }

        public bool DefaultValue
        {
            get
            {
                object o = ViewState["DefaultValue"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["DefaultValue"] = value; }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Only get the KP check box select Delegate event.
        /// </summary>
        /// <seealso cref="KPDelegate.KPCheckBoxSelect"/>
        [Browsable(false)]
        public KPCheckBoxSelect KPCheckBoxSelectDelegate
        {
            private set;
            get;
        }

        /// <summary>
        /// Add or remove the Select event for the CheckBox
        /// </summary>
        /// <seealso cref="KPDelegate.KPCheckBoxSelect"/>
        public event KPCheckBoxSelect KPEventCheckBoxSelect
        {
            add { KPCheckBoxSelectDelegate += value; }
            remove { KPCheckBoxSelectDelegate -= value; }
        }

        #endregion Events

    }
}
