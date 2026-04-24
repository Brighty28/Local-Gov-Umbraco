using LocalGov.Umbraco.Core.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.Theme.TagHelpers;

/// <summary>
/// Injects per-council CSS custom properties from lgSettings into a style tag.
/// Usage: &lt;localgov-theme /&gt; in _Layout.cshtml &lt;head&gt;
/// </summary>
[HtmlTargetElement("localgov-theme")]
public class ThemeTagHelper(ILocalGovContentHelper contentHelper) : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "style";
        output.Attributes.SetAttribute("id", "localgov-theme");

        var settings = contentHelper.FindByDocumentType("lgSettings");

        var primary = settings?.Value<string>("primaryColour") ?? "#1d70b8";
        var secondary = settings?.Value<string>("secondaryColour") ?? "#003078";
        var linkColour = settings?.Value<string>("linkColour") ?? "#1d70b8";

        output.Content.SetHtmlContent(
            $":root {{{Environment.NewLine}" +
            $"    --localgov-primary-colour: {Sanitise(primary)};{Environment.NewLine}" +
            $"    --localgov-secondary-colour: {Sanitise(secondary)};{Environment.NewLine}" +
            $"    --localgov-link-colour: {Sanitise(linkColour)};{Environment.NewLine}" +
            "}");
    }

    private static string Sanitise(string value) =>
        value.StartsWith('#') && value.Length is 4 or 7
            ? value
            : "#1d70b8";
}
