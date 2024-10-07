using Azure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace SparePartsModule
{
    public class SwaggerParameterOperationFilter: IOperationFilter
    {
  
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Add custom parameter descriptions here
            if (context.ApiDescription.ParameterDescriptions.Any())
            {
                foreach (var parameter in context.ApiDescription.ParameterDescriptions)
                {
                    var parameterAttribute = parameter.CustomAttributes().OfType<SwaggerParameterAttribute>().FirstOrDefault();
                    if (parameterAttribute != null)
                    {
                        if (!operation.Parameters.Any(p => p.Name == parameter.Name))
                        {
                            operation.Parameters.Add(new OpenApiParameter
                            {
                                Name = parameter.Name,
                                Description = parameterAttribute.Description,
                             
                                //Required = parameterAttribute.Required,
                              In = ParameterLocation.Header//parameter.Source.ConvertToSwaggerParameterLocation()
                            });

                            //  operation.Description = parameterAttribute.Description;
                        }
                    }
                }
            }
        }
    }
}
