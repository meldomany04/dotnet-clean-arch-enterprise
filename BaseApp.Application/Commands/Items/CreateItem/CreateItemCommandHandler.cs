using AutoMapper;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Interfaces.IRepositories;
using BaseApp.Domain.Entities;
using MediatR;

namespace BaseApp.Application.Commands.Items.CreateItem
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateItemCommandHandler(IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var productExists = await _unitOfWork.ProductRepository.GetProductById(request.ProductId);
            if (productExists is null)
                throw new NotFoundException("Product Id", request.ProductId);

            var item = Item.Create(request.Name, request.ProductId);

            await _unitOfWork.ItemRepository.AddItem(item);
            await _unitOfWork.SaveChangesAsync();
            return item.Id;
        }
    }
}