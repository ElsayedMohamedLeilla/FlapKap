namespace FlapKap.Models.Response
{
    public class ExecutionResponse<T>
    {
        public ExecutionResponse()
        {
        }
        public ExecutionResponse(string lang = "ar")
        {
            Lang = lang;
            Message = lang == "ar" ? "تم بنجاح" : "Done Successfully";
        }
        ResponseStatus _state;
        string Lang;
        public virtual ResponseStatus State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (_state != ResponseStatus.Success && string.IsNullOrEmpty(Message))
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? "Error Has Occurred" : Message;
                }
                else if (_state != ResponseStatus.Success)
                {
                    Message = string.IsNullOrWhiteSpace(Message) || string.IsNullOrEmpty(Message) ? "Sorry Try Again Later" : Message;
                }
            }
        }
        #region props.

        public T Result { get; set; }
        public virtual string MessageCode { get; set; }
        public virtual string Message { get; set; } = "تم بنجاح";
        public virtual List<string> Messages { get; set; }
        Exception _exception;
        public int? TotalCount { get; set; }
        public virtual Exception Exception
        {
            get
            {

                return _exception;
            }
            set
            {
                _exception = value;
            }

        }

        public static void CopyExecutionResponse<T1, T2>(ExecutionResponse<T1> source, ExecutionResponse<T2> destination)
        {
            destination.State = source.State;
            destination.Message = source.Message;
            destination.MessageCode = source.MessageCode;
            destination.Exception = source.Exception;
        }
    }

    #endregion
}

