using KeNetEditor.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Serialization;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class SerializeHelper
    {
        // Methods
        public static ContentLayer CloneLayer(ContentLayer layer)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(XamlWriter.Save(layer)), false))
            {
                return (XamlReader.Load(stream) as ContentLayer);
            }
        }

        public static FrameworkElement ConvertStringToLayer(string content)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content), false))
            {
                return (XamlReader.Load(stream) as FrameworkElement);
            }
        }

        public static DisplayFile LoadBinaryFile(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (formatter.Deserialize(stream) as DisplayFile);
        }

        public static DisplayFile LoadBinaryFile(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return LoadBinaryFile(stream);
            }
        }

        public static DisplayFile LoadXmlFile(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DisplayFile));
            return (serializer.Deserialize(stream) as DisplayFile);
        }

        public static DisplayFile LoadXmlFile(string path)
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                return LoadXmlFile(reader);
            }
        }

        private static DisplayFile LoadXmlFile(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DisplayFile));
            return (serializer.Deserialize(reader) as DisplayFile);
        }

        public static void SaveBinaryFile(DisplayFile file, Stream stream)
        {
            new BinaryFormatter().Serialize(stream, file);
        }

        public static void SaveBinaryFile(DisplayFile file, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                SaveBinaryFile(file, stream);
            }
        }

        public static void SaveXmlFile(DisplayFile file, Stream stream)
        {
            new XmlSerializer(typeof(DisplayFile)).Serialize(stream, file);
        }

        public static void SaveXmlFile(DisplayFile file, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                SaveXmlFile(file, stream);
            }
        }
    }


}
