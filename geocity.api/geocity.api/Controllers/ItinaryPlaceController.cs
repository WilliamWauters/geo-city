﻿//using geocity.application.Composit;
//using geocity.application.ItinaryPlace.Commands.CreateItinaryPlace;
//using geocity.application.ItinaryPlace.Commands.DeleteItinaryPlace;
//using geocity.application.ItinaryPlace.Commands.UpdateItinaryPlace;
//using geocity.application.ItinaryPlace.Queries;
//using geocity.application.Place.Commands.CreatePlace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace geocity.api.Controllers
{
    public class ItinaryPlaceController : ApiControllerBase
    {
        //// GET: CityController/Details/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ItinaryPointOfInterestDto>> Details(int id)
        //{
        //    return await Mediator.Send(new GetItinaryPlaceQuery { Id = id });
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> PostAsync(AddPlaceToItinaryDto placeData)
        //{
        //    // First create the city if not already
        //    var placeId = await Mediator.Send(new CreatePlaceCommand
        //    {
        //        Name = placeData.Name,
        //        Latitude = placeData.latitude,
        //        Longitude = placeData.Longitude,
        //    });

        //    // Second create the Trip itself
        //    var itinaryPlaceId = await Mediator.Send(new CreateItinaryPlaceCommand
        //    {
        //        PlaceId = placeId,
        //        ItinaryId = placeData.ItinaryId,
        //        Name = placeData.Name,
        //        Description = placeData.Description,
        //        Duration = placeData.Duration,
        //        Price = placeData.Price
        //    });

        //    return itinaryPlaceId;
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<ItinaryPlaceDto>> UpdatePlace(int id, UpdateItinaryPlaceCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
            
        //    return await Mediator.Send(command); ;
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<int>> RemovePlace(int id)
        //{
        //    await Mediator.Send(new DeleteItinaryPlaceCommand { Id = id });
        //    return id;
        //}
    }
}
