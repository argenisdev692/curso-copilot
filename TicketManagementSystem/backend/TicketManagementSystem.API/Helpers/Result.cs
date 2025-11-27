namespace TicketManagementSystem.API.Helpers
{
    /// <summary>
    /// Represents the result of an operation, encapsulating success or failure with optional data and error information.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// The value returned if the operation was successful.
        /// </summary>
        public T? Value { get; private set; }

        /// <summary>
        /// The error message if the operation failed.
        /// </summary>
        public string Error { get; private set; } = string.Empty;

        /// <summary>
        /// The error code for categorization.
        /// </summary>
        public string? ErrorCode { get; private set; }

        private Result(bool isSuccess, T? value, string error, string? errorCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a successful result with a value.
        /// </summary>
        /// <param name="value">The result value.</param>
        /// <returns>A successful Result instance.</returns>
        public static Result<T> Success(T value) => new Result<T>(true, value, string.Empty, null);

        /// <summary>
        /// Creates a successful result without a value (for void operations).
        /// </summary>
        /// <returns>A successful Result instance.</returns>
        public static Result<T> Success() => new Result<T>(true, default, string.Empty, null);

        /// <summary>
        /// Creates a failure result with an error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <param name="errorCode">Optional error code.</param>
        /// <returns>A failure Result instance.</returns>
        public static Result<T> Failure(string error, string? errorCode = null) => new Result<T>(false, default, error, errorCode);
    }

    /// <summary>
    /// Represents the result of a void operation, encapsulating success or failure.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// The error message if the operation failed.
        /// </summary>
        public string Error { get; private set; } = string.Empty;

        /// <summary>
        /// The error code for categorization.
        /// </summary>
        public string? ErrorCode { get; private set; }

        private Result(bool isSuccess, string error, string? errorCode)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A successful Result instance.</returns>
        public static Result Success() => new Result(true, string.Empty, null);

        /// <summary>
        /// Creates a failure result with an error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <param name="errorCode">Optional error code.</param>
        /// <returns>A failure Result instance.</returns>
        public static Result Failure(string error, string? errorCode = null) => new Result(false, error, errorCode);
    }
}