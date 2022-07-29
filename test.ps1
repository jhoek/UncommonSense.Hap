# ConvertTo-HtmlDocument https://www.nu.nl
# | Select-HtmlNode -SelectSingleNode '//head'
# | Select-HtmlNode -SelectSingleNode 'title'
# | Select-Object -ExpandProperty InnerText

ConvertTo-HtmlDocument 'https://www.volkskrant.nl/columns/Sylvia-witteman'
| Select-HtmlNode -SelectSingleNode '//article'