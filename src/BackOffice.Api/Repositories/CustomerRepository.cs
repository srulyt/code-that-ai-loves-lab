using BackOffice.Api.Helpers;
using BackOffice.Api.Models;

namespace BackOffice.Api.Repositories
{
    public class CustomerRepository
    {
        private const string File = "customers.json";

        public List<Customer> GetAll() => JsonStore.Load<Customer>(File);

        public Customer GetById(string id)
        {
            // Linear scan repeated in every repository.
            foreach (var c in JsonStore.Load<Customer>(File))
            {
                if (c.Id == id) return c;
            }
            return null;
        }
    }
}
