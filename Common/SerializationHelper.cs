﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Common
{
    /// <summary>
    /// 对象序列化及反序列化
    /// </summary>
    public class SerializationHelper
    {
        #region SerializeToStream
        public static Stream SerializeToStream(object obj)
        {
            Stream s = (Stream)(new MemoryStream());
            SerializeToStream(s, obj);
            return s;
        }

        public static void SerializeToStream(Stream s, object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, obj);
        }
        #endregion

        #region SerializeToXml
        public static string SerializeToXml(object obj)
        {
            return SerializeToXml(obj, "", "");
        }

        public static string SerializeToXml(object obj, string xsi, string xsd)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            xsn.Add(xsi, xsd);
            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                serializer.Serialize(tw, obj, xsn);
            }

            return sb.ToString();
        }
        #endregion

        #region DeserializeFromStream
        public static object DeserializeFromStream(Stream s, Type type)
        {
            BinaryFormatter bf = new BinaryFormatter();
            s.Position = 0;
            return bf.Deserialize(s);
        }

        public static T DeserializeFromStream<T>(Stream s)
        {
            return (T)DeserializeFromStream(s, typeof(T));
        }
        #endregion

        #region DeserializeFromXml
        public static object DeserializeFromXml(string xml, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            object obj = null;

            using (StringReader sr = new StringReader(xml))
            {
                obj = serializer.Deserialize((TextReader)sr);
            }

            return obj;
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            return (T)DeserializeFromXml(xml, typeof(T));
        }
        #endregion

        /// <summary>
        /// 数字科学计数法处理
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static Decimal ChangeToDecimal(string strData)
        {
            Decimal dData = 0.0M;
            if (strData.Contains("E"))
            {
                dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            else
            {
                dData = Convert.ToDecimal(strData);
            }
            return Math.Round(dData, 5);
        }
    }
}

