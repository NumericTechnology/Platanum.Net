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

using KPBO.Parameters;
using KPCore.KPException;
using KPData;
using KPEnumerator.KPSecurity;
using KPExtension;
using NHibernate.Validator.Cfg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Compilation;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public abstract class KPGlobal : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            CultureInfo culture;
            if (Thread.CurrentThread.CurrentCulture.Name != "pt-BR")
            {
                culture = CultureInfo.CreateSpecificCulture("pt-BR");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            DataFramework dataFrw = new DataFramework();
            //string assemblyNameEntity = FrwParametersHelper.GetValueParam<string>(FrwParamEnum.FRW_ASSEMBLY_NAME_ENTITY);
            string assemblyNameEntity = "SpecialistEntity";
            Assembly assemblyEntity = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == assemblyNameEntity);
            if (assemblyEntity == null)
            {
                assemblyEntity = Assembly.LoadFrom(Path.Combine(System.Web.HttpRuntime.BinDirectory, String.Format("{0}.dll", assemblyNameEntity)));
                if (assemblyEntity == null)
                    throw new Exception(String.Format("Assembly '{0}' not found.", assemblyNameEntity));
            }
            dataFrw.InitializeDatabases(new Assembly[] { dataFrw.KPEntity, assemblyEntity });
            DataFramework.InicializeValidator(LanguageEnum.PORTUGUESE_BRAZIL);

            #region Atualizar Parâmetros
            FrwParametersHelper.UpdateDefaultParamValueFrw();
            #endregion

            ApplicationStart(sender, e);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (HttpContext.Current != null)
            {
                HttpContext _context = HttpContext.Current;

                Uri fileError = ((HttpApplication)sender).Context.Request.Url;
#if DEBUG
                Debugger.Break();
#endif
                if (_context.Session != null)
                {
                    KPExceptionSecurity exSecurity = null;
                    try
                    {
                        exSecurity = KPExceptionHelper.GetTypedException<KPExceptionSecurity>(ex);
                    }
                    catch { }

                    if (exSecurity != null)
                    {
                        _context.Session.Add(KPSessionKeyEnum.SESSION_EXCEPTION.ToString(), exSecurity);
                        HttpContext.Current.Response.Redirect("~/ErrorPermission.aspx");
                    }
                    else
                    {
                        _context.Session.Add(KPSessionKeyEnum.SESSION_EXCEPTION.ToString(), ex);
                        HttpContext.Current.Response.Redirect("~/Error.aspx");
                    }
                }
                else
                    _context.Response.Write(KPExceptionHelper.GetCompleteError(ex as Exception, true));

                Server.ClearError();
            }

            ApplicationError(sender, e);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            SessionStart(sender, e);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            SessionEnd(sender, e);
        }

        protected abstract void ApplicationStart(object sender, EventArgs e);
        protected abstract void ApplicationError(object sender, EventArgs e);
        protected abstract void SessionStart(object sender, EventArgs e);
        protected abstract void SessionEnd(object sender, EventArgs e);
    }
}
