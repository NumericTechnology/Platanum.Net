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
using System.IO;
using Microsoft.Reporting.WebForms;
using KPComponents.Report.KPReport;
using KPCore.KPValidator;

namespace KPComponents.KPReport
{
    /// <summary>
    /// Configuration Object Report Entity
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <typeparam name="Entity">Generic Object Entity</typeparam>
    [Serializable]
    public class KPReportEntityConfig<Entity> : KPReportConfig where Entity : KPActiveRecordBase<Entity>
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="relativeReportFile">Relative Path Report File</param>
        /// <param name="dataSources">Array dataSources for include into Report</param>
        public KPReportEntityConfig(string relativeReportFile, KPReportDataSource[] dataSources)
            : base(relativeReportFile, dataSources)
        {
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="relativeReportFile">Relative Path Report File</param>
        public KPReportEntityConfig(string relativeReportFile)
            : base(relativeReportFile)
        {
        }

        /// <summary>
        /// Add data sources Entity into Report
        /// </summary>
        /// <param name="dataSourceName">DataSource key</param>
        /// <param name="Entities">Array of Entities</param>
        public void AddDataSource(string dataSourceName, Entity[] Entities)
        {
            List<Entity> entityList = new List<Entity>(Entities);
            KPReportDataSource ds = new KPReportDataSource(dataSourceName, entityList);
            DataSources.Add(ds);
        }
    }
}
