using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.ConnectWise
{
    public interface IJsonObject
    {
        string ToJson();
        string ToJson(IDictionary parameters);
    }
}
