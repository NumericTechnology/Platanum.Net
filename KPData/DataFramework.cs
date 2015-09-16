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
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord;
using NHibernate.Validator.Event;
using NHibernate.Validator.Cfg;
using System.IO;

namespace KPData
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class DataFramework
    {
        #region Properties
        protected string KPEntityName { get; private set; }
        public Assembly KPEntity { get; private set; }
        #endregion

        public DataFramework()
        {
            KPEntityName = "KPEntity";
            Assembly assemblyKPEntity = Assembly.Load(KPEntityName);
            if (assemblyKPEntity != null)
                KPEntity = assemblyKPEntity;
        }

        public virtual XmlConfigurationSource GetSource()
        {
            FileInfo xmlConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "ActiveRecord.xml"));
            if (!xmlConfig.Exists)
            {
                xmlConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ActiveRecord.xml"));
                if (!xmlConfig.Exists)
                    throw new Exception(String.Format("Não foi encontrado o arquivo \"ActiveRecord.xml\" de configuração"));
            }

            XmlConfigurationSource source = new XmlConfigurationSource(xmlConfig.FullName);
            return source;
        }

        public void InitializeDatabases(Assembly[] assembliesEntity)
        {
            try
            {
                XmlConfigurationSource source = GetSource();
                ActiveRecordStarter.Initialize(assembliesEntity, source);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RegisterEntities(Assembly[] assembliesEntity)
        {
            try
            {
                ActiveRecordStarter.RegisterAssemblies(assembliesEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void InicializeValidator(LanguageEnum languageValidation)
        {
            NHibernate.Validator.Cfg.Environment.LanguageValidation = languageValidation;
            var provider = new NHibernateSharedEngineProvider();
            NHibernate.Validator.Cfg.Environment.SharedEngineProvider = provider;
        }
    }
}
