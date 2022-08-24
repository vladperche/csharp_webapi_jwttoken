using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Response
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
