﻿using geocity.infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geocity.application.Entities.PointOfInterest.Commands.Create
{
    public class CreatePointOfInterestCommand : IRequest<Guid>
    {
        public string OsmId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsSuggestion { get; set; }
    }

    public class CreatePointOfInterestCommandHandler : IRequestHandler<CreatePointOfInterestCommand, Guid>
    {
        private readonly IMediator _mediator;
        private readonly GeoCityDbContext _context;
        public CreatePointOfInterestCommandHandler(IMediator mediator, GeoCityDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Guid> Handle(CreatePointOfInterestCommand request, CancellationToken cancellationToken)
        {
            var entity = new geocity.domain.Entities.PointOfInterest();
            entity.OsmId = request.OsmId;
            entity.Name = request.Name;
            entity.Category = request.Category;
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;
            entity.IsSuggestion = request.IsSuggestion;
            _context.PointOfInterest.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
