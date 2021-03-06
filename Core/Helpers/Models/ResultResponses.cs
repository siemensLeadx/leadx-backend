using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helpers.Models
{
    public class Result
    {
        [JsonPropertyName("is_success")]
        public bool IsSuccess { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

        public Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success(string message) => new Result(true, message);
        public static Result Fail(string message) => new Result(false, message);
    }
    public class Result<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    public class PaginatedResult<T>
    {
        [JsonPropertyName("meta")]
        public MetaData Meta { get; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; }

        public PaginatedResult(IEnumerable<T> data, long count, int pageNumber, int pageSize)
        {
            Data = data;
            var totalNumberOfPages = Math.Ceiling((decimal)count / pageSize);
            Meta = new MetaData
            {
                Next = pageNumber < totalNumberOfPages,
                Previous = pageNumber > 1,
                TotalPages = (int)totalNumberOfPages,
                TotalRecords = count,
                PageIndex = pageNumber
            };
        }
    }

    public class MetaData
    {
        [JsonPropertyName("hasPrevious")]
        public bool Previous { get; set; }

        [JsonPropertyName("hasNext")]
        public bool Next { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total")]
        public long TotalRecords { get; set; }

        [JsonPropertyName("page_index")]
        public int PageIndex { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("errors")]
        public List<ErrorResult> Errors { get; set; }
    }

    public class ErrorResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
