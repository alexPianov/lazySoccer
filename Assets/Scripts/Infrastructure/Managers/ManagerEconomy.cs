using LazySoccer.SceneLoading.Buildings;
using UnityEngine;

namespace Scripts.Infrastructure.Managers
{
    public class ManagerEconomy : MonoBehaviour
    {
        [SerializeField] private Economy Economy;

        public Economy GetEconomy()
        {
            return Economy;
        }
    }
}