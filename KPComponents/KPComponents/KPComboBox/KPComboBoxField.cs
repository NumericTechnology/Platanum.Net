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
using KPComponents.KPData;
using System.Web.UI;
using System.Web.UI.WebControls;
using KPComponents.Asset;

namespace KPComponents
{
    /// <summary>
    /// A ComboBox field component
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPComboBox"/>
    /// <seealso cref="KPData.IKPComponentData"/>
    [ToolboxData(@"<{0}:KPComboBoxField runat=""server"" FieldName=""FIELD_NAME"" />")]
    public class KPComboBoxField : KPComboBox, IKPComponentData
    {
        /// <summary>
        /// Get or Set the Field Name of the Entity you want to use in this component
        /// </summary>
        public string FieldName
        {
            get
            {
                object o = ViewState["FieldName"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FieldName"] = value; }
        }

        /// <summary>
        /// Set the invalid message to be shown in this field
        /// </summary>
        /// <param name="errorMsg">error message</param>
        public void SetInvalidateMsg(string errorMsg)
        {
            this.Attributes.Add("title", errorMsg);
            this.CssClass += " " + KPCssClass.InvalidateField;
        }

        /// <summary>
        /// Removes all the invalid message to this field 
        /// </summary>
        public void RemoveInvalidateMsg()
        {
            this.Attributes.Remove("title");
            this.CssClass = this.CssClass.Replace(KPCssClass.InvalidateField, String.Empty);
        }
    }
}
