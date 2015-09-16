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

using KPCore.KPException;
using KPCore.KPSecurity;
using KPCore.KPValidator;
using KPEntity.ORM;
using KPEnumerator.KPGlobalization;
using KPEnumerator.KPSecurity;
using KPExtension;
using KPGlobalization;
using System;
using System.Web;

namespace KPComponents.KPSession
{
    /// <summary>
    /// Provide Static Methods for manipulate Sessions Framework
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public static class KPSessionHelper
    {
        #region Public Methods

        /// <summary>
        /// Get the object Session through Key Enum
        /// </summary>
        /// <seealso cref="KPEnumerator.KPSecurity.KPSessionKeyEnum"/>
        /// <param name="sessionKey">Enum Key</param>
        /// <exception cref="KPCore.KPException.KPExceptionSession">Thrown when the session was not found</exception>
        /// <returns>Object Session</returns>
        public static object GetSession(KPSessionKeyEnum sessionKey)
        {
            try
            {
                object sessionValue = HttpContext.Current.Session[sessionKey.GetTypeValue().ToString()];
                if (sessionValue != null)
                    return sessionValue;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(sessionKey, KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_NOT_FOUND), ex);
            }

            return null;
        }

        /// <summary>
        /// Get the object Session through Key Enum using cast generic type
        /// </summary>
        /// <seealso cref="KPEnumerator.KPSecurity.KPSessionKeyEnum"/>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="sessionKey">Enum Key</param>
        /// <returns>Object Session Converted by Generic Type or default(T) if not found</returns>
        public static T GetSession<T>(KPSessionKeyEnum sessionKey)
        {
            object sessionValue = GetSession(sessionKey);
            if (sessionValue != null)
                return (T)sessionValue;

            return default(T);
        }

        /// <summary>
        /// Set object Session through the Enum Key
        /// </summary>
        /// <seealso cref="KPEnumerator.KPSecurity.KPSessionKeyEnum"/>
        /// <exception cref="KPCore.KPException.KPExceptionSession">Was not possible to set the session</exception>
        /// <param name="sessionKey">Enum Key</param>
        /// <param name="value">Object value</param>
        public static void SetSession(KPSessionKeyEnum sessionKey, object value)
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    HttpContext.Current.Session[sessionKey.GetTypeValue().ToString()] = value;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(sessionKey, KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_NOT_SET), ex);
            }
        }




        /// <summary>
        /// Remove object from Session by KPSessionKeyEnum
        /// </summary>
        /// <seealso cref="KPEnumerator.KPSecurity.KPSessionKeyEnum"/>
        /// <param name="kpSessionKeyEnum">Enum key</param>
        public static void RemoveSession(KPSessionKeyEnum kpSessionKeyEnum)
        {
            try
            {
                SetSession(kpSessionKeyEnum, null);
                HttpContext.Current.Session.Remove(kpSessionKeyEnum.GetTypeValue().ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #region SessionMasterDetail
        internal static void SetSessionMasterDetailList(string sessionPageID, string masterDetailID, DetailSession value)
        {
            string keySession = String.Format("{0}_{1}_{2}", KPSessionKeyEnum.SESSION_MASTER_DETAIL_LIST.ToString(), sessionPageID, masterDetailID);
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    HttpContext.Current.Session[keySession] = value;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(keySession, KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_NOT_SET), ex);
            }
        }

        public static DetailSession GetSessionMasterDetailList(string sessionPageID, string masterDetailID)
        {
            string keySession = String.Format("{0}_{1}_{2}", KPSessionKeyEnum.SESSION_MASTER_DETAIL_LIST.ToString(), sessionPageID, masterDetailID);
            try
            {
                object sessionValue = HttpContext.Current.Session[keySession];
                if (sessionValue != null)
                    return sessionValue as DetailSession;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(keySession, KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_NOT_FOUND), ex);
            }

            return null;
        }

        internal static void RemoveSessionMasterDetailList(string sessionPageID, string masterDetailID)
        {
            string keySession = String.Format("{0}_{1}_{2}", KPSessionKeyEnum.SESSION_MASTER_DETAIL_LIST.ToString(), sessionPageID, masterDetailID);
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    HttpContext.Current.Session.Remove(keySession);
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(keySession, KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_NOT_FOUND), ex);
            }
        }

        internal static int GetSessionDetailTemporayId(string sessionPageID, string masterDetailID)
        {
            string keySession = String.Format("{0}_{1}_{2}", KPSessionKeyEnum.SESSION_DETAIL_TEMPORARY_ID.ToString(), sessionPageID, masterDetailID);
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    object sessionValue = HttpContext.Current.Session[keySession];
                    if (sessionValue != null)
                    {
                        HttpContext.Current.Session[keySession] = Convert.ToInt32(sessionValue) - 1;
                        return Convert.ToInt32(Convert.ToInt32(sessionValue) - 1);
                    }
                    else
                        HttpContext.Current.Session[keySession] = -1;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(keySession, KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_NOT_SET), ex);
            }
        }
        #endregion SessionMasterDetail

        #region SessionData
        /// <summary>
        /// Set KPGridControl selected row into the Session
        /// </summary>
        /// <seealso cref="KPGridControl"/>
        /// <seealso cref="KPSession.KPSessionData"/>
        /// <exception cref="KPCore.KPException.KPExceptionSession">Thrown when the session was not found</exception>
        /// <exception cref="KPCore.KPException.KPExceptionSession">The session object is not an Entity</exception>
        /// <param name="sessionData">Complex object with KPGridControl information</param>
        /// <param name="sessionPageID">Page Session ID</param>
        public static void SetSessionData(KPSessionData sessionData, string sessionPageID)
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null && 
                            sessionData != null && sessionData.Entity != null && !String.IsNullOrWhiteSpace(sessionPageID))
                {
                    if (sessionData.Entity.GetType().BaseType.GetGenericTypeDefinition() == (typeof(KPActiveRecordBase<>)).GetGenericTypeDefinition())
                    {
                        string keySession = String.Format("{0}_{1}", KPSessionKeyEnum.SESSION_GRID_ROW_SELECTED_DATA.GetTypeValue(), sessionPageID);
                        HttpContext.Current.Session[keySession] = sessionData;
                    }
                    else
                        throw new Exception(KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_OBJECT_NOT_ENTITY));
                }
                else
                    throw new KPExceptionSession(KPSessionKeyEnum.SESSION_GRID_ROW_SELECTED_DATA, KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SESSION_NOT_FOUND));
            }
            catch (KPExceptionSession)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(KPSessionKeyEnum.SESSION_GRID_ROW_SELECTED_DATA, ex.Message, ex);
            }
        }

        /// <summary>
        /// Remove object KPGridControl row from Session by Entity Type
        /// </summary>
        /// <param name="sessionPageID">Page Session ID</param>
        public static void RemoveSessionData(string sessionPageID)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(sessionPageID))
                    return;

                string keySession = String.Format("{0}_{1}", KPSessionKeyEnum.SESSION_GRID_ROW_SELECTED_DATA.GetTypeValue(), sessionPageID);
                SetSession(keySession, null);
                HttpContext.Current.Session.Remove(keySession);
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(KPSessionKeyEnum.SESSION_GRID_ROW_SELECTED_DATA, ex.Message, ex);
            }
        }

        /// <summary>
        /// Get object [row selected] on KPGridControl from Session by Type Entity
        /// </summary>
        /// <param name="sessionPageID">Page Session ID</param>
        /// <returns>Complex object with KPGridControl information</returns>
        public static KPSessionData GetSessionData(string sessionPageID)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(sessionPageID))
                    return null;

                string keySession = String.Format("{0}_{1}", KPSessionKeyEnum.SESSION_GRID_ROW_SELECTED_DATA.GetTypeValue(), sessionPageID);
                object sessionValue = GetSession(keySession);
                if (sessionValue != null)
                    return (KPSessionData)sessionValue;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(KPSessionKeyEnum.SESSION_GRID_ROW_SELECTED_DATA, ex.Message, ex);
            }

            return null;
        }
        #endregion SessionData

        /// <summary>
        /// Get current company logged on Session
        /// </summary>
        /// <seealso cref="KPEntity.ORM.FrwCompany"/>
        /// <returns>Object Entity FrwCompany</returns>
        public static FrwCompany GetCurrentCompany()
        {
            try
            {
                FrwCompany frwCompany = null;
                KPSecuritySession session = KPSessionHelper.GetSession<KPSecuritySession>(KPSessionKeyEnum.SESSION_LOGIN);
                if (session != null)
                    frwCompany = FrwCompany.Find(session.FrwCompany);

                return frwCompany;
            }
            catch (Exception ex)
            {
                throw new KPExceptionSession(KPSessionKeyEnum.SESSION_LOGIN, ex.Message, ex);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get object Session by string key
        /// </summary>
        /// <param name="keySession">String session key</param>
        /// <returns>Object in Session ou Null when not found</returns>
        private static object GetSession(string keySession)
        {
            try
            {
                object sessionValue = HttpContext.Current.Session[keySession];
                if (sessionValue != null)
                    return sessionValue;
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        /// <summary>
        /// Set object on Session
        /// </summary>
        /// <param name="keySession">String Session Key</param>
        /// <param name="value">Object value</param>
        private static void SetSession(string keySession, object value)
        {
            try
            {
                HttpContext.Current.Session[keySession] = value;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
