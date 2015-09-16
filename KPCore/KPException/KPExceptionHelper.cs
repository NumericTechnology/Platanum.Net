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
using System.Data.SqlClient;
using System.Text;

namespace KPCore.KPException
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPExceptionHelper
    {
        public static string GetCompleteError(Exception objError, bool breakLineWeb)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (objError != null)
            {
                stringBuilder.AppendLine("Message:");
                stringBuilder.AppendLine(KPExceptionHelper.GetMessageErrorRecursive(objError, breakLineWeb));
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Source:");
                stringBuilder.AppendLine(KPExceptionHelper.GetSourceErrorRecursive(objError, breakLineWeb));
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("StackTrace:");
                stringBuilder.AppendLine(KPExceptionHelper.GetStackTraceErrorRecursive(objError, breakLineWeb));
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
        public static string GetMessageErrorRecursive(Exception ex, bool breakLineWeb)
        {
            string text = string.Format("{0}{0}", Environment.NewLine);
            text = (breakLineWeb ? string.Format("{0}{0}", "<br>") : text);
            string text2 = string.Empty;
            if (ex != null)
            {
                text2 = text2 + ex.Message + text;
            }
            if (ex.InnerException != null)
            {
                text2 += KPExceptionHelper.GetMessageErrorRecursive(ex.InnerException, breakLineWeb);
            }
            return text2;
        }
        public static string GetSourceErrorRecursive(Exception ex, bool breakLineWeb)
        {
            string text = string.Format("{0}{0}", Environment.NewLine);
            text = (breakLineWeb ? string.Format("{0}{0}", "<br>") : text);
            string text2 = string.Empty;
            if (ex != null)
            {
                text2 = text2 + ex.Source + text;
            }
            if (ex.InnerException != null)
            {
                text2 += KPExceptionHelper.GetSourceErrorRecursive(ex.InnerException, breakLineWeb);
            }
            return text2;
        }
        public static string GetStackTraceErrorRecursive(Exception ex, bool breakLineWeb)
        {
            string text = string.Format("{0}{0}", Environment.NewLine);
            text = (breakLineWeb ? string.Format("{0}{0}", "<br>") : text);
            string text2 = string.Empty;
            if (ex != null)
            {
                text2 = text2 + ex.StackTrace + text;
            }
            if (ex.InnerException != null)
            {
                text2 += KPExceptionHelper.GetStackTraceErrorRecursive(ex.InnerException, breakLineWeb);
            }
            return text2;
        }

        public static Exception GetCustomException(Exception ex)
        {
            var sqlException = GetTypedException<SqlException>(ex);

            if (sqlException != null)
            {
                if (sqlException.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (sqlException.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return new KPExceptionSqlForeignKey("Foreign Key Error", sqlException);
                    }
                }
            }

            return ex;
        }

        public static T GetTypedException<T>(Exception ex) where T : Exception
        {
            if (ex is T)
                return (T)ex;

            if (ex.InnerException != null)
                return GetTypedException<T>(ex.InnerException);

            return default(T);
        }
    }
}