using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;

namespace MG.ConnectWise
{
    public class Password : IPassword
    {
        private string _text;

        string IPassword.Text => _text;

        public Password(string pass) => _text = pass;

        public Password(SecureString ss) => _text = this.ConvertSecureToPlain(ss);

        public static implicit operator Password(string str) => new Password(str);
        public static implicit operator Password(SecureString ss) => new Password(ss);
        public static implicit operator Password(PSCredential psCreds) => new Password(psCreds.Password);

        private string ConvertSecureToPlain(SecureString ss)
        {
            IntPtr pointer = Marshal.SecureStringToBSTR(ss);
            string result = Marshal.PtrToStringAuto(pointer);
            Marshal.ZeroFreeBSTR(pointer);

            return result;
        }
    }
}
