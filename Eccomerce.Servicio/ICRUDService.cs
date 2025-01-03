using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio
{
    public interface ICRUDService<T>
    {
        Task<T> Create(T modelo);
        Task<T> Read(int id);
        Task<bool> Update(T modelo);
        Task<bool> Delete(int id);
    }
}
