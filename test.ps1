ConvertTo-HtmlDocument https://www.nu.nl
| Select-HtmlNode -SelectSingleNode '//head'
| Get-Member