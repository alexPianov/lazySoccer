using Project.Security;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LazySoccer.SceneLoading.Infrastructure.ManagerLocalization;

[CreateAssetMenu(fileName = "DataPlayer", menuName = "Player/DataPlayers", order = 1)]
public class PlayerData : ScriptableObject
{
    private B64X _b64x = new B64X();
    [Title("Base Attribute")]
    [SerializeField]
    private string m_Token;
    public string Token 
    {
        get => m_Token;
        set => m_Token = value;
    }
    
    [SerializeField]
    private string m_UserId;
    public string UserId 
    {
        get => m_UserId;
        set => m_UserId = value;
    }
    
    [SerializeField]
    private int m_TeamId;
    public int TeamId 
    {
        get => m_TeamId;
        set => m_TeamId = value;
    }
    
    [SerializeField]
    private string m_Email;
    public string Email
    {
        get => m_Email;
        set { m_Email = value; }
    }

    [SerializeField]
    private string m_UserName;
    public string UserName
    {
        get => m_UserName;
        set => m_UserName = value;
    }
    [SerializeField]
    private string m_Password;
    public string Password
    {
        get => m_Password;
        set { m_Password = value; }
    }

    [SerializeField]
    private string m_PasswordConfirm;
    public string PasswordConfirm
    {
        get => m_PasswordConfirm;
        set => m_PasswordConfirm = value;
    }
    [SerializeField]
    private string m_Code;
    public string Code
    {
        get => m_Code;
        set => m_Code = value;
    }
    [SerializeField]
    private string m_Pin;
    public string Pin
    {
        get => m_Pin;
        set => m_Pin = value;
    }
    
    [SerializeField]
    private int m_Season;
    public int Season
    {
        get => m_Season;
        set => m_Season = value;
    }

    [Title("Authentication")]
    [SerializeField] private string authenticationBarCodeImage;
    public string AuthenticationBarCodeImage
    {
        get => authenticationBarCodeImage;
        set => authenticationBarCodeImage = value;
    }
    [SerializeField] private string authenticationManualCode;
    public string AuthenticationManualCode
    {
        get => authenticationManualCode;
        set => authenticationManualCode = value;
    }
    
    [SerializeField]
    private bool m_isFreeNameChange;
    public bool FreeNameChange
    {
        get => m_isFreeNameChange;
        set => m_isFreeNameChange = value;
    }

    [Title("Language")]
    [SerializeField] private BaseAction<Language> m_Language;
    public Language Language
    {
        get => m_Language.Value;
        set
        {
            m_Language.Value = value;
            PlayerPrefs.SetInt("Language", (int)value);
        }
    }

    private string m_RefreshToken;
    public string RefreshToken 
    {
        get => m_RefreshToken;
        set => m_RefreshToken = value;
    }
    
    [Title("Two Factor Auth")]
    [SerializeField] private BaseAction<bool> m_2FA;
    public bool TwoFactorAuth
    {
        get => m_2FA.Value;
        set => m_2FA.Value = value;
    }
}
