using FluentAssertions;
using LocalGov.Umbraco.Core.Extensions;
using LocalGov.Umbraco.News.Services;
using Moq;
using IPublishedContent = global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent;

namespace LocalGov.Umbraco.News.Tests;

public class NewsServiceTests
{
    private readonly Mock<ILocalGovContentHelper> _contentHelper = new();
    private readonly NewsService _sut;

    public NewsServiceTests()
    {
        _contentHelper.Setup(h => h.GetNewsList()).Returns((IPublishedContent?)null);
        _sut = new NewsService(_contentHelper.Object);
    }

    [Fact]
    public void GetNews_WhenNoNewsListExists_ReturnsEmptyResult()
    {
        var result = _sut.GetNews();

        result.Items.Should().BeEmpty();
        result.TotalItems.Should().Be(0);
    }

    [Fact]
    public void GetNews_WhenNoNewsListExists_PreservesRequestedPage()
    {
        var result = _sut.GetNews(page: 3, pageSize: 5);

        result.CurrentPage.Should().Be(3);
        result.PageSize.Should().Be(5);
    }

    [Fact]
    public void GetLatestNews_WhenNoNewsListExists_ReturnsEmpty()
    {
        var result = _sut.GetLatestNews(10);

        result.Should().BeEmpty();
    }
}
