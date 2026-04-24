using FluentAssertions;
using LocalGov.Umbraco.Core.Extensions;
using Umbraco.Cms.Core.Models;

namespace LocalGov.Umbraco.Core.Tests;

public class LocalGovContentHelperTests
{
    // IsValidLink does not call UmbracoHelper, so null is safe here.
    private readonly LocalGovContentHelper _sut = new(null!);

    [Fact]
    public void IsValidLink_NullLink_ReturnsFalse()
    {
        _sut.IsValidLink(null).Should().BeFalse();
    }

    [Theory]
    [InlineData("", "Link text")]
    [InlineData("   ", "Link text")]
    public void IsValidLink_EmptyOrWhitespaceUrl_ReturnsFalse(string url, string name)
    {
        var link = new Link { Url = url, Name = name };
        _sut.IsValidLink(link).Should().BeFalse();
    }

    [Theory]
    [InlineData("/services", "")]
    [InlineData("/services", "   ")]
    public void IsValidLink_EmptyOrWhitespaceName_ReturnsFalse(string url, string name)
    {
        var link = new Link { Url = url, Name = name };
        _sut.IsValidLink(link).Should().BeFalse();
    }

    [Fact]
    public void IsValidLink_ValidInternalLink_ReturnsTrue()
    {
        var link = new Link { Url = "/pay-council-tax", Name = "Pay council tax" };
        _sut.IsValidLink(link).Should().BeTrue();
    }

    [Fact]
    public void IsValidLink_ValidExternalLink_ReturnsTrue()
    {
        var link = new Link { Url = "https://www.gov.uk", Name = "GOV.UK" };
        _sut.IsValidLink(link).Should().BeTrue();
    }
}
