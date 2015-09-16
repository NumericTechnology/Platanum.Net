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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public abstract class KPFormHelpModel : StateManagedItem
    {
        public int HelpStep
        {
            get
            {
                object o = ViewState["HelpStep"];
                return o == null ? -1 : (int)o;
            }
            set { ViewState["HelpStep"] = value; }
        }

        public string HelpInfo
        {
            get
            {
                object o = ViewState["HelpInfo"];
                return o != null ? o.ToString() : null;
            }
            set { ViewState["HelpInfo"] = value; }
        }

        public string HelpToolTip
        {
            get
            {
                object o = ViewState["HelpToolTip"];
                return o != null ? o.ToString() : null;
            }
            set { ViewState["HelpToolTip"] = value; }
        }
    }
}
