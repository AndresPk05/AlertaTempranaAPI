namespace AlertaTempranaAPI.Layers.Dtos
{
    public class Result<T>
    {
        private T? value;
        private bool successful;
        private string? message;

        private Result(T value)
        {
            this.value = value;
            successful = true;
            message = string.Empty;
        }

        private Result(bool successful, string message){
            this.successful = successful;
            this.message = message;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Error(string message)
        {
            return new Result<T>(false, message);
        }

        public T? Value => successful ? value : default;
        public bool Successful => successful;
        public string? Message => message;
    }
}
