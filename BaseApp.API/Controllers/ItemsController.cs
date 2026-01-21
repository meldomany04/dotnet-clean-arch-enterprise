using BaseApp.Application.Commands.Items.CreateItem;
using BaseApp.Application.Commands.Products.DeleteProduct;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Application.Queries.Items.GetAllItems;
using BaseApp.Application.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseApp.API.Controllers
{
    [ApiController]
    [Route("api/Items")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICurrentUserService _currentUser;

        public ItemsController(IMediator mediator, IStringLocalizer<SharedResource> localizer, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _localizer = localizer;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllItemsQuery());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string rowVersion)
        {
            await _mediator.Send(new DeleteItemCommand { Id = id, RowVersion = Convert.FromBase64String(rowVersion) });
            return NoContent();
        }

    }

}
