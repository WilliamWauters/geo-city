﻿using geocity.application.Entities.Trip.Queries;
using geocity.application.User.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geocity.application.Entities.TripUserFavorite.Queries
{
    public class TripUserFavoriteDto
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public TripOverviewDto Trip { get; set; }
        public string UserId { get; set; }
        public UserDto User { get; set; }
    }
}
