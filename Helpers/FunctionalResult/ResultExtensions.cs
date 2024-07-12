using System;
using System.Collections.Generic;

namespace PromomashInc.Helpers.FunctionalResult
{
    public static class ResultExtensions
    {
        public static Result<T> ToSuccessResult<T>(this T result)
        {
            return new Result<T>(result);
        }

        public static Result ToErrorResult(this string friendlyText)
        {
            return new Result(false, friendlyText);
        }

        public static Result<T> ToErrorResult<T>(this Exception ex, string friendlyText = "Service unavailable")
        {
            var errorResult = new Result<T>(false, friendlyText, ex)
            {
                Type = ResultType.Error
            };
            return errorResult;
        }

        public static Result<T> ToErrorResult<T>(this string friendlyText)
        {
            var errorResult = new Result<T>(false, friendlyText, null)
            {
                Type = ResultType.Error
            };
            return errorResult;
        }
        public static Result<T> ToWarnResult<T>(this string friendlyText)
        {
            var errorResult = new Result<T>(false, friendlyText, null)
            {
                Type = ResultType.Warn
            };
            return errorResult;
        }
        public static Result<T> ToEmptySuccessResult<T>(this string friendlyText)
        {
            var errorResult = new Result<T>(true, friendlyText, null)
            {
                Type = ResultType.Success
            };
            return errorResult;
        }

        public static Result<T> ToErrorResult<T>(this List<string> errors, Exception ex = null, string friendlyText = "Service unavailable")
        {
            var result = new Result<T>(false, friendlyText, null)
            {
                ErrorList = errors,
                Type = ResultType.Warn
            };
            return result;
        }

        public static Result<TTarget> To<TFrom, TTarget>(this Result<TFrom> resultFrom, TTarget newValue = default)
        {
            var result = new Result<TTarget>(resultFrom.IsSuccess,
                resultFrom.Message, resultFrom.Exception)
            {
                ErrorList = resultFrom.ErrorList,
                Type = resultFrom.Type
            };
            return result;
        }

    }
}