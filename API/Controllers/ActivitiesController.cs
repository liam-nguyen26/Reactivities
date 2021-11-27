using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ActivitiesController : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult> GetActivities()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        //activities/id
        [HttpGet("{id}")]
        //thay vi dung ActionResult<Activity> de tra ve type of thing gi do
        // thi dung ActionResult de tra ve HTTP Res
        public async Task<ActionResult> GetActivity(Guid id)
        {
            var result = await Mediator.Send(new Details.Query { Id = id });
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody] Activity activity) //default: body of request, no need to declare from body
        {
            return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}
