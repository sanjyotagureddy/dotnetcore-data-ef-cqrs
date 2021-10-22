using AutoMapper;
using Product.Application.Features.Commands.AddProduct;
using Product.Application.Features.Commands.UpdateProduct;

namespace Product.Application.Mappings
{
  public class ProductProfile : Profile
  {
    public ProductProfile()
    {
      CreateMap<Domain.Entities.Product, AddProductHandler>().ReverseMap();
      CreateMap<Domain.Entities.Product, UpdateProductCommand>()
        //.ForMember(x => x.CreatedBy, opt => opt.DoNotValidate())
        //.ForSourceMember(x => x.CreatedDate, opt => opt.DoNotValidate())
        //.ForSourceMember(x => x.Sku, opt => opt.DoNotValidate())

        .ReverseMap();
    }
  }
}