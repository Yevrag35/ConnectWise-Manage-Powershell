﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.ConnectWise
{
    public abstract class BaseResult : IJsonObject
    {
        [JsonExtensionData]
        protected IDictionary<string, object> _extraData;

        public virtual string ToJson()
        {
            var converter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                FloatParseHandling = FloatParseHandling.Double,
                DefaultValueHandling = DefaultValueHandling.Populate,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error
            };
            converter.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            return JsonConvert.SerializeObject(this, converter);
        }

        public virtual string ToJson(IDictionary parameters)
        {
            var camel = new CamelCasePropertyNamesContractResolver();
            var cSerialize = new JsonSerializer
            {
                ContractResolver = camel
            };

            var serializer = new JsonSerializerSettings
            {
                ContractResolver = camel,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DefaultValueHandling = DefaultValueHandling.Populate,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error
            };
            serializer.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

            var job = JObject.FromObject(this, cSerialize);

            string[] keys = parameters.Keys.Cast<string>().ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                job.Add(key, JToken.FromObject(parameters[key], cSerialize));
            }

            return JsonConvert.SerializeObject(job, serializer);
        }
    }
}
