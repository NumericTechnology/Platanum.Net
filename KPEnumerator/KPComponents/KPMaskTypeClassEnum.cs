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

namespace KPEnumerator.KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public enum KPMaskTypeClassEnum
    {
        [MaskCharacteresEnum(new string[] { })]
        [TypeValue("AlphanumericField")]
        ALPHANUMERIC,
        [MaskCharacteresEnum(new string[] { ".", "-", "/" })]
        [TypeValue("CGCField")]
        CGC,
        [MaskCharacteresEnum(new string[] { ".", "-", "/" })]
        [TypeValue("CNPJField")]
        CNPJ,
        [MaskCharacteresEnum(new string[] { ".", "-" })]
        [TypeValue("CPFField")]
        CPF,
        [MaskCharacteresEnum(new string[] { })]
        [TypeValue("DateField")]
        DATE,
        [MaskCharacteresEnum(new string[] { })]
        [TypeValue("DateHourField")]
        DATEHOUR,
        [MaskCharacteresEnum(new string[] { })]
        [TypeValue("DateMinuteField")]
        DATEMINUTE,
        [MaskCharacteresEnum(new string[] { })]
        [TypeValue("DateTimeField")]
        DATETIME,
        [MaskCharacteresEnum(new string[] { "." })]
        [TypeValue("DecimalField")]
        DECIMAL,
        [MaskCharacteresEnum(new string[] { })]
        [TypeValue("IntegerField")]
        INTEGER,
        [MaskCharacteresEnum(new string[] { "-" })]
        [TypeValue("LicensePlateField")]
        LICENSEPLATE,
        [MaskCharacteresEnum(new string[] { "-" })]
        [TypeValue("PostCodeField")]
        POSTCODE,
        [MaskCharacteresEnum(new string[] { "(", ")", "-", " " })]
        [TypeValue("TelephoneField")]
        TELEPHONE,
        [MaskCharacteresEnum(new string[] { })]
        [TypeValue("TimeField")]
        TIME,
        [MaskCharacteresEnum(new string[] { ".", "%" })]
        [TypeValue("PercentField")]
        PERCENT,
        [MaskCharacteresEnum(new string[] { ".", "R", "$", " " })]
        [TypeValue("MoneyField")]
        MONEY,
    }
}
