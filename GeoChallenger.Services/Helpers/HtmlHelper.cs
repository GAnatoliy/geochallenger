using System;
using System.Collections.Generic;
using System.IO;
using Ganss.XSS;
using HtmlAgilityPack;


namespace GeoChallenger.Services.Helpers
{
    public static class HtmlHelper
    {
        // TODO: write tests.
        public static string SanitizeHtml(string html, IList<string> allowedAttributes = null)
        {
            if (string.IsNullOrEmpty(html)) {
                return string.Empty;
            }

            // Use only elements allowed in Imperavi Redactor
            var allowedElements = new List<string> {
                "code", "span", "div", "label", "a", "br", "p", "b", "i", "del", "strike", "u",
                "img", "video", "audio", "iframe", "object", "embed", "param", "blockquote", "mark",
                "cite", "small", "ul", "ol", "li", "hr", "dl", "dt", "dd", "sup", "sub", "big", "pre",
                "code", "figure", "figcaption", "strong", "em", "table", "tr", "td", "th", "tbody", "thead",
                "tfoot", "h1", "h2", "h3", "h4", "h5", "h6"
            };

            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.UnionWith(allowedElements);

            if (allowedAttributes != null) {
                sanitizer.AllowedAttributes.UnionWith(allowedAttributes);
            }

            return sanitizer.Sanitize(html);
        }

        // TODO: write testst.
        public static string ConvertToText(string html)
        {
            if (string.IsNullOrEmpty(html)) {
                return string.Empty;
            }

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var stringWriter = new StringWriter();
            ConvertTo(document.DocumentNode, stringWriter);
            stringWriter.Flush();

            // TODO: now we delete first /r/n, from string line (because our redactor wrap every new
            // line with <p> </p>, and our parser sets '/r/n' in front of every line),
            // consider to find more elegant way of parsinf from html to plain text
            return stringWriter.ToString().TrimStart();
        }

        // TODO: write testst.
        private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (var subnode in node.ChildNodes) {
                ConvertTo(subnode, outText);
            }
        }

        // TODO: write testst.
        private static void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType) {
                case HtmlNodeType.Comment:
                    // Don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // Script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // Get text
                    html = ((HtmlTextNode) node).Text;

                    // Is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // Check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0) {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name) {
                        case "p":
                            // Treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes) {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }
    }
}