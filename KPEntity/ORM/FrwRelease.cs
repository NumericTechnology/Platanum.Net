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
    [KPEntityTable("FRW_RELEASE")]
    public class FrwRelease : KPActiveRecordBase<FrwRelease>
    {

        #region ID_RELEASE
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_RELEASE")]
        public virtual Int32 IdRelease { get; set; }
        #endregion ID_RELEASE

        #region BLOCK
        [NotNull]
        [KPDisplayName("FRWEntity_Block")]
        [Property(Column = "BLOCK")]
        public virtual Int32? Block { get; set; }
        #endregion BLOCK

        #region USER_EXEC
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_User")]
        [Length(Max = 255)]
        [Property(Column = "USER_EXEC")]
        public virtual String UserExec { get; set; }
        #endregion USER_EXEC

        #region KEY_DATABASE
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_KeyDatabase")]
        [Length(Max = 255)]
        [Property(Column = "KEY_DATABASE")]
        public virtual String KeyDatabase { get; set; }
        #endregion KEY_DATABASE

        #region DATE_UPDATED
        [NotNull]
        [KPDisplayName("FRWEntity_DateUpdated")]
        [Property(Column = "DATE_UPDATED")]
        public virtual DateTime? DateUpdated { get; set; }
        #endregion DATE_UPDATED

    }
}
