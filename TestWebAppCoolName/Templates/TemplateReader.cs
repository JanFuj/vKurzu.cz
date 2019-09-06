using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TestWebAppCoolName.Templates
{
    public class TemplateReader
    {
        private TemplateProcessor _templateProcesor;

        public TemplateReader()
        {
            _templateProcesor = new TemplateProcessor();
        }


        public Task<string> ReadAsync(string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            return Read(new FileInfo(file));
        }
        public async Task<string> ReadProcessedAsync(string file, object model)
        {
            return _templateProcesor.GetContentFromTemplate(await ReadAsync(file), model);
        }
        internal async Task<string> Read(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                throw new ArgumentException("File does not exist.", nameof(fileInfo));
            }
            using (var reader = new StreamReader(fileInfo.OpenRead()))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}