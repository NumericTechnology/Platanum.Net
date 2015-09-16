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
    /// This is a model for form fields.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModel"/>
    public class KPFormItemModelField : KPFormItemModel
    {

        #region Properties

        /// <summary>
        /// The ID property for the fields
        /// </summary>
        public override string ID
        {
            get
            {
                object o = ViewState["ID"];
                return o == null ? FieldName : (String)o;
            }
            set { ViewState["ID"] = value; }
        }

        /// <summary>
        /// The Field Name property of the field, it must be an entity column
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
        /// If true, a red asterisk will appear next to the Title of the field, this symbol means that the field is required.
        /// </summary>
        public bool IsRequired
        {
            get
            {
                object o = ViewState["IsRequired"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["IsRequired"] = value; }
        }

        #endregion Properties

    }
}
