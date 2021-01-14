<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:template match="/">
    <html>
      <body>
        Click <a href="https://www.youtube.com/watch?v=dQw4w9WgXcQ">here</a> to confirm an account.
      <xsl:value-of select="Token"/>  
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
