using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using UnityEngine;

namespace Scripts.Infrastructure.Managers
{
    public class ManagerBalanceUpdate : MonoBehaviour
    {
        public async UniTask UpdateBalance(int price, bool globalLoading = true)
        {
            ServiceLocator.GetService<ManagerPlayerData>().PlayerHUDs.Balance.Value -= price;
            
            await ServiceLocator.GetService<OfficeTypesOfRequests>()
                .GetFinancialStatistics(OfficeRequests.Season, globalLoading);
        }

        public async UniTask UpdateBalance(bool globalLoading = true)
        {
            await ServiceLocator.GetService<UserTypesOfRequests>()
                .GetUserRequest(globalLoading);
            
            await ServiceLocator.GetService<OfficeTypesOfRequests>()
                .GetFinancialStatistics(OfficeRequests.Season, globalLoading);
        }
    }
}