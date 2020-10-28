using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ExBook.Mails.Services
{
    public class EmbeddedTemplateResolver : ITemplateResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<string> GetTemplate(string name)
        {
            Assembly assembly = typeof(EmbeddedTemplateResolver).Assembly;
            string file = $"{assembly.GetName().Name}.Templates.{name}.xslt";
            Stream stream = assembly.GetManifestResourceStream(file) ?? throw new FileNotFoundException();
            return await new StreamReader(stream).ReadToEndAsync();
        }
    }
}
