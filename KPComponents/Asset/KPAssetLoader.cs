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

using KPEnumerator.KPComponents;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// Asset Loader, render the HTML for a kind of Asset, JAVASCRIPT or STYLESHEET
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// <seealso cref="System.Web.UI.WebControls.WebControl"/>
    /// </summary>
    [ToolboxData(@"<{0}:KPAssetLoader runat=""server"" Source=""FieldName"" Type=""KPAssetTypeEnum.JAVASCRIPT"" />")]
    public class KPAssetLoader : WebControl
    {
        /// <summary>
        /// The full path to the file, from the first folder to the file.
        /// P.S.: Put the symbol "~" infrom of the URL, if you want to start from Base directory.
        /// </summary>
        /// <example>
        /// <code lang="html" title="ASPX">
        /// &lt;!-- If I want to load the file "Main.css", who is in the folder "Style" inner the project "WebProject" --&gt;
        /// &lt;KP:KPAssetLoader runat="server" Source="~/Styles/Main.css" Type="STYLESHEET" /&gt;
        /// </code>
        /// </example>
        public string Source { get; set; }

        /// <summary>
        /// The type of the source file, KPAssetTypeEnum.JAVASCRIPT or KPAssetTypeEnum.STYLESHEET (css)
        /// </summary>
        /// <example>
        /// <code lang="html" title="ASPX">
        /// &lt;!-- If I want to load the file "Main.css", who is in the folder "Styles" inner the project "WebProject" --&gt;
        /// &lt;KP:KPAssetLoader runat="server" Source="~/Styles/Main.css" Type="STYLESHEET" /&gt;
        /// 
        /// &lt;!-- If I want to load the file "Main.js", who is in the folder "Scripts" inner the project "WebProject" --&gt;
        /// &lt;KP:KPAssetLoader runat="server" Source="~/Scripts/Main.js" Type="JAVASCRIPT" /&gt;
        /// </code>
        /// </example>
        /// <seealso cref="KPEnumerator.KPComponents.KPAssetTypeEnum"/>
        public KPAssetTypeEnum Type { get; set; }

        /// <summary>
        /// Render the right form of the HTML for each KPAssetTypeEnum (css or js)
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the HTML</param>
        /// <seealso cref="System.Web.UI.HtmlTextWriter"/>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            StringBuilder sbComponent = new StringBuilder();

            string findUrlSource = Page.ResolveClientUrl(Source);

            switch (Type)
            {
                case KPAssetTypeEnum.STYLESHEET:
                    sbComponent.AppendFormat(@"<link rel=""Stylesheet"" href=""{0}"" type=""text/css""/>", findUrlSource);
                    break;

                default:
                case KPAssetTypeEnum.JAVASCRIPT:
                    sbComponent.AppendFormat(@"<script type=""text/javascript"" src=""{0}""></script>", findUrlSource);
                    break;
            }

            writer.Write(sbComponent.ToString());
        }
    }
}
