﻿using Ozzy.Umbraco.DocumentTypes;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Ozzy.Umbraco.ViewModels
{
    public class BaseViewModel
    {
        public IPageBase Content { get; set; }
        public IHtmlString Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }                       

        public BaseViewModel(IPageBase content)        
        {
            Content = content;
            Title = new HtmlString(content.Name);
            MetaDescription = content.Description;
            MetaKeywords = content.Keywords;
        }
    }
}
