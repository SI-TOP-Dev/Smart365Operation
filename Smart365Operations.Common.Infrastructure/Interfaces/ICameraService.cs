﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart365Operations.Common.Infrastructure.Models;

namespace Smart365Operations.Common.Infrastructure.Interfaces
{
    public interface ICameraService
    {
        IList<Camera> GetCamerasBy(int customerId);
    }
}
