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
    [KPEntityTable("FRW_PROFILE_COMPONENT", "objIdCompany")]
    public class FrwProfileComponent : KPActiveRecordBase<FrwProfileComponent>
    {

        #region ID_PROFILE_COMPONENT
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column="ID_PROFILE_COMPONENT")]
        public virtual Int32 IdProfileComponent { get; set; }
        #endregion ID_PROFILE_COMPONENT

        #region ID_COMPANY
        [NotNull]
        [KPDisplayName("FRWEntity_Company")]
        [BelongsTo("ID_COMPANY", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwCompany objIdCompany { get; set; }
        #endregion ID_COMPANY

        #region ID_PROFILE
        [NotNull]
        [KPDisplayName("FRWEntity_Profile")]
        [BelongsTo("ID_PROFILE", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwProfile objIdProfile { get; set; }
        #endregion ID_PROFILE

        #region ID_COMPONENT
        [NotNull]
        [KPDisplayName("FRWEntity_Component")]
        [BelongsTo("ID_COMPONENT", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwComponent objIdComponent { get; set; }
        #endregion ID_COMPONENT

        #region IS_VISIBLE
        [NotNull]
        [KPDisplayName("FRWEntity_IsVisible")]
        [Property(Column="IS_VISIBLE")]
        public virtual Boolean? IsVisible { get; set; }
        #endregion IS_VISIBLE

        #region IS_ENABLE
        [NotNull]
        [KPDisplayName("FRWEntity_IsEnable")]
        [Property(Column="IS_ENABLE")]
        public virtual Boolean? IsEnable { get; set; }
        #endregion IS_ENABLE

    }
}
