using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Extensions
{
    public class AppendAuthoriziton : IOperationFilter
    {
        // public AppendAuthoriziton(ApiDescription apiDescription , ISchemaGenerator schemaGenerator , SchemaRepository schemaRepository , MethodInfo methodInfo)
        // {
        // }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
        }
    }
}