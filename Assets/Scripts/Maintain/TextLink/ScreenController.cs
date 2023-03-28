using LazySoccer.Status;

public class ScreenController : BaseTextLink<StatusAuthentication>
{
    public override void ChangeEvent()
    {
        ServiceLocator.GetService<AuthenticationStatus>().StatusAction = _status;
    }
}
