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
using KPEnumerator.KPEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class DetailEntity
    {
        public DetailEntity(ActiveRecordBase entity, StateDetailEntity stateEntity)
        {
            Entity = entity;
            StateEntity = stateEntity;
        }

        public ActiveRecordBase Entity { get; private set; }
        public StateDetailEntity StateEntity { get; private set; }
    }
}
