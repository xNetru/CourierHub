using CourierHub.Shared.Logging.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CourierHubWebApi.Extensions
{
    public static class ILoggerExtensions
    {
        private static char[] separators = { '.', ' ' };
        public static void CreateLog(this IMyLogger logger, HttpContext context, ulong milliseconds)
        {
            logger.blobData.ContainerBuilder.AddLogs();
            logger.CreatePath(context);
            logger.CreateBlobContext(context, milliseconds);
        }

        private static void CreatePath(this IMyLogger logger, HttpContext context)
        {
            logger.blobData.PathBuilder.AddApplication(Applications.API);
            logger.blobData.PathBuilder.AddDateAndTime(DateTime.Now);
            Endpoint? endpoint = context.GetEndpoint();
            string? stringEndpoint = null;
            if (endpoint != null)
            {
                stringEndpoint = endpoint.ToString();
                if (stringEndpoint != null)
                {
                    string[] splittedEndpoint = stringEndpoint.Split(separators);
                    logger.blobData.PathBuilder.AddController(splittedEndpoint[2]);
                    logger.blobData.PathBuilder.AddMethod(splittedEndpoint[3]);

                }
            }
            if (endpoint == null || stringEndpoint == null)
            {
                logger.blobData.PathBuilder.AddDefaultController();
                logger.blobData.PathBuilder.AddDefaultMethod();
            }
        }

        private static void CreateBlobContext(this IMyLogger logger, HttpContext context, ulong milliseconds)
        {
            logger.blobData.BlobBuilder.AddRequest(context.Request.Body);
            logger.blobData.BlobBuilder.AddOperationTime(milliseconds);
            logger.blobData.BlobBuilder.AddResponse(context.Response.Body);
            logger.blobData.BlobBuilder.AddStatusCode(context.Response.StatusCode);
        }
    }
}
