using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MG.ConnectWise
{
    public static class IntegratorAuth
    {
        public static string ConvertToAuth(string company, string publicKey, IPassword password)
        {
            string authFormat = string.Format("{0}+{1}:{2}", company, publicKey, password.Text);
            byte[] encBytes = Encoding.UTF8.GetBytes(authFormat);
            return Convert.ToBase64String(encBytes);
        }
    }
}
