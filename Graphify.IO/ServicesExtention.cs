using Graphify.IO.Exporters;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Graphify.IO;

public static class ServicesExtention
{
    public static void AddIO(this IServiceCollection services)
    {
        services.AddScoped<IExporter, SVGExporter>();
        services.AddScoped<IExporter, PNGExporter>();
        services.AddScoped<IExporter, GraphifyExporter>();

        services.AddScoped<SVGExporter>();
        services.AddScoped<PNGExporter>();
        services.AddScoped<GraphifyExporter>();
    }
}
