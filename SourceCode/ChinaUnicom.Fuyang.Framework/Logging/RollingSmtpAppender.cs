using System;
using System.Collections.Generic;
using System.Text;
using log4net.Appender;
using ChinaUnicom.Fuyang.Framework.Configuration;
//using ChinaUnicom.Fuyang.Common.Configuration;

namespace ChinaUnicom.Fuyang.Framework.Logging
{
    /// <summary>
    /// Author	: Steven Cao
    /// </summary>
    /// <summary>
    /// Create Date	: April 29, 2011
    /// </summary>
    /// <summary>
    /// Version	:1.0
    /// </summary>	
    /// <summary>
    /// Description: Catch Error on (DEV,TST and PRD) and send it back to team
    /// </summary>
    public class RollingSmtpAppender : SmtpAppender
    {
        public RollingSmtpAppender()
            : base()
        {
            if (!Configurator.Instance.IsRunningLocally())
            {
                this.Threshold = log4net.Core.Level.Error;

                this.From = Configurator.Instance.GetConfigByName("smtp-user-address");
                this.SmtpHost = Configurator.Instance.GetConfigByName("smtp-host");
                this.Username = this.From;
                this.Password = Configurator.Instance.GetConfigByName("smtp-user-pass");

                this.Authentication = SmtpAuthentication.Basic;
                this.Subject = Configurator.Instance.RunningModel() + ":Logging Error Message";

                
            }
            else
            {
                this.Close();
            }
        }
    }
}
