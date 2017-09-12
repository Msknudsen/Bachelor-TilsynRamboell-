@echo BachelorProjekt Rapport characters:
@pdftotext BachelorProjekt.pdf -enc UTF-8 - | wc
@echo   Lines ^| Words ^| Chars
@pause