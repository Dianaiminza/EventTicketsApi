using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Infrastructure.shared.Serializers;

public static class SerializerSettingsHelper
{
    public static JsonSerializerSettings CamelCase()
    {
        return new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }

    public static JsonSerializerSettings SnakeCase()
    {
        return new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}