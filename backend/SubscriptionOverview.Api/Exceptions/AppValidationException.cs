namespace SubscriptionOverview.Api.Exceptions
{
    public class AppValidationException:Exception
    {
        //400
        public AppValidationException(string message):base(message)
        {
            
        }
    }
}
