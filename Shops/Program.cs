using Shops.Services;

namespace Shops
{
    internal class Program
    {
        private static void Main()
        {
            var manager = new ShopManager();
            var uIManager = new ShopUIManager();
            var service = new ShopsService(manager, uIManager);
            service.Run();
        }
    }
}
