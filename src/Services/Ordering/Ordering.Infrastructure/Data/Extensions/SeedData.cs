using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Extensions;

internal class SeedData
{
    const string CustomerId1 = "B5843F0C-A935-4EAA-8605-2B04D1B12933";
    const string CustomerId2 = "37D2001B-31BC-4EDD-AAA3-87545B946506";

    const string ProductId1 = "80553F4A-B855-483B-A9EB-CD26B58A8789";
    const string ProductId2 = "38098B55-E4AE-473F-9D87-E102A6DE03E4";
    const string ProductId3 = "60D44CC9-B806-4ADB-B9AC-487449D2D1EC";
    const string ProductId4 = "A9E31498-FC15-425F-B328-260D547BEBCF";

    const string OrderId1 = "5353C312-8307-4312-A03A-BDF1D59333D2";
    const string OrderId2 = "6DD568CF-B3F4-44AB-903A-DBD970B74B50";

    public static IEnumerable<Customer> Customers =>
        [
            Customer.Create(CustomerId.Of(new Guid(CustomerId1)), "John", "john@email.com"),
            Customer.Create(CustomerId.Of(new Guid(CustomerId2)), "Sam", "sam@email.com")
        ];

    public static IEnumerable<Product> Products =>
        [
            Product.Create(ProductId.Of(new Guid(ProductId1)), "IPhone X", 500),
            Product.Create(ProductId.Of(new Guid(ProductId2)), "Samsung 10", 400),
            Product.Create(ProductId.Of(new Guid(ProductId3)), "Huawei Plus", 650),
            Product.Create(ProductId.Of(new Guid(ProductId4)), "Xiaomi Mi", 450)
        ];

    public static IEnumerable<Order> OrdersAndOrderItems
    {
        get
        {
            var address1 = Address.Of("John", "Doe", "john@email.com", "Line", "Country", "State", "ZipCd");
            var address2 = Address.Of("Sam", "Smith", "john@email.com", "Line", "Country", "State", "ZipCd");

            var payment1 = Payment.Of("John", "1234123412341234", "12/28", "355", 1);
            var payment2 = Payment.Of("Sam", "2345234523452345", "06/01", "451", 2);

            var order1 = Order.Create(
                OrderId.Of(new Guid(OrderId1)),
                CustomerId.Of(new Guid(CustomerId1)),
                OrderName.Of("Ord_1"),
                address1,
                address1,
                payment1);

            order1.Add(ProductId.Of(new Guid(ProductId1)), 2, 500);
            order1.Add(ProductId.Of(new Guid(ProductId2)), 1, 400);

            var order2 = Order.Create(
                OrderId.Of(new Guid(OrderId2)),
                CustomerId.Of(new Guid(CustomerId2)),
                OrderName.Of("Ord_2"),
                address2,
                address2,
                payment2);

            order2.Add(ProductId.Of(new Guid(ProductId3)), 1, 650);
            order2.Add(ProductId.Of(new Guid(ProductId4)), 1, 450);

            return [order1, order2];
        }
    }
}
