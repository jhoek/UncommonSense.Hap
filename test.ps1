# ConvertTo-HtmlDocument https://www.nu.nl
# | Select-HtmlNode -XPath '//head'
# | Select-HtmlNode -XPath 'title'
# | Select-Object -ExpandProperty InnerText

# Invoke-WebRequest 'https://www.volkskrant.nl/columns/Sylvia-Witteman'
# | Select-Object -ExpandProperty Content
# | ConvertTo-HtmlDocument
# | Select-HtmlNode -CssSelector '.teaser--compact h3' -All
# | Select-Object -ExpandProperty InnerText

ConvertTo-HtmlDocument -Uri https://bctechdays.com/event
| Select-HtmlNode -CssSelector '.modal[aria-labelledby^="event-detail-"] .modal-header' -All
| Convert-HtmlNode @{DateTime = 'h5' } -TypeName 'UncommonSense.BcTechDays.ScheduledEvent' -Mode CssSelector
| Select-Object -ExpandProperty DateTime
