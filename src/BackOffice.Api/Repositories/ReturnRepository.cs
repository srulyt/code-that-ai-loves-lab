using BackOffice.Api.Helpers;
using BackOffice.Api.Models;

namespace BackOffice.Api.Repositories
{
    public class ReturnRepository
    {
        private const string File = "returns.json";

        public List<Return> GetAll() => JsonStore.Load<Return>(File);

        public Return GetById(string id)
        {
            foreach (var r in JsonStore.Load<Return>(File))
            {
                if (r.Id == id) return r;
            }
            return null;
        }

        public void Add(Return ret)
        {
            var all = JsonStore.Load<Return>(File);
            all.Add(ret);
            JsonStore.Save(File, all);
        }
    }
}
