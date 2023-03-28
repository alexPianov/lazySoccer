using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using LazySoccer.Popup;
using LazySoccer.SceneLoading;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ProfileLayoutProfile : MonoBehaviour
{
    [Title("Buttons")]
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonNFTs;
    [SerializeField] private Button buttonWallet;
    [SerializeField] private Button buttonPicture;
    [SerializeField] private Button buttonEmail;
    [SerializeField] private Button buttonPass;
    [SerializeField] private Button buttonTwoFA;
    [SerializeField] private Button buttonNickname;
    [SerializeField] private Button buttonLanguage;
    [SerializeField] private Button buttonLogOut;
    [SerializeField] private Button buttonRemoveUser;
    
    private AuthenticationStatus _authStatus;
    private ModalWindowStatus _statusModalWin;
    private PopupStatus _statusLogin;
    private LoadingScene _loadingScene;
    private QuestionPopup _questionPopup;
    private CommandTypesOfRequests _commandTypesOfRequests;

    private void Start()
    {
        _questionPopup = ServiceLocator.GetService<QuestionPopup>();
        _statusModalWin = ServiceLocator.GetService<ModalWindowStatus>();
        _authStatus = ServiceLocator.GetService<AuthenticationStatus>();
        _statusLogin = ServiceLocator.GetService<PopupStatus>();
        _loadingScene = ServiceLocator.GetService<LoadingScene>();
        _commandTypesOfRequests = ServiceLocator.GetService<CommandTypesOfRequests>();

        buttonClose.onClick.AddListener(ClickClose);
        buttonNFTs.onClick.AddListener(ClickNFTs);
        buttonWallet.onClick.AddListener(ClickWallet);
        buttonPicture.onClick.AddListener(ClickPicture);
        buttonEmail.onClick.AddListener(ClickEmail);
        buttonPass.onClick.AddListener(ClickPassword);
        buttonTwoFA.onClick.AddListener(ClickTwoFA);
        buttonNickname.onClick.AddListener(ClickNickname);
        buttonLanguage.onClick.AddListener(ClickLanguage);
        buttonLogOut.onClick.AddListener(LogOut);
        buttonRemoveUser.onClick.AddListener(RemoveUser);
    }

    private void ClickClose()
    {
        _statusLogin.SetAction(StatusPopup.None);
    }

    private void ClickNFTs()
    {
        _statusModalWin.SetAction(StatusModalWindows.NFT);
    }
    private void ClickWallet()
    {
        _statusModalWin.SetAction(StatusModalWindows.Wallet);
    }

    private void ClickPicture()
    {
        _statusModalWin.SetAction(StatusModalWindows.ProfilePicture);
    }

    private void ClickEmail()
    {
        _statusModalWin.SetAction(StatusModalWindows.ChangeEmail);
    }

    private void ClickPassword()
    {
        _statusModalWin.SetAction(StatusModalWindows.ChangePassword);
    }
    
    private void ClickTwoFA()
    {
        _statusModalWin.SetAction(StatusModalWindows.Apply2FA);
    }

    private void ClickNickname()
    {
        _statusModalWin.SetAction(StatusModalWindows.ChangeName);
    }

    private void ClickLanguage()
    {
        _statusModalWin.SetAction(StatusModalWindows.ChangeLanguage);
    }

    private async void LogOut()
    {
        var result = await _questionPopup.OpenQuestion("You will be logged out", "Are you sure?", "Log out");

        if (result)
        {
            _statusLogin.SetAction(StatusPopup.None);
            _authStatus.SetAction(StatusAuthentication.LogIn);
            await _loadingScene.AssetLoaderScene("LogInSignUp", StatusBackground.Active, new UniTask());
        }
    }
    
    private async void RemoveUser()
    {
        var result = await _questionPopup.OpenQuestion("User will be removed", "Are you sure?", "Remove user");

        if (result)
        {
            await _commandTypesOfRequests.SendMessage(CommandRequests.DeleteUserTest);
        
            _statusLogin.SetAction(StatusPopup.None);
            _authStatus.SetAction(StatusAuthentication.LogIn);
            await _loadingScene.AssetLoaderScene("LogInSignUp", StatusBackground.Active, new UniTask());
        }
    }
}
