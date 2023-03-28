using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading.Infrastructure.Customize;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamEmbem : CustomizeTeamEmblem
    {
        public async void Start()
        {
            base.Start();
            await UniTask.Delay(10);
            quickTable.UpdateScrollbarValue(0);
        }
    }
}