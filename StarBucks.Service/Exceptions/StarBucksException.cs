using System;

namespace StarBucks.Service.Exceptions
{
    public class StarBucksException : Exception
    {
        public int Code { get; set; }

        public StarBucksException(int code, string message) :
            base(message) =>
            this.Code = code;
    }
}
