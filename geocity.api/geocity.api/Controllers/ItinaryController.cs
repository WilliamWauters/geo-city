﻿using geocity.application.Itinary.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace geocity.api.Controllers
{
    public class ItinaryController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ItinaryDto>> Details(Guid id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetItinaryQuery { Id = id }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}