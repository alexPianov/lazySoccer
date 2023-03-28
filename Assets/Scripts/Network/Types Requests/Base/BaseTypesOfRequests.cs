using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading;
using LazySoccer.Status;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace LazySoccer.Network
{
    public class BaseTypesOfRequests<T> : MonoBehaviour
    {
        protected NetworkingManager _networkingManager;
        protected GeneralValidation _generalValidation;
        protected AuthenticationStatus _authentication;
        protected ManagerPlayerData _managerPlayer;
        protected ManagerLoading _managerLoading;
        protected ManagerTeamData _managerTeamData;
        protected LoadingScene _loadingScene;
        protected ManagerPayloadBootstrap _managerPayloadBootstrap;
        protected GeneralPopupMessage _generalPopupMessage;
        protected ManagerMarket _managerMarket;

        protected TeamTypesOfRequests _teamTypesOfRequests;

        //[SerializeField] protected virtual BaseDbURL<T> dbURL;

        public virtual async UniTask SendMessage(T type) { }
        //
        // public virtual async UniTask<string> SendMessagePlayerId(T type, int playerId)
        // {
        //     return "";
        // }

        protected virtual void ResultRequest(int code) { }

        protected virtual void Start()
        {
            _authentication = ServiceLocator.GetService<AuthenticationStatus>();
            _generalValidation = ServiceLocator.GetService<GeneralValidation>();
            _managerPlayer = ServiceLocator.GetService<ManagerPlayerData>();
            _managerLoading = ServiceLocator.GetService<ManagerLoading>();
            _loadingScene = ServiceLocator.GetService<LoadingScene>();
            _networkingManager = ServiceLocator.GetService<NetworkingManager>();
            _managerTeamData = ServiceLocator.GetService<ManagerTeamData>();
            _managerPayloadBootstrap = ServiceLocator.GetService<ManagerPayloadBootstrap>();
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            _managerMarket = ServiceLocator.GetService<ManagerMarket>();
            
            _teamTypesOfRequests = ServiceLocator.GetService<TeamTypesOfRequests>();
        }

        public virtual async UniTask<UnityWebRequest> PostRequest(string URL, T type, string URLParam = "", string JSON = "", string Token = "", bool ActiveGlobalLoading = true, int DelayInMMSeconds = 0)
        {
            if(ActiveGlobalLoading) _managerLoading.ControlLoading(true);
            
            var fullUrl = FullURL(URL, type, URLParam);
            Debug.Log("POST: " + fullUrl);
            
            var uwr = new UnityWebRequest(fullUrl, "POST");

            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            if (JSON != null)
                SendOneJSON(JSON, ref uwr);
            if (JSON != null)
                SendOneToken(Token, ref uwr);

            await uwr.Send();

            if(DelayInMMSeconds > 0) await UniTask.Delay(DelayInMMSeconds);
            
            if (ActiveGlobalLoading) _managerLoading.ControlLoading(false);

            if (uwr.error != null)
            {
                Debug.LogError($"Error {uwr.error}");
                return uwr;
            }
            
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"Error Connection {uwr.error}");
                return null;
            }
            
            return uwr;

        }
        
        public virtual async UniTask<UnityWebRequest> GetRequest(string URL, T type, string URLParam = "", string Token = "", bool ActiveGlobalLoading = true)
        {
            if(ActiveGlobalLoading) _managerLoading.ControlLoading(true);

            UnityWebRequest webRequest = UnityWebRequest.Get(FullURL(URL, type, URLParam));

            webRequest.SetRequestHeader("Authorization", $"Bearer {Token}");
            
            Debug.Log("GET: " + FullURL(URL, type, URLParam) + " | Global loading: " + ActiveGlobalLoading);

            var webRequestResult = await webRequest.SendWebRequest();

            if (webRequestResult == null)
            {
                Debug.LogError("Web request error");
                return null;
            }
            
            if (ActiveGlobalLoading) _managerLoading.ControlLoading(false);
            string[] pages = URL.Split('/');
            int page = pages.Length - 1;

            switch (webRequestResult.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(pages[page] + ": Connection Error: " + webRequestResult.error);
                    return null;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequestResult.error);
                    return null;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequestResult.error);
                    return null;
                case UnityWebRequest.Result.Success:
                    return webRequestResult;
            }
            return webRequestResult;
        }
        
        public virtual string FullURL(string URL, T type, string URLParam = "") => URL + URLParam;
        
        private void SendOneJSON(string JSON, ref UnityWebRequest request)
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(JSON);

            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json");
        }
        
        private void SendOneToken(string Token, ref UnityWebRequest request)
        {
            request.SetRequestHeader("Authorization", $"Bearer {Token}");
        }
    }
}
