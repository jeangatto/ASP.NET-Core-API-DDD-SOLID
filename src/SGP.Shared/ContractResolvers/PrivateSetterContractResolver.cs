using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SGP.Shared.ContractResolvers
{
    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        public PrivateSetterContractResolver(NamingStrategy namingStrategy)
        {
            NamingStrategy = namingStrategy;
        }

        public PrivateSetterContractResolver()
        {
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jProperty = base.CreateProperty(member, memberSerialization);
            if (jProperty.Writable)
            {
                return jProperty;
            }

            var property = member as PropertyInfo;
            jProperty.Writable = property?.SetMethod != null;
            return jProperty;
        }
    }
}