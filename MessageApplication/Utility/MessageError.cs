namespace Server.Diagnostic

{
    public class MessageError
    {
        public string Message { get; set; }
        public string Stack { get; set; }
        public MessageError(string msg, string stack)
        {
            this.Message = msg;
            this.Stack = stack;
        }
    }
}
