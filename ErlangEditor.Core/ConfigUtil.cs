using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace ErlangEditor.Core
{
    public static class ConfigUtil
    {
        private static Entity.ConfigEntity config_ = new Entity.ConfigEntity();

        public static void LoadConfig()
        {
            try
            {
                using (var sr = new StreamReader("ErlangEditorConfig.json"))
                {
                    var strData = sr.ReadToEnd();
                    var entity = JsonConvert.DeserializeObject<Entity.ConfigEntity>(strData);
                    config_ = entity;
                }
            }
            catch 
            {
                throw new Exception("读取设置文件出现错误，请在设置重新保存设置。");
            }
        }

        public static void SaveConfig()
        {
            try
            {
                var strData = JsonConvert.SerializeObject(config_);
                using (var sw = new StreamWriter("ErlangEditorConfig.json"))
                {
                    sw.Write(strData);
                    sw.Flush();
                }
            }
            catch 
            {
                throw new Exception("保存设置文件遇到了问题。");
            }
        }

        public static Entity.ConfigEntity Config
        {
            get { return config_; }
        }
    }
}
