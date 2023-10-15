namespace Kris.Server
{
    public class ServiceBase : IServiceBase
    {
        protected string _errorMessage;

        public string GetErrorMessage()
        {
            return _errorMessage;
        }

        protected T SetErrorMessage<T>(string errorMessage, T result = default)
        {
            _errorMessage = errorMessage;
            return result;
        }
    }
}
