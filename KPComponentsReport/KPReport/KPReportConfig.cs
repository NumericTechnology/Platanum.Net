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

namespace KPComponents.Report.KPReport
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class KPReportConfig
    {
        public string RelativeReportFile { get; protected set; }
        public List<KPReportDataSource> DataSources { get; protected set; }
        public List<KPReportParameter> Parameters { get; protected set; }

        public KPReportConfig(string relativeReportFile)
        {
            DataSources = new List<KPReportDataSource>();
            Parameters = new List<KPReportParameter>();
            RelativeReportFile = relativeReportFile;
        }

        public KPReportConfig(string relativeReportFile, KPReportDataSource[] dataSource)
            : this(relativeReportFile)
        {
            if (dataSource != null)
            {
                DataSources.AddRange(dataSource);
            }
        }

        public void AddDataSource(KPReportDataSource dataSource)
        {
            DataSources.Add(dataSource);
        }

        public void AddParameter(KPReportParameter parameter)
        {
            Parameters.Add(parameter);
        }

        public ReportDataSource[] GetReportDataSources()
        {
            List<ReportDataSource> reportDataSourceList = new List<ReportDataSource>();
            foreach (KPReportDataSource item in DataSources)
            {
                reportDataSourceList.Add(new ReportDataSource(item.Name, item.DataSourceValue));
            }

            return reportDataSourceList.ToArray();
        }

        public ReportParameter[] GetReportParameters()
        {
            List<ReportParameter> reportParameterList = new List<ReportParameter>();
            foreach (KPReportParameter item in Parameters)
            {
                reportParameterList.Add(new ReportParameter(item.Name, item.Values, item.Visible));
            }

            return reportParameterList.ToArray();
        }

    }
}
