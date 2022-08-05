# ConvertTo-HtmlDocument https://www.nu.nl
# | Select-HtmlNode -XPath '//head'
# | Select-HtmlNode -XPath 'title'
# | Select-Object -ExpandProperty InnerText

# Invoke-WebRequest 'https://www.volkskrant.nl/columns/Sylvia-Witteman'
# | Select-Object -ExpandProperty Content
# | ConvertTo-HtmlDocument
# | Select-HtmlNode -CssSelector '.teaser--compact h3' -All
# | Select-Object -ExpandProperty InnerText

$Property = [Ordered]@{
    DateTime    = 'h5.modal-title'
    Title       = 'h2.text-color-secondary'
    Speakers    = 'h2.accordion-header button'
    Tags        = 'span.tag'
    Description = '.pt-3 p'
}

ConvertTo-HtmlDocument -Uri https://bctechdays.com/event
| Select-HtmlNode -CssSelector '.modal[aria-labelledby^="event-detail-"] .modal-content' -All
| Convert-HtmlNode -Property $Property -TypeName 'UncommonSense.BcTechDays.ScheduledEvent' -Mode CssSelector

# FIXME: Description