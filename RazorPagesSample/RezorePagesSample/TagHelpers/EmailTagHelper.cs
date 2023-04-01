using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace RezorePagesSample.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        private const string EmailDomain = "example.com";

        public string? MailTo { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            if (string.IsNullOrWhiteSpace(MailTo))
            {
                var content = await output.GetChildContentAsync();
                var address = content.GetContent() + "@" + EmailDomain;
                output.Attributes.SetAttribute("href", "mailto:" + address);
                output.Content.SetContent(address);
            }
            else
            {
                var address = MailTo + "@" + EmailDomain;
                output.Attributes.SetAttribute("href", "mailto:" + address);
            }

            output.Attributes.SetAttribute("style", "color: blue; display: block;");            
        }
    }
}
