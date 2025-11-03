namespace Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Message { get; init; }
    public ResultType Type { get; init; }

    protected Result(bool isSuccess, string? message, ResultType type)
    {
        IsSuccess = isSuccess;
        Message = message;
        Type = type;
    }

    public static Result Success(string? message = null) => new(true, message, ResultType.Success);
    public static Result NoContent(string? message = null) => new(true, message, ResultType.NoContent);
    public static Result NotFound(string? message = null) => new(false, message, ResultType.NotFound);
    public static Result ValidationError(string? message = null) => new(false, message, ResultType.ValidationError);
    public static Result Conflict(string? message = null) => new(false, message, ResultType.Conflict);
    public static Result Error(string? message = null) => new(false, message, ResultType.Error);
}

public enum ResultType
{
    Success,
    Created,
    NoContent,
    NotFound,
    ValidationError,
    Conflict,
    Error
}

public class Result<T> : Result
{
    public T? Value { get; }
    protected Result(bool isSuccess, T? value, string? message, ResultType type)
        : base(isSuccess, message, type)
    {
        Value = value;
    }

    public static Result<T> Success(T value, string? message = null) => new(true, value, message, ResultType.Success);
    public static Result<T> Created(T value, string? message = null) => new(true, value, message, ResultType.Created);
    public new static Result<T> NoContent(string? message = null) => new(true, default, message, ResultType.NoContent); 
    public new static Result<T> NotFound(string? message = null) => new(false, default, message, ResultType.NotFound);
    public new static Result<T> ValidationError(string? message = null) => new(false, default, message, ResultType.ValidationError);
    public new static Result<T> Conflict(string? message = null) => new(false, default, message, ResultType.Conflict);
    public new static Result<T> Error(string? message = null) => new(false, default, message, ResultType.Error);
}
