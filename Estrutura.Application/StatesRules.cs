using Estrutura.Model;
using Estrutura.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Estrutura.Application
{
    public class StatesRules
    {
        public IEnumerable<States> GetAll()
        {
            StatesRepository repo = new StatesRepository();
            return null; // repo.GetAll();
        }

        public States GetByName(string name)
        {
            StatesRepository repo = new StatesRepository();
            return null; // repo.GetByName(name);
        }

        public void Insert(States state)
        {
            StatesRepository repo = new StatesRepository();
            //repo.Insert(state);
        }
    }
}
