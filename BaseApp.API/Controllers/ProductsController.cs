using BaseApp.Application.Commands.Products.CreateProduct;
using BaseApp.Application.Commands.Products.UpdateProduct;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Application.Queries.Products.GetAllProducts;
using BaseApp.Application.Queries.Products.GetProducts;
using BaseApp.Application.Resources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Security.Claims;

namespace BaseApp.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICurrentUserService _currentUser;

        public ProductsController(IMediator mediator, IStringLocalizer<SharedResource> localizer, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _localizer = localizer;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProductsQuery query)
        {
            var result = await _mediator.Send(query);

            Response.Headers.Add("X-Total-Count", result.TotalRecords.ToString());

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                _currentUser.UserId,
                _currentUser.UserName,
                _currentUser.Email,
                _currentUser.DisplayName,
                _currentUser.FirstName,
                _currentUser.LastName,
                _currentUser.Roles,
                _currentUser.IsAuthenticated
            });
        }

    }

}
