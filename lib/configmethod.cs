using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace HTTP.lib
{
   public  class configmethod
    {
        public static bool initConfig()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<设置>");
                sb.AppendFormat("<当前版本>{0}</当前版本></设置>", "1.0");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sb.ToString());
                doc.Save(Application.StartupPath + @"\zh-CN\Config.xml");
                return true;
               
            }
            catch(Exception ex)
            {
                return false;
            }
        }



        //判断文件是否存在
        public static bool hasConfig()
        {
            if (File.Exists(Application.StartupPath + @"\zh-CN\Config.xml"))
            { 
                return true;

            }
            else
            {
                return false;
            }
        }

        public static string SaveConfig(configobj obj)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Application.StartupPath + @"\zh-CN\Config.xml");
                XmlNode nodeobj = xml.SelectSingleNode("/设置/当前版本");
                nodeobj.InnerText = obj.NowVersion;
                xml.Save(Application.StartupPath + @"\zh-CN\Config.xml");
                return obj.NowVersion;

            }
            catch(Exception ex)
            {
                return obj.NowVersion;
            }
        }

        public static string LoadConfig(configobj obj)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Application.StartupPath + @"\zh-CN\Config.xml");
                XmlNode nodeobj = xml.SelectSingleNode("/设置/当前版本");
                obj.NowVersion = nodeobj.InnerText;
                xml.Save(Application.StartupPath + @"\zh-CN\Config.xml");
                return obj.NowVersion;

            }
            catch (Exception ex)
            {
                return obj.NowVersion;
            }
        }
    }
}
