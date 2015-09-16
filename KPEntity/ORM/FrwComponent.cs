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
    [KPEntityTable("FRW_COMPONENT")]
    public class FrwComponent : KPActiveRecordBase<FrwComponent>
    {

        #region ID_COMPONENT
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column="ID_COMPONENT")]
        public virtual Int32 IdComponent { get; set; }
        #endregion ID_COMPONENT

        #region ID_WINDOW
        [NotNull]
        [KPDisplayName("FRWEntity_Window")]
        [BelongsTo("ID_WINDOW", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwWindow objIdWindow { get; set; }
        #endregion ID_WINDOW

        #region COMPONENT_NAME_ID
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_ComponentNameId")]
        [Length(Max = 255)]
        [Property(Column="COMPONENT_NAME_ID")]
        public virtual String ComponentNameId { get; set; }
        #endregion COMPONENT_NAME_ID

        #region TRANSLATE_KEY_NAME
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_TranslateKeyName")]
        [Length(Max = 255)]
        [Property(Column="TRANSLATE_KEY_NAME")]
        public virtual String TranslateKeyName { get; set; }
        #endregion TRANSLATE_KEY_NAME

        #region DESCRIPTION
        [KPDisplayName("FRWEntity_Description")]
        [Length(Max = 4000)]
        [Property(Column="DESCRIPTION")]
        public virtual String Description { get; set; }
        #endregion DESCRIPTION

    }
}
