namespace ZillPillService.Domain.DTO.Error
{
    /// <summary>
    /// response object
    /// </summary>
    public class HttpErrorMessage
    {
        /// <summary>
        /// message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        public HttpErrorMessage(string message)
            => Message = message;
    }
}
