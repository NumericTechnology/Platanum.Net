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
using System.ComponentModel;

namespace KPComponents
{
    /// <summary>
    /// This is a model for entity form fields.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelField"/>
    public abstract class KPFormItemModelEntityField : KPFormItemModelField
    {

        #region Properties

        /// <summary>
        /// The Field Name Description property, it must receive the name of the entity column to show as a description for the KP Entity Field.
        /// </summary>
        public string FieldNameDescription
        {
            get
            {
                object o = ViewState["FieldNameDescription"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FieldNameDescription"] = value; }
        }

        #endregion Properties

    }
}
