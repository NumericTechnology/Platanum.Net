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
using KPComponents.Generic;
using System.Web.UI;
using KPAttributes;

namespace KPComponents
{
    /// <summary>
    /// This is a Model for Boolean columns used in the component <see cref="KPGridControl">KPGridControl</see>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPBooleanModel FieldName="""" CustomTrue="""" CustomFalse="""" runat=""server"" />")]
    [Serializable]
    public class KPBooleanModel : KPItemModel
    {
        /// <summary>
        /// Default constructor to the component
        /// </summary>
        public KPBooleanModel()
            : base()
        {

        }

        /// <summary>
        /// Custom label to the TRUE value.
        /// P.S.: This property could receive the Resource Translation Key.
        /// </summary>
        public string CustomTrue
        {
            get
            {
                // TODO: IMPORTANT: Jacobi, devemos prever que este campo pode receber como valor a chave da tradução do Resource.
                object o = ViewState["CustomTrue"];
                return o == null ? null : (String)o;
            }
            set { ViewState["CustomTrue"] = value; }
        }

        /// <summary>
        /// Custom label to the FALSE value.
        /// P.S.: This property could receive the Resource Translation Key.
        /// </summary>
        public string CustomFalse
        {
            get
            {
                // TODO: IMPORTANT: Jacobi, devemos prever que este campo pode receber como valor a chave da tradução do Resource.
                object o = ViewState["CustomFalse"];
                return o == null ? null : (String)o;
            }
            set { ViewState["CustomFalse"] = value; }
        }
    }
}
