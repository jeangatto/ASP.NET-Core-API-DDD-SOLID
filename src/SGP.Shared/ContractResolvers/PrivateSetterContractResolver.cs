using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace SGP.Shared.ContractResolvers
{
    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        public PrivateSetterContractResolver(NamingStrategy namingStrategy)
        {
            NamingStrategy = namingStrategy;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jProperty = base.CreateProperty(member, memberSerialization);
            if (jProperty.Writable)
            {
                return jProperty;
            }

            jProperty.Writable = member.IsPropertyWithSetter();
            return jProperty;
        }
    }

    internal static class MemberInfoExtensions
    {
        internal static bool IsPropertyWithSetter(this MemberInfo member)
        {
            var property = member as PropertyInfo;
            return property?.SetMethod != null;
        }
    }
}
