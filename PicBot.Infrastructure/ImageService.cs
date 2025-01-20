using Newtonsoft.Json.Linq;
using PicBot.Application.Abstractions;
using PicBot.Domain.ImageService;

namespace PicBot.Infrastructure;

internal class ImageService(HttpClient httpClient) : IImageService
{
    private readonly Dictionary<string, string> _parameters = new()
    {
        ["tmpl_version"] = "releases%2Ffrontend%2Fimages%2Fv1.1468.0%23426e910ddcfcb72957d019b76950fb58f0d0cf20",
        ["format"] = "json",
        ["request"] = "%7B%22blocks%22%3A%5B%7B%22block%22%3A%22extra-content%22%2C%22params%22%3A%7B%7D%2C%22version%22%3A2%7D%2C%7B%22block%22%3A%7B%22block%22%3A%22i-react-ajax-adapter%3Aajax%22%7D%2C%22params%22%3A%7B%22type%22%3A%22ImagesApp%22%2C%22ajaxKey%22%3A%22serpList%2Ffetch%22%7D%2C%22version%22%3A2%7D%5D%7D",
        ["yu"] = "3856468921736584152",
        ["lr"] = "172",
        ["p"] = "1",
        ["rpt"] = "image",
        ["serpListType"] = "horizontal",
        ["serpid"] = "jI3pCy3CCkJtvg0aT-VWvg",
        ["uinfo"] = "sw-3440-sh-1440-ww-1098-wh-1265-pd-1-wp-16x10_2560x1600",
    };

    public async Task<List<ImageResponse>> GetImageByTextAsync(string text, CancellationToken cancellationToken)
    {
        _parameters.Add("text", Uri.EscapeDataString(text));
        var request = new HttpRequestMessage(HttpMethod.Get, GetUri("images/search"));

        var myResponse = await httpClient.SendAsync(request, cancellationToken);
        var myJsonResponse = await myResponse.Content.ReadAsStringAsync(cancellationToken);

        var root = (JContainer)JToken.Parse(myJsonResponse);
        var list = root.DescendantsAndSelf().OfType<JProperty>().Where(p => p.Name == "origUrl").Select(p => p.Value.Value<string>());

        return list
              .Select(
                   z => new ImageResponse
                   {
                       Url = z,
                       ThumbUrl = z,
                   })
              .ToList();
    }

    private Uri GetUri(string uriString)
    {
        var queryString = ToQueryString();
        var result = httpClient.BaseAddress + (Uri.IsWellFormedUriString(uriString, UriKind.Absolute) ? uriString : GetAbsolutePath(uriString)) + queryString;

        return new(result, UriKind.Absolute);
    }

    private static string GetAbsolutePath(string relativeUri)
        => relativeUri.EndsWith('/')
            ? relativeUri.Substring(startIndex: 1, relativeUri.Length - 1)
            : relativeUri;

    private string ToQueryString()
    {
        return _parameters != null
            ? "?"
              + string.Join(
                  "&",
                  _parameters
                     .Where(p => !string.IsNullOrEmpty(p.Value))
                     .Select(p => p.Key + "=" + p.Value))
            : null;
    }
}