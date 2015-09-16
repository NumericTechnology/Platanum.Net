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
    [KPEntityTable("FRW_USER", "objIdCompany")]
    public class FrwUser : KPActiveRecordBase<FrwUser>
    {

        #region ID_USER
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_USER")]
        public virtual Int32 IdUser { get; set; }
        #endregion ID_USER

        #region ID_COMPANY
        [NotNull]
        [KPDisplayName("FRWEntity_Company")]
        [BelongsTo("ID_COMPANY", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwCompany objIdCompany { get; set; }
        #endregion ID_COMPANY

        #region USER_LOGIN
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_UserLogin")]
        [Length(Max = 255)]
        [Property(Column = "USER_LOGIN")]
        public virtual String UserLogin { get; set; }
        #endregion USER_LOGIN

        #region USER_FULL_NAME
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_UserFullName")]
        [Length(Max = 255)]
        [Property(Column = "USER_FULL_NAME")]
        public virtual String UserFullName { get; set; }
        #endregion USER_FULL_NAME

        #region USER_PASSWORD
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_UserPassword")]
        [Length(Max = 255)]
        [Property(Column = "USER_PASSWORD")]
        public virtual String UserPassword { get; set; }
        #endregion USER_PASSWORD

        #region REDEFINE_PSWD_NEXT_LOGIN
        [NotNull]
        [KPDisplayName("FRWEntity_RedefinePswdNextLogin")]
        [Property(Column = "REDEFINE_PSWD_NEXT_LOGIN")]
        public virtual bool? RedefinePswdNextLogin { get; set; }
        #endregion REDEFINE_PSWD_NEXT_LOGIN

        #region PHONE
        [KPDisplayName("FRWEntity_Phone")]
        [Length(Max = 13)]
        [Property(Column = "PHONE")]
        public virtual String Phone { get; set; }
        #endregion PHONE

        #region EMAIL
        [KPDisplayName("FRWEntity_Email")]
        [Length(Max = 255)]
        [Property(Column = "EMAIL")]
        public virtual String Email { get; set; }
        #endregion EMAIL

        #region IS_ACCESS_ALLOWED
        [NotNull]
        [KPDisplayName("FRWEntity_IsAccessAllowed")]
        [Property(Column = "IS_ACCESS_ALLOWED")]
        public virtual Boolean? IsAccessAllowed { get; set; }
        #endregion IS_ACCESS_ALLOWED

    }
}
