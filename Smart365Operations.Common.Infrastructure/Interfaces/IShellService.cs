using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;

namespace Smart365Operations.Common.Infrastructure.Interfaces
{
    public interface IShellService
    {
        void ShowShell(string uri, NavigationParameters navigationParameters);
    }
}
