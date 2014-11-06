using ChinaUnicom.Fuyang.Framework;
using ChinaUnicom.Fuyang.Framework.FileSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Autofac;
using System.Web.Script.Services;
using ChinaUnicom.Fuyang.Framework.Data;
using ChinaUnicom.Fuyang.CreditManagement.Models;
using Newtonsoft.Json;
using ChinaUnicom.Fuyang.Framework.Adapter;

namespace ChinaUnicom.Fuyang.WebService
{
    /// <summary>
    /// cufyWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class cufyWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            Repository<Channel> channel = new Repository<Channel>(AutofacContainer.Current.Resolve<IRepositoryContext>());
            return channel.Table.ToList().Count.ToString();
        }

        [WebMethod]
        public string Services()
        {
            List<string> result = new List<string>();
            foreach (var x in AutofacContainer.Current.ComponentRegistry.Registrations)
            {
                result.Add(x.Activator.LimitType.FullName);
            }

            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public string Assemblies()
        {
            var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();

            List<string> result = new List<string>();
            foreach (var x in assemblys)
            {
                if (x.FullName.IndexOf("ChinaUnicom") == 0)
                {
                    result.Add(x.FullName);
                }
            }

            return JsonConvert.SerializeObject(result);
        }
       
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public TransBox Process(TransBox data,string token)
        {
            //AutofacContainer.ConfigureContainer();

            //var typeAdapterFactory = AutofacContainer.Current.Resolve<ITypeAdapterFactory>();
            //TypeAdapterFactory.SetCurrent(typeAdapterFactory);

            TransBox transBox = new TransBox();
            try
            {
                var user = 0;
                if (data.OPID != "Login")
                {
                    user = TokenManager.Instance.GetCurrentUserId(token);
                }

                var trans = data;
                if (trans != null)
                {
                    var manager = AutofacContainer.Current.ResolveNamed<IModuleManager>(trans.OwnerClass);
                    if (manager != null)
                    {
                        transBox = manager.ProcessTask(trans, user.ToString());
                    }
                    else
                    {
                        transBox.Message = "OwnerClass is invalid.";
                    }
                }
                else
                {
                    transBox.Message = "Data is invalid.";
                }
            }
            catch (Exception err)
            {
                transBox.IsOK = false;
                transBox.Message = err.ToString();
            }
            finally
            {
            }

            return transBox;
        }
    }
}
