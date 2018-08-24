using Estrutura.Model;
using Estrutura.Repository;
using System.Collections.Generic;

namespace Estrutura.Application
{
    public class StatesRules
    {
        public IEnumerable<States> GetAll()
        {
            var repo = new StatesRepository();
            return repo.GetAll();
        }

        public States GetByName(string name)
        {
            var repo = new StatesRepository();
            return repo.GetByName(name);
        }

        public void Insert(States state)
        {
            var repo = new StatesRepository();
            repo.Insert(state);
        }
    }
}
