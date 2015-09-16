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
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace KPCore.SerializerMap
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    [XmlRoot("KPConfiguration")]
    public class KPConfiguration
    {
        private KPGridView kpGridView;

        [XmlElement("KPGridView", typeof(KPGridView))]
        public KPGridView KPGridView
        {
            get { return kpGridView; }
            set { kpGridView = value; }
        }
    }

    [Serializable]
    [XmlType("config")]
    public class KPGridView
    {
        private int maxRows;
        private bool allowPaging;
        private KPImages kpImages;
        private KPPaging kpPaging;
        private KPStyle kpStyle;

        [XmlElement("MaxRows")]
        [Description("Limite de linhas retornadas no GridView")]
        public int MaxRows
        {
            get { return maxRows; }
            set { maxRows = value; }
        }

        [XmlElement("AllowPaging")]
        [Description("Ativa paginação do Grid")]
        public bool AllowPaging
        {
            get { return allowPaging; }
            set { allowPaging = value; }
        }

        [XmlElement("KPImages")]
        [Description("Imagens do GridView")]
        public KPImages KPImages
        {
            get { return kpImages; }
            set { kpImages = value; }
        }

        [XmlElement("KPPaging")]
        [Description("Paginação do GridView")]
        public KPPaging KPPaging
        {
            get { return kpPaging; }
            set { kpPaging = value; }
        }

        [XmlElement("KPStyle")]
        [Description("Styles do GridView")]
        public KPStyle KPStyle
        {
            get { return kpStyle; }
            set { kpStyle = value; }
        }
    }

    [Serializable]
    [XmlType("KPImages")]
    public class KPImages
    {
        private string ascendingSort;
        private string descendingSort;
        private string pagingImageFirst;
        private string pagingImagePrevious;
        private string pagingImageNext;
        public string pagingImageLast;

        [XmlElement("AscendingSort")]
        [Description("Imagem de que representará ordenação crescente")]
        public string AscendingSort
        {
            get { return ascendingSort; }
            set { ascendingSort = value; }
        }

        [XmlElement("DescendingSort")]
        [Description("Imagem de que representará ordenação decrescente")]
        public string DescendingSort
        {
            get { return descendingSort; }
            set { descendingSort = value; }
        }

        [XmlElement("PagingImageFirst")]
        [Description("Imagem que aparecerá no lugar do botão '|<' (Primeiro).")]
        public string PagingImageFirst
        {
            get { return pagingImageFirst; }
            set { pagingImageFirst = value; }
        }

        [XmlElement("PagingImagePrevious")]
        [Description("Imagem que aparecerá no lugar do botão '<' (Anterior).")]
        public string PagingImagePrevious
        {
            get { return pagingImagePrevious; }
            set { pagingImagePrevious = value; }
        }

        [XmlElement("PagingImageNext")]
        [Description("Imagem que aparecerá no lugar do botão '>' (Próximo).")]
        public string PagingImageNext
        {
            get { return pagingImageNext; }
            set { pagingImageNext = value; }
        }

        [XmlElement("PagingImageLast")]
        [Description("Imagem que aparecerá no lugar do botão '>|' (Último).")]
        public string PagingImageLast
        {
            get { return pagingImageLast; }
            set { pagingImageLast = value; }
        }
    }

    [Serializable]
    [XmlType("KPPaging")]
    public class KPPaging
    {
        private HorizontalAlign horizontalAlign;
        private int position;
        private int maxPages;

        [XmlElement("HorizontalAlign")]
        [Description("Posição Horizontal da Paginação")]
        public HorizontalAlign HorizontalAlignProperty
        {
            get { return horizontalAlign; }
            set { horizontalAlign = value; }
        }

        [XmlElement("Position")]
        [Description("Posição")]
        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        [XmlElement("MaxPages")]
        [Description("Quantidade máxima de Páginas")]
        public int MaxPages
        {
            get { return maxPages; }
            set { maxPages = value; }
        }

        #region Enumerators
        public enum HorizontalAlign
        {
            [Description("Center")]
            Center,
        }
        #endregion
    }

    [Serializable]
    [XmlType("KPStyle")]
    public class KPStyle
    {
        private ColorLine colorLine;
        private string cssClassGridView;

        [XmlElement("ColorLine")]
        [Description("Cor das Linhas do GridView")]
        public ColorLine ColorLine
        {
            get { return colorLine; }
            set { colorLine = value; }
        }

        [XmlElement("CssClassGridView")]
        [Description("CSS GridView")]
        public string CssClassGridView
        {
            get { return cssClassGridView; }
            set { cssClassGridView = value; }
        }
    }

    [Serializable]
    [XmlType("ColorLine")]
    public class ColorLine
    {
        private string selectedLine;
        private string intercalatedColor;
        private string rowCssClass;

        [XmlElement("SelectedLine")]
        [Description("Cor da Linha")]
        public string SelectedLine
        {
            get { return selectedLine; }
            set { selectedLine = value; }
        }

        [XmlElement("IntercalatedColor")]
        [Description("Cor alteranada da Linha")]
        public string IntercalatedColor
        {
            get { return intercalatedColor; }
            set { intercalatedColor = value; }
        }

        [XmlElement("RowCssClass")]
        [Description("CSS da Linha")]
        public string RowCssClass
        {
            get { return rowCssClass; }
            set { rowCssClass = value; }
        }
    }
}
