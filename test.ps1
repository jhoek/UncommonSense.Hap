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
| Select-HtmlNode -CssSelector '.modal[aria-labelledby^="event-detail-"] .modal-content' -All
| Convert-HtmlNode @{DateTime = 'h5.modal-title'; Title = 'h2.text-color-secondary'; Speakers = 'h2.accordion-header button' } -TypeName 'UncommonSense.BcTechDays.ScheduledEvent' -Mode CssSelector -Verbose
