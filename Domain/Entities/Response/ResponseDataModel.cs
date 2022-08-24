using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Response
{
    public class ResponseDataModel<T> : ResponseModel
    {
        public T Data { get; set; }
    }
}
