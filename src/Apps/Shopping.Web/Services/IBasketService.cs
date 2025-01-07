using System.Net;

namespace Shopping.Web.Services;

public interface IBasketService
{
    [Get("/basket-service/baskets/{userName}")]
    Task<GetBasketResponse> GetBasket(string userName);

    [Post("/basket-service/baskets")]
    Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

    [Delete("/basket-service/baskets/{userName}")]
    Task<DeleteBasketResponse> DeleteBasket(string userName);

    [Post("/basket-service/baskets/checkout")]
    Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);

    public async Task<ShoppingCartModel> LoadUserBasket()
    {
        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = "swn";
        ShoppingCartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(userName);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            basket = new ShoppingCartModel
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }
}
