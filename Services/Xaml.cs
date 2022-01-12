using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;

namespace FasesDaLua.Services
{
    public class Xaml : ServiceBase
    {
        public Xaml()
        {

        }

        public T GetXamlElement<T>(string elementPath) where T : FrameworkElement
        {
            if (!String.IsNullOrEmpty(elementPath))
            {
                var assembly = Assembly.GetEntryAssembly();

                using (var stream = assembly.GetManifestResourceStream(elementPath))
                {
                    return (T)XamlReader.Load(stream);
                }
            }

            throw new FileNotFoundException($"File {elementPath} not found.");
        }

        public void SaveXps(FixedDocument document, string path, string fileName)
        {
            using (var xpsd = new XpsDocument(Path.Combine(path, fileName), FileAccess.ReadWrite))
            {
                var xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
                xw.Write(document);
                xpsd.Close();
            }
        }
    }
}
