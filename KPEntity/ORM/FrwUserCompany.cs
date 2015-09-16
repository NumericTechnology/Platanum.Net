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
    [KPEntityTable("FRW_USER_COMPANY", "objIdCompany")]
    public class FrwUserCompany : KPActiveRecordBase<FrwUserCompany>
    {

        #region ID_USER_COMPANY
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_USER_COMPANY")]
        public virtual Int32 IdUserCompany { get; set; }
        #endregion ID_USER_COMPANY

        #region ID_COMPANY
        [NotNull]
        [KPDisplayName("FRWEntity_Company")]
        [BelongsTo("ID_COMPANY", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwCompany objIdCompany { get; set; }
        #endregion ID_COMPANY

        #region ID_USER
        [NotNull]
        [KPDisplayName("FRWEntity_User")]
        [BelongsTo("ID_USER", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwUser objIdUser { get; set; }
        #endregion ID_USER

        #region IS_DEFAULT_COMPANY
        [NotNull]
        [KPDisplayName("FRWEntity_IsDefaultCompany")]
        [Property(Column = "IS_DEFAULT_COMPANY")]
        public virtual bool? IsDefaultCompany { get; set; }
        #endregion IS_DEFAULT_COMPANY

    }
}
