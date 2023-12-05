using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AllianzBackEnd.API
{

    public class SwaggerFilter : IDocumentFilter
    {
        public string SwaggerFilterUrl { get; set; }
        public SwaggerFilter(string filterUrl)
        {
            SwaggerFilterUrl = filterUrl;
        }
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(SwaggerFilterUrl))
                return;
            swaggerDoc.Servers = new List<OpenApiServer>()
            {
                new OpenApiServer() { Url = SwaggerFilterUrl }
            };
        }
    }
}
