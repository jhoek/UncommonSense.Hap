# ConvertTo-HtmlDocument https://www.nu.nl
# | Select-HtmlNode -XPath '//head'
# | Select-HtmlNode -XPath 'title'
# | Select-Object -ExpandProperty InnerText

Invoke-WebRequest 'https://www.volkskrant.nl/columns/Sylvia-Witteman'
| Select-Object -ExpandProperty Content
| ConvertTo-HtmlDocument
| Select-HtmlNode -CssSelector '.teaser--compact h3' -All
| Select-Object -ExpandProperty InnerText