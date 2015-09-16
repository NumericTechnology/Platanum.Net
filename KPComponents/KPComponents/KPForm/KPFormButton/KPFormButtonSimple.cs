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
    /// A simple KP Form Button
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormButtonModel"/>
    [ToolboxData(@"<{0}:KPFormButtonSimple runat=""server"" />")]
    public class KPFormButtonSimple : KPFormButtonModel
    {
        /// <summary>
        /// Click event Handler
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Title propertie of the Button
        /// </summary>
        public string Title
        {
            get
            {
                object o = ViewState["Title"];
                return o == null ? null : (String)o;
            }
            set { ViewState["Title"] = value; }
        }

        /// <summary>
        /// OnClick function, dispatch the handler when user click the button.
        /// </summary>
        /// <param name="sender">The sender Object</param>
        /// <param name="e">The event args</param>
        public void OnClick(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(this, new EventArgs());
            }
        }
    }
}
