using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPI.Model
{
    public class ResponseModel<T> where T : class
    {
        public ResponseModel()
        {
            Errors = new List<string>();
        }

        public ResponseModel(T data)
        {
            Errors = new List<string>();
            Data = data;
        }

        public List<string> Errors {get;set;}
        public T Data { get; set; }
    }
}
