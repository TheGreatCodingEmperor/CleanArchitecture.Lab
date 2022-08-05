using System;
using System.Collections.Generic;

namespace Masstransit.Test.Components.Models.Exceptions
{
    public class BadRequestViewModel {
        public string Title { get; set; }
        public int Status {get;set;}
        public string Message { get; set; }
    }
}