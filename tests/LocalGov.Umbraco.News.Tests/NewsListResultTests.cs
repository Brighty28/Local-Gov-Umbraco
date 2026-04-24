using FluentAssertions;
using LocalGov.Umbraco.News.Services;

namespace LocalGov.Umbraco.News.Tests;

public class NewsListResultTests
{
    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(1, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(25, 10, 3)]
    [InlineData(100, 10, 10)]
    [InlineData(1, 5, 1)]
    [InlineData(6, 5, 2)]
    public void TotalPages_CorrectlyCalculatesCeilingDivision(int totalItems, int pageSize, int expectedPages)
    {
        var result = new NewsListResult([], totalItems, 1, pageSize);
        result.TotalPages.Should().Be(expectedPages);
    }

    [Fact]
    public void TotalPages_SingleItem_ReturnsOne()
    {
        var result = new NewsListResult([], 1, 1, 10);
        result.TotalPages.Should().Be(1);
    }
}
