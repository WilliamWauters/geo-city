﻿using AutoMapper;
using geocity.application.Trip.Queries;
using geocity.infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geocity.application.Entities.Trip.Queries
{
    public class GetTripOverviewQuery : IRequest<TripOverviewDto>
    {
        public Guid Link { get; set; }
    }

    public class GetTripOverviewQueryHandler : IRequestHandler<GetTripOverviewQuery, TripOverviewDto>
    {
        private readonly GeoCityDbContext _context;
        private readonly IMapper _mapper;

        public GetTripOverviewQueryHandler(GeoCityDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TripOverviewDto> Handle(GetTripOverviewQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var trip = await _context.Trips
                .Include(t => t.City)
                .Include(t => t.TripUsers)
                .ThenInclude(t => t.User)
                .SingleOrDefaultAsync(x => x.Link == request.Link);
                var tripDto = _mapper.Map<TripOverviewDto>(trip);
                var ratings = await _context.TripUserRatings.Where(x => x.TripId == trip.Id).ToListAsync();
                int rating = 0;
                if (ratings.Count != 0)
                {
                    tripDto.Rating = (int)Math.Round(ratings.Average(x => x.Rating));
                }
                var duration = _context.Itinaries.Where(x => x.TripId == trip.Id).Sum(x => x.Duration);
                var distance = _context.Itinaries.Where(x => x.TripId == trip.Id).Sum(x => x.Distance);
                var price = _context.ItinaryPointOfInterests.Where(x => x.Itinary.Trip.Id == trip.Id).DefaultIfEmpty().Sum(x => x.Price);
                duration = duration + _context.ItinaryPointOfInterests.Where(x => x.Itinary.Trip.Id == trip.Id).DefaultIfEmpty().Sum(x => x.Duration);
                tripDto.Duration = (int)duration;
                tripDto.Distance = (decimal)distance;
                tripDto.Price = (decimal)price;
                return tripDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
