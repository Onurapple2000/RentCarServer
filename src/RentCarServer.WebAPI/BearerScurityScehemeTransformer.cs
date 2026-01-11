using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

internal sealed class BearerSecuritySchemeTransformer
    : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes ??=
            new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            };

        foreach (var operation in document.Paths.Values
    .Where(p => p.Operations is not null)
    .SelectMany(p => p.Operations!.Values))
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            operation.Security.Add(
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference("Bearer")
                    ] = new List<string>()
                });
        }

        return Task.CompletedTask;
    }
}
