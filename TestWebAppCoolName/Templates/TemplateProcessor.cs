using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TestWebAppCoolName.Templates
{
    public class TemplateProcessor 
    {
        private object _model;
        private Regex _propertyRegex;

        public TemplateProcessor()
        {
            _propertyRegex = new Regex(@"(?<=\@\{)(.*?)(?=\})");
        }

        public string GetContentFromTemplate(string template, object model)
        {
            _model = model;
            return Regex.Replace(template, @"\@\{.*\}", GetMatchPropertyValue);
        }
        internal string GetMatchPropertyValue(Match match)
        {
            var property = match.Value;
            return GetPropertyValueString(property.Substring(2, property.Length - 3));
        }

        internal string GetPropertyValueString(string propertyName)
        {
            return _model.GetType().GetProperty(propertyName).GetValue(_model).ToString();
        }
    }
}