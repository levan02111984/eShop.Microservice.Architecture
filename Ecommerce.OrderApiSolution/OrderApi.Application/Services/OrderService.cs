using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System.Net.Http.Json;


namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        //Call ProductAPI using HttpClient ( Add Polly extension )
        //Call API to API Gateway . 
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var getProduct = await httpClient.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }
        //Get User
        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"/api/products/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;

            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return product!;
        }

        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            //Prepare Order
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null || order.Id ==0)
                return null!;

            //Get retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("retry-pipeline-key");

            //Preparing Product
            var productDTO =await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            //Preparing Client
            var appUserDTO = await retryPipeline.ExecuteAsync(async token =>await GetUser(order.ClientId));

            //Populate record order Details
            return new OrderDetailsDTO(
                 order.Id,
                 productDTO.Id,
                 appUserDTO.Id,
                 appUserDTO.Name,
                 appUserDTO.Email,
                 appUserDTO.Address,
                 appUserDTO.TelephoneNumber,
                 productDTO.Name,
                 order.PurchaseQuantity,
                 productDTO.Price,
                 productDTO.Quantity * order.PurchaseQuantity,
                 order.OrderedDate
                 );
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            var currentOrders = await orderInterface.GetOrdersAsync( p=>p.ClientId == clientId );
            if (!currentOrders.Any()) 
                return null!;


            //Convert to DTO
            var (_, _currentOrders) = OrderConversion.FromEntity(null, currentOrders);
            return _currentOrders!;
        }
    }
}
