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

using KPEnumerator.KPComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPComponents.KPSession
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class KPSessionMasterDetailData
    {
        #region Properties

        /// <summary>
        /// Get KPGridControl State 
        /// </summary>
        public KPFormStateEnum FormState { get; private set; }

        /// <summary>
        /// Get HTML TableID on KPGridControl
        /// </summary>
        public string GridTableID { get; private set; }

        /// <summary>
        /// Entity object [selected row]
        /// </summary>
        public object Entity { get; private set; }

        /// <summary>
        /// Entity Type
        /// </summary>
        public Type TypeEntity
        {
            get
            {
                if (Entity != null)
                    return Entity.GetType();
                else return null;
            }
        }
        #endregion

        /// <summary>
        /// Construtor default
        /// </summary>
        /// <param name="kpGridControl">KPGridControl control</param>
        /// <param name="entity">Entity object</param>
        /// <param name="formState">KPGridControl State</param>
        public KPSessionMasterDetailData(KPGridControl kpGridControl, object entity, KPFormStateEnum formState)
        {
            GridTableID = kpGridControl.ID_Table;
            Entity = entity;
            FormState = formState;
        }
    }
}
