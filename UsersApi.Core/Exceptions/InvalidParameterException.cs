namespace UsersApi.Core.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string moduleName, string message) : base(message) 
        { 
            ModuleName = moduleName;
        }

        public string ModuleName { get; private set; }
    }
}
