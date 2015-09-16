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

using Castle.ActiveRecord;
using KPAttributes;
using KPCore.KPValidator;
using KPEntity.ORM;
using NHibernate.Validator.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KPEntity.ORM
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    [KPEntityTable("FRW_PROFILE_WINDOW", "objIdCompany")]
    public class FrwProfileWindow : KPActiveRecordBase<FrwProfileWindow>
    {

        #region ID_PROFILE_WINDOW
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_PROFILE_WINDOW")]
        public virtual Int32 IdProfileWindow { get; set; }
        #endregion ID_PROFILE_WINDOW

        #region ID_COMPANY
        [NotNull]
        [KPDisplayName("FRWEntity_Company")]
        [BelongsTo("ID_COMPANY", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwCompany objIdCompany { get; set; }
        #endregion ID_COMPANY

        #region ID_WINDOW
        [NotNull]
        [KPDisplayName("FRWEntity_Window")]
        [BelongsTo("ID_WINDOW", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwWindow objIdWindow { get; set; }
        #endregion ID_WINDOW

        #region ID_PROFILE
        [NotNull]
        [KPDisplayName("FRWEntity_Profile")]
        [BelongsTo("ID_PROFILE", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwProfile objIdProfile { get; set; }
        #endregion ID_PROFILE

        #region IS_PREVIEW
        [NotNull]
        [KPDisplayName("FRWEntity_IsPreview")]
        [Property(Column = "IS_PREVIEW")]
        public virtual Boolean? IsPreview { get; set; }
        #endregion IS_PREVIEW

        #region IS_READONLY
        [NotNull]
        [KPDisplayName("FRWEntity_IsReadOnly")]
        [Property(Column = "IS_READONLY")]
        public virtual Boolean? IsReadOnly { get; set; }
        #endregion IS_READONLY

        #region Properties Computed
        public string FrwProfileDescription
        {
            get
            {
                if (objIdProfile != null)
                    return objIdProfile.Description;

                return null;
            }
        }

        public string FrwWindowTitle
        {
            get
            {
                if (objIdWindow != null)
                    return objIdWindow.WindowTitle;

                return null;
            }
        }

        #endregion
    }
}
