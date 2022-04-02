# regenerate all images first
Write-Host
Write-Host "Generating images out of plant UML diagrams" -ForegroundColor Yellow
$directorypath = Split-Path $MyInvocation.MyCommand.Path
java -jar "C:/Program Files/plantuml/plantuml.jar" "$directorypath/diagrams" -o "$directorypath/images"

# generate the html version
Write-Host
Write-Host "Generating HTML documentation" -ForegroundColor Yellow
asciidoctor --attribute=generate-html .\BerghAdminTOC.adoc -o BerghAdminDesign.html

# generate the pdf version
Write-Host
Write-Host "Generating PDF documentation" -ForegroundColor Yellow
asciidoctor -r asciidoctor-pdf -b pdf --attribute=generate-pdf .\BerghAdminTOC.adoc --verbose --trace -o BerghAdminDesign.pdf
