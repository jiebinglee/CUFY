using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;

namespace ChinaUnicom.Fuyang.Framework
{
    public sealed class TokenManager
    {
        private static volatile TokenManager instance;
        private static object syncRoot = new Object();

        private TokenManager() { }

        public static TokenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TokenManager();
                        }
                    }
                }

                return instance;
            }
        }

        public const int LOGINTOKEN_TIMEOUT = 20;
        public class TokenTimeout
        {
            public int UserId;
            public string UserName;
            public DateTime Expires;
        }
        private object loginTokensLock = new object();
        private Dictionary<string, TokenTimeout> loginTokens = new Dictionary<string, TokenTimeout>();

        public string GenerateLoginToken(int userId, string userName)
        {
            string loginToken = null;
            try
            {
                loginToken = Guid.NewGuid().ToString();
                var tokenTimeout = new TokenTimeout();
                tokenTimeout.UserId = userId;
                tokenTimeout.UserName = userName;
                tokenTimeout.Expires = DateTime.Now.AddMinutes(LOGINTOKEN_TIMEOUT);
                lock (loginTokensLock)
                {
                    loginTokens.Add(loginToken, tokenTimeout);
                }
                ClearExpiresToken();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return loginToken;
        }

        public void ClearExpiresToken()
        {
            try
            {
                var now = DateTime.Now;
                lock (loginTokensLock)
                {
                    List<string> expiresTokens = new List<string>();
                    foreach (var token in loginTokens.Keys)
                    {
                        var tokenTimeout = loginTokens[token];
                        if (now >= tokenTimeout.Expires)
                        {  
                            expiresTokens.Add(token);
                        }
                    }
                    foreach (var token in expiresTokens)
                    {
                        loginTokens.Remove(token);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public int CheckLoginToken(string token)
        {
            lock (loginTokensLock)
            {
                if ((loginTokens.Count > 0) && (loginTokens.ContainsKey(token)))
                {
                    var loginToken = loginTokens[token];
                    if (loginToken != null)
                    {
                        var now = DateTime.Now;
                        if (now < loginToken.Expires)
                        {
                            loginToken.Expires = now.AddMinutes(LOGINTOKEN_TIMEOUT);
                            return loginToken.UserId;
                        }
                        else
                        {
                            throw new ApplicationException("The given token has been expired.");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("The given token is not valid.");
                    }
                }
                else
                {
                    throw new ApplicationException("The given token is not valid.");
                }
            }
        }
        private string AnalyzeToken(string raw, bool isNew)
        {
            string token = string.Empty;
            if (!string.IsNullOrEmpty(raw))
            {
                if (raw.Contains("."))
                {
                    var temp = raw.Split(".".ToCharArray());
                    token = isNew ? temp[1] : temp[0];
                }
                else
                {
                    token = raw;
                }
            }
            return token;
        }
        public string GetCurrentUserName(string stoken)
        {
            lock (loginTokensLock)
            {
                var token = AnalyzeToken(stoken, true);
                if (string.IsNullOrEmpty(token)) throw new ApplicationException("The given token is invalid.");
                if ((loginTokens.Count > 0) && (loginTokens.ContainsKey(token)))
                {
                    var loginToken = loginTokens[token];
                    if (loginToken != null)
                    {
                        var now = DateTime.Now;
                        if (now < loginToken.Expires)
                        {
                            loginToken.Expires = now.AddMinutes(LOGINTOKEN_TIMEOUT);
                            return loginToken.UserName;
                        }
                        else
                        {
                            throw new ApplicationException("The given token has been expired.");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("The given token is not valid. loginTokens[token] is wrong.");
                    }
                }
                else
                {
                    throw new ApplicationException(string.Format("The given token is not valid. loginTokens.count={0} or loginTokens does not contain the token.", loginTokens.Count));
                }
            }
        }
        public int GetCurrentUserId(string token)
        {
            var userId = -1;

            lock (loginTokensLock)
            {
                if (!string.IsNullOrEmpty(token)
                    && (loginTokens.Count > 0)
                    && (loginTokens.ContainsKey(token)))
                {
                    var loginToken = loginTokens[token];
                    if (loginToken != null)
                    {
                        var now = DateTime.Now;
                        if (now < loginToken.Expires)
                        {
                            loginToken.Expires = now.AddMinutes(LOGINTOKEN_TIMEOUT);
                            userId = loginToken.UserId;
                        }
                    }
                }
            }

            return userId;
        }

        public List<string> GetCurrentToken()
        {
            if (loginTokens.Count > 0)
            {
                var result = new List<string>();
                foreach (var token in loginTokens)
                {
                    result.Add(token.Key);
                }
                return result;               
            }

            return null;
        }
    }
}