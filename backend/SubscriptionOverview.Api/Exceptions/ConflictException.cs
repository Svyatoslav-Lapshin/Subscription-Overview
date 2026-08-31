namespace SubscriptionOverview.Api.Exceptions
{
    public class ConflictException:Exception
    {
        //409
        public ConflictException(string message):base(message)
        {
            
        }

    }
}
