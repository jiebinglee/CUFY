/**
 * Project Title: Lilly Work Flow Site
 * Description: Lilly Work Flow Site
 * Copyright: Copyright (C) 2010
 * Company: Lilly China
 *
 */
using System;
using log4net;
using System.IO;

namespace ChinaUnicom.Fuyang.Framework.Logging
{
    public enum LOG_FLAG
    {
        INFO = 1,
        DEBUG = 2,
        WARN = 3,
        ERROR = 4
    }

    /// <summary>
    /// Class to write log
    /// </summary>
    public class Logger
    {

        /// <summary>
        /// logger
        /// </summary>
        ILog logger = LogManager.GetLogger(typeof(Logger));
        //private LSGUtilities _util = LSGUtilities.Instance();

        static Logger()
        {
            Init();

        }
        public static void Init()
        {
            Stream configStream = null;
            try
            {
                configStream = typeof(Logger)
                    .Assembly.GetManifestResourceStream("ChinaUnicom.Fuyang.Framework.Logging.log4net.xml");
                log4net.Config.XmlConfigurator.Configure(configStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                configStream.Close();
            }
        }

        public Logger()
        {

        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="obj">info to show</param>
        public void Debug(object obj)
        {
            logger.Debug(obj);
        }

        /// <summary>
        /// info
        /// </summary>
        /// <param name="obj">info to show</param>
        public void Info(object obj)
        {
            logger.Info(obj);
        }

        /// <summary>
        /// warn
        /// </summary>
        /// <param name="obj">info to show</param>
        public void Warn(object obj)
        {
            logger.Warn(obj);
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="obj">info to show</param>
        public void Error(object obj)
        {
            logger.Error(obj);
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="obj">info to show</param>
        /// <param name="ex">exception to show</param>
        public void Debug(object obj, Exception ex)
        {
            logger.Debug(obj, ex);
        }

        /// <summary>
        /// info
        /// </summary>
        /// <param name="obj">info to show</param>
        /// <param name="ex">exception to show</param>
        public void Info(object obj, Exception ex)
        {
            logger.Info(obj, ex);
        }

        /// <summary>
        /// warn
        /// </summary>
        /// <param name="obj">info to show</param>
        /// <param name="ex">exception to show</param>
        public void Warn(object obj, Exception ex)
        {
            logger.Warn(obj, ex);
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="obj">info to show</param>
        /// <param name="ex">exception to show</param>
        public void Error(object obj, Exception ex)
        {
            logger.Error(obj, ex);
        }

        public string GetVersion()
        {
            return "1.0.0.0";
        }

        public void Warn(Exception ex)
        {
            logger.Warn("Exception:", ex);
        }

        public void Error(Exception ex)
        {
            logger.Error("Exception:", ex);
        }

        //private void WriteLogThroughLSG(string source, string msg)
        //{
        //    _util.log(source, "ims TGen app|" + msg, true, true);
        //}

        private string BuildExceptionString(Exception exception)
        {
            string errMessage = string.Empty;

            errMessage += exception.Message + Environment.NewLine + exception.StackTrace;

            while (exception.InnerException != null)
            {
                errMessage += BuildInnerExceptionString(exception.InnerException);
                exception = exception.InnerException;
            }

            return errMessage;
        }

        private string BuildInnerExceptionString(Exception innerException)
        {
            string errMessage = string.Empty;

            errMessage += Environment.NewLine + " InnerException ";
            errMessage += Environment.NewLine + innerException.Message + Environment.NewLine + innerException.StackTrace;

            return errMessage;
        }
    }
}
