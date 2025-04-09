namespace RoyPhishingProj.BusinessLogicLayer.Helpers
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public T Data { get; }

        private Result(bool isSuccess, T data, string error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public static Result<T> Success(T data) => new(true, data, null);
        public static Result<T> Fail(string error) => new(false, default, error);
    }
}
