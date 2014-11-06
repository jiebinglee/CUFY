using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ChinaUnicom.Fuyang.Framework.Logging;

namespace ChinaUnicom.Fuyang.Framework
{
    public delegate bool ProcessFunc(TransBox data, string user, out object content, ref StringBuilder messager);

    public abstract class ModuleManager : IModuleManager
    {
        public ModuleManager()
        {
            _tasks = new Dictionary<string, ProcessFunc>();
            TaskBinding(ref _tasks);
            //logger = new Logger();
        }
        private Logger logger = null;

        protected abstract void TaskBinding(ref Dictionary<string, ProcessFunc> tasks);
        private Dictionary<string, ProcessFunc> _tasks =
            new Dictionary<string, ProcessFunc>();

        public TransBox ProcessTask(TransBox data, string user)
        {
            TransBox ltb_ret = new TransBox();
            try
            {
                ltb_ret.OwnerClass = GetType().Name;
                ltb_ret.OPID = data.OPID;

                ProcessFunc lfun_task = null;
                if (_tasks != null && _tasks.ContainsKey(data.OPID) && (lfun_task = _tasks[data.OPID]) != null)
                {
                    object content = null;
                    StringBuilder messager = new StringBuilder();
                    ltb_ret.IsOK = lfun_task(data, user, out content, ref messager);
                    ltb_ret.Message = messager == null ? string.Empty : messager.ToString();
                    ltb_ret.SetContent(content);
                }
                else
                {
                    ltb_ret.IsOK = false;
                    ltb_ret.Message = string.Format("There is no any task corresponding to '{0}'", data.OPID);
                }

            }
            catch (Exception err)
            {
                ltb_ret.IsOK = false;
                ltb_ret.Message = err.ToString();
            }
            finally
            {
            }
            return ltb_ret;
        }
                
        protected void WriteLog(LOG_FLAG flg, string msg) 
        {
            switch (flg)
            {
                case LOG_FLAG.INFO:
                    logger.Info(msg ?? string.Empty);
                    break;
                case LOG_FLAG.DEBUG:
                    logger.Debug(msg ?? string.Empty);
                    break;
                case LOG_FLAG.WARN:
                    logger.Warn(msg ?? string.Empty);
                    break;
                case LOG_FLAG.ERROR:
                    logger.Error(msg ?? string.Empty);
                    break;
            }
        }
    }
}
