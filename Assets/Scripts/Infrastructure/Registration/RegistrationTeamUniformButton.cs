using LazySoccer.SceneLoading.Infrastructure.Customize;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamUniformButton : CustomizeTeamUniformButton
    {
        private void Awake()
        {
            ActiveColor = "FFFFFF";
            NeutralColor = "9A9A9A";
        }
    }
}