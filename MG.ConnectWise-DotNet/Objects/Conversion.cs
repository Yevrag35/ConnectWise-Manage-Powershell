using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace MG.ConnectWise
{
    public static class Conversion
    {
        #region FIELDS/CONSTANTS
        internal static readonly JsonSerializerSettings Serializer = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTime,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            DefaultValueHandling = DefaultValueHandling.Populate,
            FloatFormatHandling = FloatFormatHandling.DefaultValue,
            FloatParseHandling = FloatParseHandling.Decimal,
            Formatting = Formatting.Indented,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        #endregion

        #region PUBLIC METHODS

        public static T ToCwResult<T>(string jsonString) where T : IJsonObject
        {

            return JsonConvert.DeserializeObject<T>(jsonString, Serializer);
        }

        public static List<T> ToCwResults<T>(string jsonString) where T : IJsonObject
        {
            List<T> list = IsJsonArray(jsonString)
                ? JsonConvert.DeserializeObject<List<T>>(jsonString, Serializer)
                : new List<T>(1)
                {
                    JsonConvert.DeserializeObject<T>(jsonString, Serializer)
                };

            return list;
        }

#if DEBUG

        /// These methods are marked as Obsolete to dissuade use in main code.

        [Obsolete]
        public static object ToCwResultDebug(string jsonString, Type convertTo)
        {
            if (!convertTo.GetInterfaces().Contains(typeof(IJsonObject)))
                throw new InvalidCastException("The parameter for \"convertTo\" does not inherit the interface \"IJsonObject\".");

            MethodInfo otherMeth = typeof(Conversion).GetMethod("ToCwResults", BindingFlags.Public | BindingFlags.Static);
            MethodInfo genMeth = otherMeth.MakeGenericMethod(convertTo);
            return genMeth.Invoke(null, new object[1] { jsonString });
        }

        [Obsolete]
        public static List<object> ToCwResultsDebug(string jsonString, Type convertTo)
        {
            if (!convertTo.GetInterfaces().Contains(typeof(IJsonObject)))
                throw new InvalidCastException("The parameter for \"convertTo\" does not inherit the interface \"IJsonObject\".");

            MethodInfo otherMeth = typeof(Conversion).GetMethod("ToCwResults", BindingFlags.Public | BindingFlags.Static);
            MethodInfo genMeth = otherMeth.MakeGenericMethod(convertTo);
            object result = genMeth.Invoke(null, new object[1] { jsonString });

            if (result is IEnumerable ienum)
            {
                var list = ienum.Cast<object>().ToList();
                return list;
            }
            else
                return null;
        }

#endif

#endregion

        #region BACKEND/PRIVATE METHODS
        private static bool IsJsonArray(string jsonStr)
        {
            var jtok = JToken.Parse(jsonStr);

            return jtok is JArray jar
                ? true
                : false;
        }

        #endregion
    }
}