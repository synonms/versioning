using System;
using Swashbuckle.AspNetCore.Annotations;

namespace Synonms.Versioning.Swashbuckle
{
    public class VersionableSwaggerSchemaAttribute : SwaggerSchemaAttribute
    {
        public VersionableSwaggerSchemaAttribute(Type type, string description = null) : base($"@[{type.AssemblyQualifiedName}]@{description ?? string.Empty}")
        {
        }
    }
}
