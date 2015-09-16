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
    [KPEntityTable("FRW_ACTIVITY_LOG", "objIdCompany")]
	public class FrwActivityLog : KPActiveRecordBase<FrwActivityLog>
	{
        #region ID_ACTIVITY_LOG
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_ACTIVITY_LOG")]
        public virtual Int32 IdActivityLog { get; set; }
        #endregion ID_ACTIVITY_LOG

        #region ID_USER
        [KPDisplayName("FRWEntity_User")]
        [BelongsTo("ID_USER", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwUser objIdUser { get; set; }
        #endregion ID_USER

        #region LOGIN
        [KPDisplayName("KPEntity_Login")]
        [Length(Max = 255)]
        [Property(Column = "LOGIN")]
        public virtual String Login { get; set; }
        #endregion LOGIN

        #region ID_COMPANY
        [KPDisplayName("FRWEntity_Company")]
        [BelongsTo("ID_COMPANY", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwCompany objIdCompany { get; set; }
        #endregion ID_COMPANY

        #region ID_WINDOW
        [KPDisplayName("FRWEntity_Window")]
        [BelongsTo("ID_WINDOW", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwWindow objIdWindow { get; set; }
        #endregion ID_WINDOW

        #region SESSION_ID
        [KPDisplayName("FRWEntity_SessionID")]
        [Length(Max = 255)]
        [Property(Column = "SESSION_ID")]
        public virtual String SessionId { get; set; }
        #endregion SESSION_ID

        #region DATE_ACCESS
        [NotNull]
        [KPDisplayName("FRWEntity_DataAccess")]
        [Property(Column = "DATE_ACCESS")]
        public virtual DateTime? DateAccess { get; set; }
        #endregion DATE_ACCESS

        #region IP_ADDRESS
        [KPDisplayName("FRWEntity_IPv4_Internal")]
        [Length(Max = 255)]
        [Property(Column = "IP_ADDRESS")]
        public virtual string IPAddress { get; set; }
        #endregion IP_ADDRESS

        #region DESCRIPTION
        [NotNull, NotEmpty]
        [KPDisplayName("KPEntity_Description")]
        [Length(Max = 255)]
        [Property(Column = "DESCRIPTION")]
        public virtual String Description { get; set; }
        #endregion DESCRIPTION

	}
}
