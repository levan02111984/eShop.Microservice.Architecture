using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public record ProductDTO
    (
        int Id,
        [Required] string Name,
        [Required] decimal Price,
        [Required] int Quantity
    );
}
