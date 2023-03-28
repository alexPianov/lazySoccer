using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading;
using LazySoccer.Status;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using I2.Loc;
using UnityEngine;
using UnityEngine.Networking;
using static LazySoccer.SceneLoading.Infrastructure.ManagerLocalization;

public class CheckVersion : MonoBehaviour
{
    [SerializeField] private string LocalBaseURL = @"https://localhost:7199/Test/SupportedVersions";
    [SerializeField] private string GlobalBaseURL = @"http://192.168.1.33:2525/Test/SupportedVersions";
    [SerializeField] private bool runAsMobilePlatform;
    [SerializeField] private GameObject panelLoadingDesktop;
    [SerializeField] private GameObject panelLoadingMobile;

#if UNITY_EDITOR
    [SerializeField] private bool isLocalURL = true;
#endif
    public string BaseURL
    {
        get
        {
#if UNITY_EDITOR
            if (isLocalURL)
                return LocalBaseURL;
            else
                return GlobalBaseURL;
#else
            return GlobalBaseURL;
#endif
        }
        private set
        {
#if UNITY_EDITOR
            LocalBaseURL = value;
#else
            GlobalBaseURL = value;
#endif
        }
    }
    [SerializeField, Range(1, 5)] private int SecondFirstStart = 2;
    [SerializeField] private TMPro.TMP_Text loadingText;
    [SerializeField] private TMPro.TMP_Text version;

    [SerializeField] private ManagerLoading _managerLoading;
    [SerializeField] private LoadingStatus _loadingStatus;

    [Title("Prefabs")]
    [SerializeField] private GameObject popup;
    async void Start()
    {
        Debug.Log("Start");
        await UniTask.Delay(400);
        
        _managerLoading = ServiceLocator.GetService<ManagerLoading>();
        _loadingStatus = ServiceLocator.GetService<LoadingStatus>();

        await FirstStart();
        version.text = $"Version: {Application.version}";
    }
    private async UniTask FirstStart()
    {
        CheckMobileLoadingScreen();
        _loadingStatus.StatusAction = StatusLoading.Active;
        
        var result = await CheckVersionUpdated();
        if (result)
        {
            ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Language = (Language)PlayerPrefs.GetInt("Language");

            var scenes = ServiceLocator.GetService<LoadingScene>();
            
            scenes.SetMobileMode(runAsMobilePlatform);

            await UniTask.Delay(200);
            
            var task = scenes.AssetLoaderScene("LogInSignUp", StatusAuthentication.LogIn, new UniTask());
            
            await _managerLoading.ActiveteLoading(task);
            _managerLoading.ControlLoading(false);
            
            Destroy(this);
        }
        else
        {
            //Instantiate(popup);
            Debug.Log("Version does not fit");
        }
    }

    private void CheckMobileLoadingScreen()
    {
        var mobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || runAsMobilePlatform;
        
        if (mobile)
        {
            panelLoadingMobile.SetActive(true);
            panelLoadingDesktop.SetActive(false);
        }
        else
        {
            panelLoadingMobile.SetActive(false);
            panelLoadingDesktop.SetActive(true);
        }
    }

    private async UniTask<bool> CheckVersionUpdated()
    {
        var request = UnityWebRequest.Get(BaseURL);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        await request.SendWebRequest();
        
        Version listVersion = JsonConvert.DeserializeObject<Version>(request.downloadHandler.text);

        if (request.responseCode == 200 && ChechVersion(listVersion)) 
            return true;
        else
            return false;
    }
    private bool ChechVersion(Version version)
    {
        var appVersion = Convert.ToDouble
            (Application.version, new NumberFormatInfo { NumberDecimalSeparator = "." });

        if (version.minVersion <= appVersion && appVersion <= version.maxVersion)
            return true;
        
        return false;
    }
}
class Version
{
    public double minVersion;
    public double maxVersion;
}
