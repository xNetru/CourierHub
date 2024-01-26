namespace CourierHubWebApi.Errors {
    public class ApiError {
        public struct Error {
            public int StatusCode { get; set; }
            public string? Message { get; set; }
            public string? Title { get; set; }
        }

        private List<Error> _errors = new();
        public ICollection<Error> Errors { get => _errors; }
        public Error First {
            get { Error? first = _errors.FirstOrDefault(); if (first == null) return DefaultInternalServerError.Errors.First(); return (Error)first; }
        }
        public ApiError(int StatusCode, string? Message = null, string? Title = null) {
            _errors.Add(new Error { StatusCode = StatusCode, Message = Message, Title = Title });
        }
        public void Add(Error error) {
            _errors.Add(error);
        }
        public static ApiError DefaultInternalServerError { get => new ApiError(StatusCodes.Status500InternalServerError, null, "Internal server error."); }
    }
}
