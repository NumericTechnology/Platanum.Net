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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using KPCore.KPValidator;
using KPBO.Validator;
using NHibernate.Validator.Engine;
using KPExtension;

namespace KPBO
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public abstract class BaseBO<Entity> where Entity : KPActiveRecordBase<Entity>
    {
        public BaseBO(Entity entity)
        {
            if (entity == null)
            {
                entity = (Entity)Activator.CreateInstance(typeof(Entity));
            }

            EntityField = entity;
            InvalidEntityHeader = new List<InvalidValueBO>();
            InvalidValues = new List<InvalidValue>();
        }

        public Entity EntityField { get; private set; }
        public List<InvalidValueBO> InvalidEntityHeader { get; protected set; }
        public List<InvalidValue> InvalidValues { get; protected set; }

        public void AddInvalidValue(string fieldName, string message)
        {
            this.AddInvalidValue(fieldName, message, null);
        }

        public void AddInvalidValue(string fieldName, string message, object value)
        {
            this.AddInvalidValue(fieldName, message, value, null);
        }

        public void AddInvalidValue(string fieldName, string message, object value, ICollection<object> matchTags)
        {
            this.InvalidValues.Add(new InvalidValue(message, EntityField.GetType(), fieldName, value, EntityField, matchTags));
        }

        /// <summary>
        /// Verify if the current Entity (this.EntityField) is in an Update mode.
        /// </summary>
        public bool IsUpdate
        {
            get
            {
                bool isUpdate = false;

                try
                {
                    isUpdate = !(Convert.ToInt32(this.EntityField.GetType().GetEntityPrimaryKey().GetValue(EntityField, null)) == 0);
                }
                catch
                {
                }

                return isUpdate;
            }
        }

        public Entity SaveEntityBase()
        {
            if (Validate())
            {
                return SaveEntity();
            }

            return null;
        }

        public virtual Entity SaveEntity()
        {
            MethodInfo methodSave = typeof(Entity).GetMethod("Save");
            methodSave.Invoke(EntityField, null);
            return EntityField;
        }

        public virtual bool Validate()
        {
            return true;
        }
    }
}
