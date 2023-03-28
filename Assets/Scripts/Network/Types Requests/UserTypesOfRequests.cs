using Cysharp.Threading.Tasks;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    public class UserTypesOfRequests : BaseTypesOfRequests<GetUserOfRequests>
    {
        [SerializeField] private GetUserDbURL dbURL;
        public override string FullURL(string URL, GetUserOfRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        
        public async UniTask GetUserRequest(bool globalLoading = true)
        {
            var result = await GetRequest
                (_networkingManager.BaseURL, GetUserOfRequests.GetUser, 
                    Token: _managerPlayer.PlayerData.Token,
                    ActiveGlobalLoading: globalLoading);
            
            var a = DataUtils.CreateFromJSON<UserData>(result.downloadHandler.text);
            
            _managerPlayer.PlayerHUDs.AddParam(a);

            _managerPlayer.PlayerData.UserName = a.userName;
            _managerPlayer.PlayerData.UserId = a.id;
            _managerPlayer.PlayerData.FreeNameChange = a.isFreeNameChange;
        }
    }
}
