using BackOffice.Api.Helpers;
using BackOffice.Api.Models;

namespace BackOffice.Api.Repositories
{
    public class OrderRepository
    {
        private const string File = "orders.json";

        public List<Order> GetAll() => JsonStore.Load<Order>(File);

        public Order GetById(string id)
        {
            foreach (var o in JsonStore.Load<Order>(File))
            {
                if (o.Id == id) return o;
            }
            return null;
        }

        public void Add(Order order)
        {
            var all = JsonStore.Load<Order>(File);
            all.Add(order);
            JsonStore.Save(File, all);
        }

        public void Update(Order order)
        {
            var all = JsonStore.Load<Order>(File);
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].Id == order.Id)
                {
                    all[i] = order;
                    break;
                }
            }
            JsonStore.Save(File, all);
        }
    }
}
