using BaseApp.Application.Common.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [AutoBindRowVersion]
        public byte[] RowVersion { get; set; }
    }
}