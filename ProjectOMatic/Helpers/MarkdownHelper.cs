using Markdig;
using Microsoft.AspNetCore.Html;

namespace ProjectOMatic.Helpers
{
    public static class MarkdownHelper
    {
        public static string Parse(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            return Markdown.ToHtml(markdown, pipeline);
        }

        public static HtmlString ParseHtmlString(string markdown, bool usePragmaLines = false, bool forceReload = false)
        {
            return new HtmlString(Parse(markdown));
        }
    }
}
