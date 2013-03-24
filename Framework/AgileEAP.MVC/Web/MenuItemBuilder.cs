using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Data;
using AgileEAP.Infrastructure.Domain;

namespace AgileEAP.MVC
{
    public class MenuItemBuilder
    {
        private readonly IRepository<string> repository;
        public MenuItemBuilder(IRepository<string> repository)
        {
            this.repository = repository;
        }

        public void AddMenuItem(Resource resource)
        {
            if (resource != null)
                repository.SaveOrUpdate(resource);
        }
    }
}
