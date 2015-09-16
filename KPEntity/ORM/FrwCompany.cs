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
    [KPEntityTable("FRW_COMPANY")]
    public class FrwCompany : KPActiveRecordBase<FrwCompany>
    {

        #region ID_COMPANY
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_COMPANY")]
        public virtual Int32 IdCompany { get; set; }
        #endregion ID_COMPANY

        #region COMPANY_NAME
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_CompanyName")]
        [Length(Max = 255)]
        [Property(Column = "COMPANY_NAME")]
        public virtual String CompanyName { get; set; }
        #endregion COMPANY_NAME

        #region COMPANY_FANTASY_NAME
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_FantasyName")]
        [Length(Max = 255)]
        [Property(Column = "COMPANY_FANTASY_NAME")]
        public virtual String CompanyFantasyName { get; set; }
        #endregion COMPANY_FANTASY_NAME

        #region PHONE
        [KPDisplayName("FRWEntity_Phone")]
        [Length(Max = 13)]
        [Property(Column = "PHONE")]
        public virtual String Phone { get; set; }
        #endregion PHONE

    }
}
