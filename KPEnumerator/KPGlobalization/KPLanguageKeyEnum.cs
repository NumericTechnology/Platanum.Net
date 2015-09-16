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
 
using KPAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPEnumerator.KPGlobalization
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public enum KPLanguageKeyEnum
    {
        #region General

        // en: Information
        // pt-BR: Informação
        [ResourceKey("General_Info")]
        INFO,

        // en: Warning
        // pt-BR: Atenção
        [ResourceKey("General_Warning")]
        WARNING,

        // en: Error
        // pt-BR: Erro
        [ResourceKey("General_Error")]
        ERROR,

        // en: "Value is not valid!"
        // pt-BR: "Valor inválido!"
        [ResourceKey("General_InvalidValue")]
        INVALID_VALUE,


        // en: "Error Solution Suggestion: {0}"
        // pt-BR: "Sugestão de Solução de erro: {0}"
        [ResourceKey("General_ErrorSolutionSuggestion")]
        ERROR_SOLUTION_SUGGESTION,

        // en: "Verify property name: {0} with the Entity.{1}"
        // pt-BR: "Verificar nome da Propriedade: {0} com a Entidade.{1}"
        [ResourceKey("General_SuggestionVerifyPropertyName")]
        SUGGESTION_VERIFY_PROPERTY_NAME,
        #endregion General

        #region KPComponents
        

        // Juliano
        // en: "Session was not found!"
        // pt-BR: "Seção não foi encontrada!"
        [ResourceKey("KPComponents_SessionNotFound")]
        SESSION_NOT_FOUND,
	
        // en: "Was not possible to set the session!"
        // pt-BR: "Não foi possível salvar a sessão!"
        [ResourceKey("KPComponents_SessionNotSet")]
        SESSION_NOT_SET,
	
        // en: "The session object is not Entity!"
        // pt-BR: "O objeto da sessão não é uma Entidade!"
        [ResourceKey("KPComponents_SessionObjectNotEntity")]
        SESSION_OBJECT_NOT_ENTITY,

        // en: "Security Failure"
        // pt-BR: "Falha de Segurança"
        [ResourceKey("KPComponents_SecurityFail")]
        SECURITY_FAILURE,

        // en: "Incorrect User or Password!"
        // pt-BR: "Usuário ou Senha incorretos!"
        [ResourceKey("KPComponents_IncorrectUserOrPassword")]
        INCORRECT_USER_OR_PASSWORD,

        #endregion KPComponents
    }
}
