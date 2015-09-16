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
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// This is the Grid item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelField"/>
    [ToolboxData(@"<{0}:KPFormItemGrid runat=""server"" FieldName="""" />")]
    public class KPFormItemGrid : KPFormItemModelField
    {

        #region Properties

        /// <summary>
        /// The grid ID config property for the Grid form component
        /// </summary>
        public string GridIDConfig
        {
            get
            {
                object o = ViewState["GridIDConfig"];
                return o == null ? null : (String)o;
            }
            set { ViewState["GridIDConfig"] = value; }
        }

        /// <summary>
        /// The height property for the grid form component
        /// </summary>
        public int Height
        {
            get
            {
                object o = ViewState["Height"];
                return o == null ? 150 : (int)o;
            }
            set { ViewState["Height"] = value; }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Only get the KP grid delete Delegate event.
        /// </summary>
        /// <seealso cref="System.Web.UI.WebControls.GridViewDeleteEventHandler"/>
        [Browsable(false)]
        public GridViewDeleteEventHandler KPGridViewDeleteDelegate
        {
            private set;
            get;
        }

        /// <summary>
        /// Add or remove the Grid Delete event for the grid form component
        /// </summary>
        /// <seealso cref="System.Web.UI.WebControls.GridViewDeleteEventHandler"/>
        public event GridViewDeleteEventHandler KPDeleteLine
        {
            add { KPGridViewDeleteDelegate += value; }
            remove { KPGridViewDeleteDelegate -= value; }
        }

        #endregion Events

    }
}
