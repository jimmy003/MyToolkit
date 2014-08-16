﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

#if WINRT
using Windows.Data.Xml.Dom;
#endif

#if !SL4
using System.Threading.Tasks;
#endif

namespace MyToolkit.Utilities
{
    public static class Xml
    {
#if WINRT

        public static string XmlEscape(string unescaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerText;
        }

        public static string XmlUnescape(string escaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerText = escaped;
            return node.InnerText;
        }

#elif !SL4 && !WP7 && !WP8 && !SL5

        public static string XmlEscape(string unescaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        }

        public static string XmlUnescape(string escaped)
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerXml = escaped;
            return node.InnerText;
        }

#endif
    }
}
