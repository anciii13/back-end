﻿using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDto, Tour>, ITourService
    {
        private readonly ITourEquipmentRepository _tourEquipmentRepository;
        private readonly ITourRepository _tourRepository;

        public TourService(ICrudRepository<Tour> repository, IMapper mapper, ITourEquipmentRepository tourEquipmentRepository, ITourRepository tourRepository) : base(repository, mapper)
        {
            _tourEquipmentRepository = tourEquipmentRepository;
            _tourRepository = tourRepository;
        }

        public Result<List<TourDto>> GetToursByAuthor(int page, int pageSize, int id) 
        { 
            /*
            var allTours = CrudRepository.GetPaged(page, pageSize);
            List<Tour> toursByAuthor= allTours.Results.Where(t=>t.AuthorId == id).ToList();
            return MapToDto(toursByAuthor);
            */
            List<Tour> toursByAuthor = _tourRepository.GetToursByAuthor(id);
            return MapToDto(toursByAuthor);
        }

        public Result AddEquipment(int tourId, int equipmentId)
        {
            var isExists = _tourEquipmentRepository.Exists(tourId);

            if (!isExists) return Result.Fail(FailureCode.NotFound);

            _tourEquipmentRepository.AddEquipment(tourId, equipmentId);


            return Result.Ok();

        }


    }
}
