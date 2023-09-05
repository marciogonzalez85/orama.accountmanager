using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Orama.AccountManager.CrossCutting.Common
{
    public class Result<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public IList<FieldProblemDetail> Errors { get; set; }
        [JsonIgnore] public int StatusCode { get; set; }

        private Result(int statusCode, bool success, T data, IList<FieldProblemDetail> errors)
        {
            Data = data;
            Errors = errors;
            StatusCode = statusCode;
            Success = success;
        }

        public Result()
        {

        }

        public static Result<T> Ok(T? response) => new Result<T>(200, true, response, null);
        public static Result<T> Accepted(T? response) => new Result<T>(202, true, response, null);
        public static Result<T> NoContent() => new Result<T>(204, true, null, null);
        public static Result<T> Created(T? response) => new Result<T>(201, true, response, null);

        public static Result<T> BadRequest(string name, string message) => new Result<T>(400, false, null,
            new List<FieldProblemDetail>()
            {
            new FieldProblemDetail(name, message)
            });

        public static Result<T> BadRequest(ICollection<(string name, string message)> items) => 
            new Result<T>(400, false, null, items.Select(a => new FieldProblemDetail(a.name, a.message)).ToList());

        public static Result<T> NotFound(string name, string message) => new Result<T>(404, false, null,
            new List<FieldProblemDetail>()
            {
            new FieldProblemDetail(name, message)
            });
    }

    public class FieldProblemDetail
    {
        [JsonPropertyName("code")] public string Code { get; set; }

        [JsonPropertyName("message")] public string Message { get; set; }

        public FieldProblemDetail(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public enum ErrorCode
    {
        ProdutoNaoEncontrado,
        OutroErro,
        PayloadInvalido
    }
}
