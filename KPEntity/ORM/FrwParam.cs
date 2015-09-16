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
    [KPEntityTable("FRW_PARAM", "objIdCompany")]
    public class FrwParam : KPActiveRecordBase<FrwParam>
    {

        #region ID_PARAM
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_PARAM")]
        [Length(Max = 100)]
        public virtual String IdParam { get; set; }
        #endregion ID_PARAM

        #region ID_COMPANY
        [KPDisplayName("FRWEntity_Company")]
        [BelongsTo("ID_COMPANY", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwCompany objIdCompany { get; set; }
        #endregion ID_COMPANY

        #region ID_DATATYPE
        [NotNull]
        [KPDisplayName("FRWEntity_DataType")]
        [BelongsTo("ID_DATATYPE", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwDatatype objIdDatatype { get; set; }
        #endregion ID_DATATYPE

        #region DESCRIPTION
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_Description")]
        [Length(Max = 255)]
        [Property(Column = "DESCRIPTION")]
        public virtual String Description { get; set; }
        #endregion DESCRIPTION

        #region IS_EDITABLE
        [KPDisplayName("FRWEntity_IsEditable")]
        [Property(Column = "IS_EDITABLE")]
        public virtual Boolean? IsEditable { get; set; }
        #endregion IS_EDITABLE

        #region DEFAULT_VALUE
        [KPDisplayName("FRWEntity_DefaultValue")]
        [Length(Max = 255)]
        [Property(Column = "DEFAULT_VALUE")]
        public virtual String DefaultValue { get; set; }
        #endregion DEFAULT_VALUE

        #region IS_COMPANY_REQUIRED
        [NotNull]
        [KPDisplayName("FRWEntity_IsCompanyRequired")]
        [Property(Column = "IS_COMPANY_REQUIRED")]
        public virtual Boolean? IsCompanyRequired { get; set; }
        #endregion IS_COMPANY_REQUIRED

        #region REGEX
        [KPDisplayName("FRWEntity_Regex")]
        [Length(Max = 255)]
        [Property(Column = "REGEX")]
        public virtual String Regex { get; set; }
        #endregion REGEX

    }
}
