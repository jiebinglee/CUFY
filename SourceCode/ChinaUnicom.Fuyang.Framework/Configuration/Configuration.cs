using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace ChinaUnicom.Fuyang.Framework.Configuration
{
    public class Configurator
    {
        private static readonly object m_lock = new object();
        private static volatile Configurator m_inst = null;

        private Dictionary<string, string> table = null;
        private string runningModle = null;

        public static Configurator Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (m_inst == null) m_inst = new Configurator();
                }
                return m_inst;
            }
        }

        private Configurator() { Init(); }

        private void Init()
        {
            table = new Dictionary<string, string>();

            Stream stream = null;
            string filename = null;
            string path = null;

            filename = "appconfig.xml";
            path = System.AppDomain.CurrentDomain.BaseDirectory + "Bin\\" + filename;


            if (!System.IO.File.Exists(path))
            {
                stream = typeof(Configurator).Assembly
                    .GetManifestResourceStream("ChinaUnicom.Fuyang.Common." + filename);
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                stream = reader.BaseStream;
            }

            try
            {
                Parse(stream);
                runningModle = GetConfigByName("run-model");
            }
            catch (Exception ex)
            {
                //logger.Error("Please check the config XML file:", ex);
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }
        private void Parse(Stream stream)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList list = doc.SelectNodes("/config/item");
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode node = list[i];
                string key = node.SelectSingleNode("key").InnerText.Trim();
                string value = node.SelectSingleNode("value").InnerText.Trim();
                //logger.Debug("config:" + key + ":" + value);
                table.Add(key, value);
            }
        }

        public string GetConfigByName(string name)
        {
            string s = null;
            try
            {
                //running model
                if ((!string.IsNullOrEmpty(runningModle))
                    && (!runningModle.Equals("local"))
                    && (table.ContainsKey(name + "-" + runningModle)))
                {
                    s = (string)table[name + "-" + runningModle];
                }
                else
                {
                    s = (string)table[name];
                }
            }
            catch (Exception ex)
            {
                //logger.Error("Cannot find Config by key:" + name + ":", ex);
                throw ex;
            }
            return s;
        }

        public bool IsRunningLocally()
        {
            if (string.IsNullOrEmpty(runningModle))
            {
                return true;
            }
            return runningModle.Equals("local");

        }

        public string RunningModel()
        {
            return runningModle;
        }

        public bool IsOnLocal { get { return "local".Equals(runningModle); } }
        public bool IsOnDEV { get { return "dev".Equals(runningModle); } }
        public bool IsOnQA { get { return "tst".Equals(runningModle); } }
        public bool IsOnPRD { get { return "prd".Equals(runningModle); } }
    }
}
