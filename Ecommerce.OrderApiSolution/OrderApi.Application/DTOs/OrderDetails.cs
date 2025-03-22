﻿using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDetails
    (
        [Required] int OrderId,
        [Required] int ProductId,
        [Required] int Client,
        [Required, EmailAddress] string Email,
        [Required] string TelephoneNumber,
        [Required] string ProductName,
        [Required] int PurchaseQuantity,
        [Required , DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderedDate
    );
    
}
