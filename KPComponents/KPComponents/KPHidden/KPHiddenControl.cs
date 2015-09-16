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
using KPComponents.KPData;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPHiddenControl runat=""server"" FieldName=""FIELD_NAME"" />")]
    public class KPHiddenControl : System.Web.UI.WebControls.HiddenField
    {
        public string FieldName
        {
            get
            {
                object o = ViewState["FieldName"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FieldName"] = value; }
        }
    }
}
