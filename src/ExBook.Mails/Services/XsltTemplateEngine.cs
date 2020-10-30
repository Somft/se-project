using ExBook.Mails.Templates;

using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace ExBook.Mails.Services
{
    public class XsltTemplateEngine : ITemplateEngine
    {
        private readonly ITemplateResolver templateResolver;
        private readonly XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };

        public XsltTemplateEngine(ITemplateResolver templateResolver)
        {
            this.templateResolver = templateResolver;
        }

        public async Task<string> Render<T>(string template, T context) where T : EmailContext
        {
            XslCompiledTransform transform = await this.CompileTransform(template);
            XmlDocument contextXml = this.SerializeContext(context);

            using var sw = new StringWriter();
            var myWriter = XmlWriter.Create(sw, this.xmlWriterSettings);

            transform.Transform(contextXml, null, myWriter);

            return sw.ToString();
        }

        private async Task<XslCompiledTransform> CompileTransform(string template)
        {
            var transform = new XslCompiledTransform();
            var doc = new XmlDocument();
            doc.LoadXml(await this.templateResolver.GetTemplate(template));
            transform.Load(doc);
            return transform;
        }

        private XmlDocument SerializeContext(object context)
        {
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, this.xmlWriterSettings);

            var serializer = new XmlSerializer(context.GetType());
            serializer.Serialize(xmlWriter, context);

            var doc = new XmlDocument();
            doc.LoadXml(stringWriter.ToString());
            return doc;
        }
    }
}
