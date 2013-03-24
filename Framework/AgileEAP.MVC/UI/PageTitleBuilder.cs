﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.MVC.UI
{
    public class PageTitleBuilder : IPageTitleBuilder
    {
        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly List<string> _metaKeywordParts;
        private readonly List<string> _scriptParts;
        private readonly List<string> _cssParts;
        private readonly List<string> _canonicalUrlParts;

        public PageTitleBuilder()
        {
            this._titleParts = new List<string>();
            this._metaDescriptionParts = new List<string>();
            this._metaKeywordParts = new List<string>();
            this._scriptParts = new List<string>();
            this._cssParts = new List<string>();
            this._canonicalUrlParts = new List<string>();
        }

        public void AddTitleParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _titleParts.Add(part);
        }
        public void AppendTitleParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _titleParts.Insert(0, part);
        }
        public string GenerateTitle(bool addDefaultTitle)
        {
            var result = string.Join(",", _titleParts.AsEnumerable().Reverse().ToArray());
            return result;
        }


        public void AddMetaDescriptionParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaDescriptionParts.Add(part);
        }
        public void AppendMetaDescriptionParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaDescriptionParts.Insert(0, part);
        }
        public string GenerateMetaDescription()
        {
            var metaDescription = string.Join(", ", _metaDescriptionParts.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaDescription) ? metaDescription : "";// _seoSettings.DefaultMetaDescription;
            return result;
        }


        public void AddMetaKeywordParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaKeywordParts.Add(part);
        }
        public void AppendMetaKeywordParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _metaKeywordParts.Insert(0, part);
        }
        public string GenerateMetaKeywords()
        {
            var metaKeyword = string.Join(", ", _metaKeywordParts.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaKeyword) ? metaKeyword : "";// _seoSettings.DefaultMetaKeywords;
            return result;
        }


        public void AddScriptParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _scriptParts.Add(part);
        }
        public void AppendScriptParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _scriptParts.Insert(0, part);
        }
        public string GenerateScripts()
        {
            var result = new StringBuilder();
            foreach (var scriptPath in _scriptParts)
            {
                result.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", scriptPath);
                result.Append(System.Environment.NewLine);
            }
            return result.ToString();
        }


        public void AddCssFileParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _cssParts.Add(part);
        }
        public void AppendCssFileParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _cssParts.Insert(0, part);
        }
        public string GenerateCssFiles()
        {
            var result = new StringBuilder();
            foreach (var cssPath in _cssParts)
            {
                result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssPath);
                result.Append(System.Environment.NewLine);
            }
            return result.ToString();
        }


        public void AddCanonicalUrlParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _canonicalUrlParts.Add(part);
        }
        public void AppendCanonicalUrlParts(params string[] parts)
        {
            if (parts != null)
                foreach (string part in parts)
                    if (!string.IsNullOrEmpty(part))
                        _canonicalUrlParts.Insert(0, part);
        }
        public string GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(System.Environment.NewLine);
            }
            return result.ToString();
        }
    }
}
