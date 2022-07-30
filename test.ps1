# ConvertTo-HtmlDocument https://www.nu.nl
# | Select-HtmlNode -SelectSingleNode '//head'
# | Select-HtmlNode -SelectSingleNode 'title'
# | Select-Object -ExpandProperty InnerText

Invoke-WebRequest 'https://www.volkskrant.nl/columns/Sylvia-Witteman'
| Select-Object -ExpandProperty Content
| ConvertTo-HtmlDocument
| Select-HtmlNode -SelectNodes '//article//h3'
| Select-Object -ExpandProperty InnerText