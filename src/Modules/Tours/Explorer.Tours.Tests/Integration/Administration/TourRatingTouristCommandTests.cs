﻿using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class TourRatingTouristCommandTests : BaseToursIntegrationTest
{
    public TourRatingTouristCommandTests(ToursTestFactory factory) : base(factory) { }
    
    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = new TourRatingDto
        {
            Rating = 4,
            Comment = "neki komentar",
            TouristId = 1,
            TourId = 1,
            DateTime = new DateTime(),
            Pictures = null
        };


        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourRatingDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TouristId.ShouldNotBe(0);
        result.TourId.ShouldNotBe(0);
        result.TouristId.ShouldBe(newEntity.TouristId);
        result.DateTime.ShouldBe(newEntity.DateTime); // one tourist can't leave more ratings at the same time

        // Assert - Database
        var storedEntity = dbContext.TourRating.FirstOrDefault(i => i.DateTime == newEntity.DateTime);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }   
    
    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourRatingDto
        {
            Rating = 7
        };

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }
    private static TourRatingTouristController CreateController(IServiceScope scope)
    {
        return new TourRatingTouristController(scope.ServiceProvider.GetRequiredService<ITourRatingService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
