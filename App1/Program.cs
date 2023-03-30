using System.Net.Mime;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapGet("/", () => Results.Extensions.Html("""
<!doctype html>
<html>
    <head><title>Repro</title></head>
    <body>
        <div id="result"></div>
		<script>
			window.addEventListener("load", async () => {
				for(var i=2; i<=3; i++) {
					const url = "https://localhost:500"+i;
					try {
						const r = await fetch(url);
						const t = await r.text();
						document.getElementById('result').innerHTML += url+": Ok ("+t+")<br/>";
					} catch (e) {
						document.getElementById('result').innerHTML += url+": <span style=\"color: red\">Failed ("+e+")</span><br/>";
					}
				}
			});
		</script>
    </body>
</html>
"""));

app.Run();

static class ResultsExtensions
{
	public static IResult Html(this IResultExtensions resultExtensions, string html)
	{
		ArgumentNullException.ThrowIfNull(resultExtensions);

		return new HtmlResult(html);
	}
}

class HtmlResult : IResult
{
	private readonly string _html;

	public HtmlResult(string html)
	{
		_html = html;
	}

	public Task ExecuteAsync(HttpContext httpContext)
	{
		httpContext.Response.ContentType = MediaTypeNames.Text.Html;
		httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
		return httpContext.Response.WriteAsync(_html);
	}
}