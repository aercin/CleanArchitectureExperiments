using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Models
{
    public class Result<T> : Result
    {
        private Result(T data, bool succeeded, IEnumerable<string> errors) : base(succeeded, errors)
        {
            Data = data;
        }

        public T Data { get; private set; }

        public static Result<T> Success(T data)
        {
            return new Result<T>(data, true, new string[] { });
        }
    }

    public class Result
    {
        public Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; private set; }
        public string[] Errors { get; private set; }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }

        public static Result Success()
        {
            return new Result(true, new List<string>());
        }
    }
}
