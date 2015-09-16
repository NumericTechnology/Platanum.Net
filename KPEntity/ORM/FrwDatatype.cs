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

namespace KPEntity.ORM
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    [KPEntityTable("FRW_DATATYPE")]
    public class FrwDatatype : KPActiveRecordBase<FrwDatatype>
    {
        #region ID_DATATYPE
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_DATATYPE")]
        public virtual Int32 IdDatatype { get; set; }
        #endregion ID_DATATYPE

        #region TYPE_NAME
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_TypeName")]
        [Length(Max = 255)]
        [Property(Column = "TYPE_NAME")]
        public virtual String TypeName { get; set; }
        #endregion TYPE_NAME
    }
}
