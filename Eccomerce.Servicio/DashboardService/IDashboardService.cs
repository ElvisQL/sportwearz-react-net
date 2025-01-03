using Eccomerce.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.DashboardService
{
    public interface IDashboardService
    {
        DashboardDTO GetDashboardResume();
    }
}
