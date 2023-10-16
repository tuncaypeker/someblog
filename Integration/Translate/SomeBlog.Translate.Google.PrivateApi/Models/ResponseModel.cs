using System;
using System.Collections.Generic;
using System.Text;

namespace SomeBlog.Translate.Google.PrivateApi.Models
{
    public class ResponseModel
    {
        public List<Sentences> sentences { get; set; }
    }

    public class Sentences
    {
        public string trans { get; set; }
        public string orig { get; set; }
    }
}
