namespace UsersApi.Core.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string recordName, string message) : base(message)
        {
            RecordName = recordName;
        }

        public string RecordName { get; set; }
    }
}
