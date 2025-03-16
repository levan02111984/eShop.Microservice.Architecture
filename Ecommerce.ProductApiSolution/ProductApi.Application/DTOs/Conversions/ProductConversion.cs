using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversion
    {

        //Mapping DTOs To Product Entity
        public static Product ConvertToEntity(ProductDTO productDTO) => new()
        { 
            Id = productDTO.Id,
            Name = productDTO.Name,
            Quantity = productDTO.Quantity,
            Price = productDTO.Price
        };


        //Constant Fileds
        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product,IEnumerable<Product>? products)
        {
            //return single entity
            if(product is not null || products is null)
            {
                var singleProduct = new ProductDTO(
                        product!.Id,
                        product.Name,
                        product.Quantity,
                        product.Price
                    );

                return (singleProduct, null);
            }

            //return list
            if(products is not null || product is null)
            {
                var _product = products!.Select(p =>
                    new ProductDTO(p.Id, p.Name!,p.Quantity,p.Price)
                ).ToList();

                return (null, _product);
            }

            return (null, null);
        }
        
    }
}
