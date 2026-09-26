namespace SubscriptionOverview.Api.Exceptions
{
    public class UnauthorizedException : Exception
    {
        //401
        public UnauthorizedException(string message) : base(message)
        {

        }
    }
}
