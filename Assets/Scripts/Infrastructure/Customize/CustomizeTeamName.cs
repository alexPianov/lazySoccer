using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Customize
{
    public class CustomizeTeamName : MonoBehaviour
    {
        [SerializeField] private FindValidationContainer _validationContainer;
        
        protected ManagerPlayerData managerPlayerData;
        private OfficeTypesOfRequests _officeTypesOfRequests;

        protected void Start()
        {
            _validationContainer = GetComponent<FindValidationContainer>();

            managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            _officeTypesOfRequests = ServiceLocator.GetService<OfficeTypesOfRequests>();
        }

        protected string newName = "";
        protected string newShortName = "";

        public void ClearNameStrings()
        {
            var fields = GetComponentsInChildren<TMP_InputField>();
            
            foreach (var input in fields)
            {
                input.text = null;
            }
            
            newName = "";
            newShortName = "";
        }

        public async UniTask<bool> Save()
        {
            if (_validationContainer.isRatification())
            {
                return await StartNameChanging();
            }

            return false;
        }

        private async UniTask<bool> StartNameChanging()
        {
            Debug.Log("Change name to: " + newName);
            
            var success = await _officeTypesOfRequests
                .POST_ChangeTeamName(OfficeRequests.ChangeTeamName, newName, newShortName);
            
            if (success)
            {
                managerPlayerData.PlayerHUDs.NameTeam.Value = newName;
                managerPlayerData.PlayerHUDs.NameShortTeam.Value = newShortName;
                
                return true;
            }

            return false;
        }
    }
}