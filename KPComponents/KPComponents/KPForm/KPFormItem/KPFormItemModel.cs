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
using KPComponents.Generic;
using System.Web.UI;
using System.ComponentModel;

namespace KPComponents
{
    /// <summary>
    /// The default item model used in the form components
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormBaseModel"/>
    public class KPFormItemModel : KPFormBaseModel
    {
        #region Properties

        /// <summary>
        /// The default width property for the components who extends the KPFormItemModel
        /// </summary>
        public virtual int Width
        {
            get
            {
                object o = ViewState["Width"];
                return o == null ? 100 : (int) o;
            }
            set { ViewState["Width"] = value; }
        }

        /// <summary>
        /// The Index Tab property, refers to the <see cref="KPFormTabControl">Tab</see> this component is in.
        /// default: null, normally when the form has no <see cref="KPFormTabControl">Tabs</see>.
        /// </summary>
        [Browsable(false)]
        public int? IndexTab
        {
            get
            {
                object o = ViewState["IndexTab"];
                return (int?)o;
            }
            set { ViewState["IndexTab"] = value; }
        }

        #endregion Properties
    }
}
