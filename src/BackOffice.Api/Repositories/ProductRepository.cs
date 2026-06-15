using BackOffice.Api.Helpers;
using BackOffice.Api.Models;

namespace BackOffice.Api.Repositories
{
    public class ProductRepository
    {
        private const string File = "products.json";

        public List<Product> GetAll() => JsonStore.Load<Product>(File);

        public Product GetById(string id)
        {
            foreach (var p in JsonStore.Load<Product>(File))
            {
                if (p.Id == id) return p;
            }
            return null;
        }
    }
}
