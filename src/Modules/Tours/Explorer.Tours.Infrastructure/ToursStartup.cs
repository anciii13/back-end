using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Mappers;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }
    
    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IMapObjectService, MapObjectService>();
        services.AddScoped<ITourPreferenceService, TourPreferenceService>();
        services.AddScoped<ICheckpointService, CheckpointService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IReportedIssuesReviewService, ReportedIssuesReviewService>();
        services.AddScoped<IReportingIssueService, ReportingIssueService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<Equipment>), typeof(CrudDatabaseRepository<Equipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<Explorer.Tours.Core.Domain.MapObject>), typeof(CrudDatabaseRepository<Explorer.Tours.Core.Domain.MapObject, ToursContext>));

        services.AddScoped(typeof(ICrudRepository<TourPreference>), typeof(CrudDatabaseRepository<TourPreference, ToursContext>));
        services.AddScoped(typeof(ICheckpointRepository), typeof(CheckpointDatabaseRepository));
        services.AddScoped(typeof(ICrudRepository<Tour>), typeof(CrudDatabaseRepository<Tour, ToursContext>));
        services.AddScoped(typeof(ITourEquipmentRepository), typeof(TourEquipmentDatabaseRepository));
        services.AddScoped(typeof(ITourRepository), typeof(TourDatabaseRepository));

        services.AddScoped(typeof(ICrudRepository<ReportedIssue>), typeof(CrudDatabaseRepository<ReportedIssue, ToursContext>));

        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("tours"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
    }
}