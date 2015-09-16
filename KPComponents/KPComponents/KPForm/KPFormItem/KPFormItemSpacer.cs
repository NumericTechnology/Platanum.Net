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
using KPAttributes;
using KPExtension;
using KPComponents.Generic;

namespace KPComponents
{
    /// <summary>
    /// This is the Spacer item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModel"/>
    [ToolboxData(@"<{0}:KPFormItemSpacer runat=server Width=""10"" />")]
    public class KPFormItemSpacer : KPFormItemModel
    {

        #region Properties

        /// <summary>
        /// The Height of the component
        /// </summary>
        public virtual int Height
        {
            get
            {
                object o = ViewState["Height"];
                return o == null ? 0 : (int)o;
            }
            set { ViewState["Height"] = value; }
        }

        #endregion Properties

    }
}
