using LazySoccer.Status; 

namespace LazySoccer.Windows
{
    public class StadiumLeagueWindows : BaseWindows<StatusStadiumTournament>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<StadiumStatusTournament>();
            base.Awake();
        }
    }
}