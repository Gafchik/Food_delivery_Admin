using System;
using System.Collections.Generic;
using System.Text;

namespace Food_delivery_library
{
    interface IRepository<T>
    {
        IEnumerable<T> GetColl();
        void Create(T value);
        void Delete(T value);
        void Update(T value);
        T Get(int Id);
    }
}
