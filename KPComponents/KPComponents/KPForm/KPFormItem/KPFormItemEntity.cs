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

using KPGlobalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace KPComponents
{
    /// <summary>
    /// This is the Entity item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelEntityField"/>
    [ToolboxData(@"<{0}:KPFormItemEntity runat=server FieldName="""" FieldNameDescription="""" />")]
    public class KPFormItemEntity : KPFormItemModelEntityField
    {

        #region Properties

        /// <summary>
        /// The title property for the Entity component
        /// </summary>
        public string Title
        {
            get
            {
                object o = ViewState["Title"];
                return o == null ? null : (String)o;
            }
            set { ViewState["Title"] = KPGlobalizationLanguage.GetString(value); }
        }

        #endregion Properties

    }
}
