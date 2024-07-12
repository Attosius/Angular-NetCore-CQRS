using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromomashInc.Helpers.Extensions;

namespace PromomashInc.Helpers.FunctionalResult
{
    public enum ResultType
    {
        None = 0,
        Success = 1,
        Debug = 2,
        Info = 3,
        Warn = 4,
        Error = 5
    }

    public class Result
    {
        public bool IsOk { get; set; }
        public ResultType Type { get; set; } = ResultType.Success;

        public bool IsSuccess => IsOk;

        public bool IsFailure => !IsOk;

        public Result(bool success = true, string message = null, Exception ex = null)
        {
            IsOk = success;
            Message = message;
            Exception = ex;
        }

        public string Message { get; set; }

        /// <summary>
        /// List of friendly messages (summary validation, etc)
        /// </summary>
        public List<string> ErrorList { get; set; }

        public Exception Exception { get; set; }

        public string GetErrorList()
        {
            var errors = ErrorList
                .ToEmptyIfNullList()
                .AddItem(Message.ToEmptyIfNullString())
                .Where(o => !o.IsNullOrEmpty())
                .ToStringFromEnumerable();
            return errors;
        }

        public virtual string GetFullError()
        {
            var errorList = GetErrorList();
            if (Exception == null)
            {
                return errorList;
            }

            return $"{errorList} | {GetFullExceptionMessage(Exception)}";
        }

        private string GetFullExceptionMessage(Exception ex)
        {
            if (ex == null)
                return string.Empty;
            var sb = new StringBuilder();
            sb.AppendFormat("Message: {0}; Trace: {1}", ex.Message, ex.StackTrace);
            if (ex.InnerException != null)
            {
                sb.AppendFormat("\r\nInner exception: {0}", GetFullExceptionMessage(ex.InnerException));
            }
            return sb.ToString();
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }
        public static Result Fail(Exception ex, string friendlyText = "Inner error")
        {
            return new Result(false, friendlyText, ex);
        }

        public static Result<T> Fail<T>(Exception ex, string friendlyText = "Inner error")
        {
            return new Result<T>(default, false, friendlyText, ex);
        }

        public static Result<T> Fail<T>(List<string> errors, Exception ex, string friendlyText = "Inner error")
        {
            var result = new Result<T>(default, false, friendlyText, ex)
            {
                ErrorList = errors
            };
            return result;
        }

        public static Result Success()
        {
            return new Result();
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value);
        }

        public static Result<Tout> WrapResult<T, Tout>(T value, Func<T, Tout> func)
        {
            try
            {
                var res = new Result<Tout>(func(value));
                return res;
            }
            catch (Exception e)
            {
                return Fail<Tout>(e);
            }
        }

        public static Result<Tout> WrapResult<Tout>(Func<Tout> func)
        {
            try
            {
                var res = new Result<Tout>(func());
                return res;
            }
            catch (Exception e)
            {
                return Fail<Tout>(e);
            }
        }
        public static Result Combine(List<Result> results)
        {
            var failed = new List<Result>();
            foreach (Result result in results)
            {
                if (result.IsFailure)
                {
                    failed.Add(result);
                }
            }

            if (failed.Count > 0)
            {
                var res = Fail<string>(failed.Select(o => o.Exception?.ToString()).ToList(), null,
                    failed.Select(o => o.Message).ToStringFromEnumerable());
                return res;
            }
            return Success();
        }

        public static Result Combine(params Result[] results)
        {
            return Combine(results.ToList());
        }

    }

    public class Result<T> : Result
    {
        public T Value { get; set; }


        public bool IsFailureOrNull => IsFailure || Value == null;

        public Result(
            T value = default,
            bool success = true,
            string message = null,
            Exception ex = null) : base(success, message, ex)
        {
            Value = value;
        }

        public Result(bool success = true, string message = null, Exception ex = null)
            : base(success, message, ex)
        {

        }

        public Result<T> SetErrorList(List<string> listErrors)
        {
            ErrorList = listErrors;
            return this;
        }
        public Result<T> AddToErrorList(string error)
        {
            if (ErrorList.IsNullOrCountZero())
            {
                ErrorList = new List<string>();
            }
            ErrorList.Add(error);
            return this;
        }

        public override string GetFullError()
        {
            var errorList = base.GetFullError();
            if (errorList.IsNullOrEmpty() && Value == null)
            {
                return "Errors is empty and value is null";
            }

            return errorList;
        }

    }
}