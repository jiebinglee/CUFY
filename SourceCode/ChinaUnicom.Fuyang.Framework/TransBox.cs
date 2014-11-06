using System;
using System.Text;
using Newtonsoft.Json;

namespace ChinaUnicom.Fuyang.Framework
{
    [Serializable]
    public class TransBox
    {
        public string OwnerClass { get; set; }

        public bool IsOK { get; set; }
        public string Message { get; set; }

        public string OPID { get; set; }
        public string Content { get; set; }

        public void SetContent(object content)
        {
            try
            {
                this.Content = JsonConvert.SerializeObject(content);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public T GetContent<T>()
        {
            T content = default(T);
            try
            {
                content = JsonConvert.DeserializeObject<T>(this.Content);
            }
            catch (Exception err)
            {
                throw err;
            }
            return content;
        }

        //public override string ToString()
        //{
        //    StringBuilder lsb_msg = new StringBuilder();

        //    // OwnerClass-OPID
        //    lsb_msg.AppendLine(string.Format("TransBox({0}-{1})", this.OwnerClass, this.OPID) + "{");
        //    lsb_msg.AppendLine(string.Format("  IsOK:{0}", this.IsOK.ToString()));
        //    lsb_msg.AppendLine(string.Format("  Message:{0}", this.Message ?? "Null"));
        //    lsb_msg.AppendLine(string.Format("  Content:{0}", this.Content ?? "Null"));
        //    lsb_msg.Append("}");

        //    return lsb_msg.ToString();
        //}
    }
}
