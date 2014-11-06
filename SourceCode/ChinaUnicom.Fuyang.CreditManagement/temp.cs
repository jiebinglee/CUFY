using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement
{
    class temp
    {
        private string CreateRandomUserName()
        {
            string result = string.Empty;

            List<string> strList = new List<string>();

            for (int i = 97; i <= 122; i++)
            {
                char aa = (char)i;
                strList.Add(aa.ToString());
            }

            for (int i = 65; i <= 90; i++)
            {
                char aa = (char)i;
                strList.Add(aa.ToString());
            }

            for (int i = 48; i <= 57; i++)
            {
                char aa = (char)i;
                strList.Add(aa.ToString());
            }

            for (int i = 0; i < 8; i++)
            {
                Random r = new Random(Guid.NewGuid().GetHashCode());
                int y = r.Next(100);
                int x = y % strList.Count;
                result = result + strList[x];
            }

            return result;
        }

        private int GetDevType(string devTypeDesc)
        {
            int result = 0;
            switch (devTypeDesc)
            {
                case "亲情卡、长话卡":
                    result = 1;
                    break;
                case "2G单卡、无线公话":
                    result = 2;
                    break;
                case "3G/4G单卡、宽带":
                    result = 3;
                    break;
                case "3G/4G合约终端":
                    result = 4;
                    break;
            }

            return result;
        }


    }
}
