using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    class XamlHelper
    {
        private static string nameSpace = "http://schemas.microsoft.com/winfx/2006/xaml";
        private static XmlReaderSettings xmlReaderSetting;
        private static XmlWriterSettings xmlWriterSetting;

        internal static XamlUI ConvertDLLToUI(string dllpath)
        {
            FileInfo info = new FileInfo(dllpath);
            Match match = new Regex(@"(?<name>.*)\.[dD][lL][lL]").Match(info.FullName);
            if (match.Success)
            {
                string path = match.Result("${name}") + ".xaml";
                if (System.IO.File.Exists(path))
                {
                    ValidationResult result = ValidateXaml(XmlReader.Create(path, XmlReaderSetting));
                    foreach (Type type in Assembly.LoadFile(info.FullName).GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(Page)) || type.IsSubclassOf(typeof(Control)))
                        {
                            FrameworkElement ele = Activator.CreateInstance(type) as FrameworkElement;
                            string[] id = new string[0];
                            if (ele != null)
                            {
                                id = HandleBindingInformation(ele, result.BindingInfo);
                            }
                            if (ele is Window)
                            {
                                Window window = ele as Window;
                                ele = window.Content as FrameworkElement;
                                ele.Resources = window.Resources;
                                NameScope.SetNameScope(ele, NameScope.GetNameScope(window));
                                window.Close();
                            }
                            XamlUI lui = new XamlUI(ele, id);
                            if (result.BindingInfo != null)
                            {
                                lui.BindingInfo = result.BindingInfo.ToArray();
                            }
                            return lui;
                        }
                    }
                }
            }
            return null;
        }

        internal static XamlUI ConvertDLLToUI(string xamlURI, string suffix)
        {
            FileInfo info = new FileInfo(xamlURI);
            Match match = new Regex(@"(?<name>.*)\.[dD][lL][lL]").Match(info.FullName);
            if (match.Success)
            {
                string path = match.Result("${name}") + ".xaml";
                if (System.IO.File.Exists(path))
                {
                   ValidationResult result = ValidateXaml(XmlReader.Create(path, XmlReaderSetting));
                    foreach (Type type in Assembly.LoadFile(info.FullName).GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(Page)) || type.IsSubclassOf(typeof(Control)))
                        {
                            FrameworkElement ele = Activator.CreateInstance(type) as FrameworkElement;
                            string[] id = new string[0];
                            if (ele != null)
                            {
                                id = HandleBindingInformation(ele, result.BindingInfo, suffix);
                            }
                            if (ele is Window)
                            {
                                Window window = ele as Window;
                                ele = window.Content as FrameworkElement;
                                ele.Resources = window.Resources;
                                NameScope.SetNameScope(ele, NameScope.GetNameScope(window));
                                window.Close();
                            }
                            XamlUI lui = new XamlUI(ele, id);
                            if (result.BindingInfo != null)
                            {
                                lui.BindingInfo = result.BindingInfo.ToArray();
                            }
                            return lui;
                        }
                    }
                }
            }
            return null;
        }

        internal static XamlUI ConvertXamlToUI(string uri)
        {
            ValidationResult result = ValidateXaml(XmlReader.Create(uri, XmlReaderSetting));
            FrameworkElement ele = XamlReader.Load(result.OutputXmlReader) as FrameworkElement;
            result.OutputXmlReader.Close();
            string[] id = new string[0];
            if (ele != null)
            {
                id = HandleBindingInformation(ele, result.BindingInfo);
            }
            if (ele is Window)
            {
                Window window = ele as Window;
                ele = window.Content as FrameworkElement;
                ele.Resources = window.Resources;
                NameScope.SetNameScope(ele, NameScope.GetNameScope(window));
                window.Close();
            }
            XamlUI lui = new XamlUI(ele, id);
            if (result.BindingInfo != null)
            {
                lui.BindingInfo = result.BindingInfo.ToArray();
            }
            return lui;
        }

        internal static XamlUI ConvertXamlToUI(byte[] fileBuffer)
        {
            MemoryStream input = new MemoryStream(fileBuffer);
            ValidationResult result = ValidateXaml(XmlReader.Create(input));
            FrameworkElement ele = XamlReader.Load(result.OutputXmlReader) as FrameworkElement;
            result.OutputXmlReader.Close();
            string[] id = new string[0];
            if (ele != null)
            {
                id = HandleBindingInformation(ele, result.BindingInfo);
            }
            if (ele is Window)
            {
                Window window = ele as Window;
                ele = window.Content as FrameworkElement;
                ele.Resources = window.Resources;
                NameScope.SetNameScope(ele, NameScope.GetNameScope(window));
                window.Close();
            }
            XamlUI lui = new XamlUI(ele, id);
            if (result.BindingInfo != null)
            {
                lui.BindingInfo = result.BindingInfo.ToArray();
            }
            return lui;
        }

        internal static XamlUI ConvertXamlToUI(string xamlURI, string suffix)
        {
            ValidationResult result = ValidateXaml(XmlReader.Create(xamlURI, XmlReaderSetting));
            FrameworkElement ele = XamlReader.Load(result.OutputXmlReader) as FrameworkElement;
            result.OutputXmlReader.Close();
            string[] id = new string[0];
            if (ele != null)
            {
                id = HandleBindingInformation(ele, result.BindingInfo, suffix);
            }
            if (ele is Window)
            {
                Window window = ele as Window;
                ele = window.Content as FrameworkElement;
                ele.Resources = window.Resources;
                NameScope.SetNameScope(ele, NameScope.GetNameScope(window));
                window.Close();
            }
            XamlUI lui = new XamlUI(ele, id);
            if (result.BindingInfo != null)
            {
                lui.BindingInfo = result.BindingInfo.ToArray();
            }
            return lui;
        }

        internal static XamlUI ConvertXamlToUI(byte[] fileBuffer, string suffix)
        {
            MemoryStream input = new MemoryStream(fileBuffer);
            ValidationResult result = ValidateXaml(XmlReader.Create(input));
            FrameworkElement ele = XamlReader.Load(result.OutputXmlReader) as FrameworkElement;
            result.OutputXmlReader.Close();
            string[] id = new string[0];
            if (ele != null)
            {
                id = HandleBindingInformation(ele, result.BindingInfo, suffix);
            }
            if (ele is Window)
            {
                Window window = ele as Window;
                ele = window.Content as FrameworkElement;
                ele.Resources = window.Resources;
                NameScope.SetNameScope(ele, NameScope.GetNameScope(window));
                window.Close();
            }
            XamlUI lui = new XamlUI(ele, id);
            if (result.BindingInfo != null)
            {
                lui.BindingInfo = result.BindingInfo.ToArray();
            }
            return lui;
        }

        internal static XamlUI ConvertXmlToUI(string xamlURI)
        {
            DisplayFile file = SerializeHelper.LoadXmlFile(xamlURI);
            FrameworkElement ele = SerializeHelper.ConvertStringToLayer(file.Content);
            string[] id = HandleBindingInformation(ele, file.BindingInfo);
            ele.Width = file.Width;
            ele.Height = file.Height;
            XamlUI lui = new XamlUI(ele, id);
            if (file.BindingInfo != null)
            {
                lui.BindingInfo = file.BindingInfo.ToArray();
            }
            return lui;
        }

        internal static XamlUI ConvertXmlToUI(byte[] fileBuffer)
        {
            DisplayFile file = SerializeHelper.LoadXmlFile(new MemoryStream(fileBuffer));
            FrameworkElement ele = SerializeHelper.ConvertStringToLayer(file.Content);
            string[] id = HandleBindingInformation(ele, file.BindingInfo);
            ele.Width = file.Width;
            ele.Height = file.Height;
            XamlUI lui = new XamlUI(ele, id);
            if (file.BindingInfo != null)
            {
                lui.BindingInfo = file.BindingInfo.ToArray();
            }
            return lui;
        }

        internal static XamlUI ConvertXmlToUI(string xamlURI, string suffix)
        {
            DisplayFile file = SerializeHelper.LoadXmlFile(xamlURI);
            FrameworkElement ele = SerializeHelper.ConvertStringToLayer(file.Content);
            string[] id = HandleBindingInformation(ele, file.BindingInfo, suffix);
            ele.Width = file.Width;
            ele.Height = file.Height;
            XamlUI lui = new XamlUI(ele, id);
            if (file.BindingInfo != null)
            {
                lui.BindingInfo = file.BindingInfo.ToArray();
            }
            return lui;
        }

        internal static XamlUI ConvertXmlToUI(byte[] fileBuffer, string suffix)
        {
            DisplayFile file = SerializeHelper.LoadXmlFile(new MemoryStream(fileBuffer));
            FrameworkElement ele = SerializeHelper.ConvertStringToLayer(file.Content);
            string[] id = HandleBindingInformation(ele, file.BindingInfo, suffix);
            ele.Width = file.Width;
            ele.Height = file.Height;
            XamlUI lui = new XamlUI(ele, id);
            if (file.BindingInfo != null)
            {
                lui.BindingInfo = file.BindingInfo.ToArray();
            }
            return lui;
        }

        private static XmlReaderSettings GetURLSupportedXmlReaderSettings()
        {
            XmlUrlResolver resolver = new XmlUrlResolver
            {
                Credentials = CredentialCache.DefaultCredentials
            };
            return new XmlReaderSettings { XmlResolver = resolver, CloseInput = true };
        }

        public static string[] HandleBindingInformation(FrameworkElement ele, BindingInformation bindingInformation)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex(".*XPath=(?<path>.*)[},]");
            foreach (BindingTag tag in bindingInformation)
            {
                if ((!string.IsNullOrEmpty(tag.ElementName) && !string.IsNullOrEmpty(tag.BindingPath)) && !string.IsNullOrEmpty(tag.AttributeName))
                {
                    DependencyObject obj2 = ele.FindName(tag.ElementName) as DependencyObject;
                    if (obj2 != null)
                    {
                        Type type = obj2.GetType();
                        string name = tag.AttributeName + "Property";
                        FieldInfo field = type.GetField(name, System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        if ((field != null) && (field.FieldType == typeof(DependencyProperty)))
                        {
                            string bindingPath;
                            DependencyProperty dp = field.GetValue(obj2) as DependencyProperty;
                            Match match = regex.Match(tag.BindingPath);
                            if (match.Success)
                            {
                                bindingPath = match.Result("${path}");
                            }
                            else
                            {
                                bindingPath = tag.BindingPath;
                            }
                            DataPointBindingObject bindingObject = UIManager.Instance.GetBindingObject(bindingPath);
                            if (bindingObject == null)
                            {
                                bindingObject = new DataPointBindingObject(bindingPath);
                                UIManager.Instance.AddBindingObjcet(bindingPath, bindingObject);
                            }
                            Binding binding = BindingOperations.GetBinding(obj2, dp);
                            if ((binding != null) && (obj2 is ISetValue))
                            {
                                object source = binding.Source;
                                if (source is DataPointBindingObject)
                                {
                                    MultiDataPointBindingObject obj5 = new MultiDataPointBindingObject(obj2 as ISetValue);
                                    obj5.AddBindingObject(source as DataPointBindingObject);
                                    obj5.AddBindingObject(bindingObject);
                                    BindingOperations.ClearBinding(obj2, dp);
                                    Binding binding2 = new Binding("Value")
                                    {
                                        Mode = BindingMode.OneWay,
                                        IsAsync = false,
                                        Source = obj5
                                    };
                                    BindingOperations.SetBinding(obj2, dp, binding2);
                                }
                                else if (source is MultiDataPointBindingObject)
                                {
                                    (source as MultiDataPointBindingObject).AddBindingObject(bindingObject);
                                }
                            }
                            else
                            {
                                Binding binding3 = new Binding("Value")
                                {
                                    IsAsync = false,
                                    Mode = BindingMode.OneWay,
                                    Source = bindingObject
                                };
                                if (obj2 is RotateTransform)
                                {
                                    binding3.Converter = new RotateValueConverter();
                                }
                                if (!string.IsNullOrEmpty(tag.Expression))
                                {
                                    //binding3.Converter = new ExpressionCalculateConverter(tag.Expression);
                                }
                                BindingOperations.SetBinding(obj2, dp, binding3);
                            }
                            if (!list.Contains(bindingPath))
                            {
                                list.Add(bindingPath);
                            }
                        }
                    }
                }
            }
            return list.ToArray();
        }

        private static string[] HandleBindingInformation(FrameworkElement ele, BindingInformation bindingInformation, string suffix)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex(".*XPath=(?<path>.*)[},]");
            foreach (BindingTag tag in bindingInformation)
            {
                if (tag.ElementName != string.Empty)
                {
                    DependencyObject obj2 = ele.FindName(tag.ElementName) as DependencyObject;
                    if (obj2 != null)
                    {
                        Type type = obj2.GetType();
                        string name = tag.AttributeName + "Property";
                        FieldInfo field = type.GetField(name, System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        if ((field != null) && (field.FieldType == typeof(DependencyProperty)))
                        {
                            string bindingPath;
                            DependencyProperty dp = field.GetValue(obj2) as DependencyProperty;
                            Match match = regex.Match(tag.BindingPath);
                            if (match.Success)
                            {
                                bindingPath = match.Result("${path}");
                            }
                            else
                            {
                                bindingPath = tag.BindingPath;
                            }
                            bindingPath = bindingPath + string.Format("({0})", suffix);
                            DataPointBindingObject bindingObject = UIManager.Instance.GetBindingObject(bindingPath);
                            if (bindingObject == null)
                            {
                                bindingObject = new DataPointBindingObject(bindingPath);
                                UIManager.Instance.AddBindingObjcet(bindingPath, bindingObject);
                            }
                            Binding binding = BindingOperations.GetBinding(obj2, dp);
                            if (binding != null)
                            {
                                object source = binding.Source;
                                if (source is DataPointBindingObject)
                                {
                                    MultiDataPointBindingObject obj5 = new MultiDataPointBindingObject(obj2 as ISetValue);
                                    obj5.AddBindingObject(source as DataPointBindingObject);
                                    obj5.AddBindingObject(bindingObject);
                                    BindingOperations.ClearBinding(obj2, dp);
                                    Binding binding2 = new Binding("Value")
                                    {
                                        Mode = BindingMode.OneWay,
                                        IsAsync = false,
                                        Source = obj5
                                    };
                                    BindingOperations.SetBinding(obj2, dp, binding2);
                                }
                                else if (source is MultiDataPointBindingObject)
                                {
                                    (source as MultiDataPointBindingObject).AddBindingObject(bindingObject);
                                }
                            }
                            else
                            {
                                Binding binding3 = new Binding("Value")
                                {
                                    IsAsync = false,
                                    Mode = BindingMode.OneWay,
                                    Source = bindingObject
                                };
                                if (obj2 is RotateTransform)
                                {
                                    binding3.Converter = new RotateValueConverter();
                                }
                                if (!string.IsNullOrEmpty(tag.Expression))
                                {
                                    //binding3.Converter = new ExpressionCalculateConverter(tag.Expression);
                                }
                                BindingOperations.SetBinding(obj2, dp, binding3);
                            }
                            if (!list.Contains(bindingPath))
                            {
                                list.Add(bindingPath);
                            }
                        }
                    }
                }
            }
            return list.ToArray();
        }

        private static void RewriteAttributes(XmlReader reader, XmlWriter writer, BindingInformation bindingInfo)
        {
            string str = string.Empty;
            List<BindingTag> list = new List<BindingTag>();
            while (reader.MoveToNextAttribute())
            {
                if ((reader.Prefix != "x") || (reader.LocalName != "Class"))
                {
                    if (reader.LocalName == "Name")
                    {
                        str = reader.Value;
                    }
                    string str2 = reader.Value;
                    if ((str2.StartsWith("{") && str2.EndsWith("}")) && str2.Contains("XPath"))
                    {
                        list.Add(new BindingTag("", reader.LocalName, reader.Value));
                    }
                    else
                    {
                        writer.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                        writer.WriteValue(reader.Value);
                        writer.WriteEndAttribute();
                    }
                }
            }
            if (list.Count > 0)
            {
                if (str == string.Empty)
                {
                    str = "ID_" + Guid.NewGuid().ToString("N");
                    writer.WriteStartAttribute("x", "Name", nameSpace);
                    writer.WriteValue(str);
                    writer.WriteEndAttribute();
                }
                foreach (BindingTag tag in list)
                {
                    tag.ElementName = str;
                    bindingInfo.Add(tag);
                }
            }
        }

        public static ValidationResult ValidateXaml(XmlReader reader)
        {
            MemoryStream output = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(output);
            BindingInformation bindingInfo = new BindingInformation();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Whitespace)
                {
                    writer.WriteWhitespace(reader.Value);
                }
                else
                {
                    if ((reader.NodeType == XmlNodeType.Element) && !reader.IsEmptyElement)
                    {
                        writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                        if (reader.HasAttributes)
                        {
                            RewriteAttributes(reader, writer, bindingInfo);
                        }
                        continue;
                    }
                    if (reader.IsEmptyElement)
                    {
                        writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                        if (reader.HasAttributes)
                        {
                            RewriteAttributes(reader, writer, bindingInfo);
                        }
                        writer.WriteEndElement();
                        continue;
                    }
                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        writer.WriteEndElement();
                    }
                    else if ((reader.NodeType == XmlNodeType.Text) && reader.HasValue)
                    {
                        writer.WriteValue(reader.Value);
                    }
                }
            }
            writer.Flush();
            output.Position = 0L;
            writer.Close();
            reader.Close();
            return new ValidationResult(bindingInfo, output);
        }

        public static XmlReaderSettings XmlReaderSetting
        {
            get
            {
                if (xmlReaderSetting == null)
                {
                    xmlReaderSetting = GetURLSupportedXmlReaderSettings();
                }
                return xmlReaderSetting;
            }
        }

        private static XmlWriterSettings XmlWriterSetting
        {
            get
            {
                if (xmlWriterSetting == null)
                {
                    xmlWriterSetting = new XmlWriterSettings();
                    xmlWriterSetting.CloseOutput = true;
                }
                return xmlWriterSetting;
            }
        }
    }
}
