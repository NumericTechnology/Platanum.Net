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
    [KPEntityTable("FRW_PARAM_LIST", "objIdCompany")]
    public class FrwParamList : KPActiveRecordBase<FrwParamList>
    {

        #region ID_PARAM_LIST
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_PARAM_LIST")]
        public virtual Int32 IdParamList { get; set; }
        #endregion ID_PARAM_LIST

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

        #region ID_PARAM
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_Param")]
        [Length(Max = 100)]
        [BelongsTo("ID_PARAM", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwParam objIdParam { get; set; }
        #endregion ID_PARAM

        #region VALUE
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_Value")]
        [Length(Max = 255)]
        [Property(Column = "VALUE")]
        public virtual String Value { get; set; }
        #endregion VALUE

        #region DESCRIPTION
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_Description")]
        [Length(Max = 255)]
        [Property(Column = "DESCRIPTION")]
        public virtual String Description { get; set; }
        #endregion DESCRIPTION

    }
}
