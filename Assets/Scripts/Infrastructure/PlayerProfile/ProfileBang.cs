using System.Collections;
using System.Collections.Generic;
using LazySoccer.SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LazySoccer.Windows;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class ProfileBang : MonoBehaviour
{
    [Title("Text")]
    [SerializeField] private TMP_Text m_NamePlayer;
    [SerializeField] private TMP_Text m_NameTeam;
    [SerializeField] private TMP_Text m_NameUnion;
    [SerializeField] private TMP_Text m_Money;
    [SerializeField] private TMP_Text m_CountMatch;
    [SerializeField] private TMP_Text m_CountWin;
    
    [Title("Image")]
    [SerializeField] private Image m_ImagePlayer;
    [SerializeField] private Image m_ImageTeamEmblem;
    [SerializeField] private Image m_ImageUnionEmblem;

    [Title("Buttons")]
    [SerializeField] private Button OpenProfile;
    [SerializeField] private Button AddMoney;

    [Title("Panel")] 
    [SerializeField] private GameObject panelUnion;

    private PopupStatus _statusLogin;
    private ManagerPlayerData _managerPlayerData;

    private void Start()
    {
        _statusLogin = ServiceLocator.GetService<PopupStatus>();
        _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();

        OpenProfile.onClick.AddListener(ClickOpenProfile);
        AddMoney.onClick.AddListener(ClickAddMoney);

        SubscriptionAction();

        FirstUpdate();
    }
    private void ClickOpenProfile()
    {
        _statusLogin.SetAction(StatusPopup.Profile);
    }
    private void ClickAddMoney()
    {
        
    }
    
    private void FirstUpdate()
    {
        m_NamePlayer.text = _managerPlayerData.PlayerHUDs.Name.Value;
        m_NameTeam.text = _managerPlayerData.PlayerHUDs.NameTeam.Value;
        m_Money.text = _managerPlayerData.PlayerHUDs.Balance.Value.ToString();
        m_CountMatch.text = _managerPlayerData.PlayerHUDs.CountMatch.Value.ToString();
        m_CountWin.text = _managerPlayerData.PlayerHUDs.CountWinMatch.Value.ToString();
        
        if (_managerPlayerData.PlayerHUDs.Avatar.Value != null)
        {
            m_ImagePlayer.sprite = _managerPlayerData.PlayerHUDs.Avatar.Value;
        }

        if (_managerPlayerData.PlayerHUDs.NameUnion.Value == null)
        {
            panelUnion.SetActive(false);
        }
        else
        {
            if (_managerPlayerData.PlayerHUDs.NameUnion.Value.Length >= 4)
            {
                m_NameUnion.text = _managerPlayerData.PlayerHUDs.NameUnion.Value;
                panelUnion.SetActive(true);
            }
            else
            {
                panelUnion.SetActive(false);
            }
        }
        
        
        m_ImageTeamEmblem.sprite = _managerPlayerData.PlayerHUDs.TeamEmblem.Value;

        if (_managerPlayerData.PlayerHUDs.UnionEmblem != null)
        {
            m_ImageUnionEmblem.sprite = _managerPlayerData.PlayerHUDs.UnionEmblem.Value;
        }
    }
    private void SubscriptionAction()
    {
        _managerPlayerData.PlayerHUDs.Name.onActionUser += UpdateName;
        _managerPlayerData.PlayerHUDs.NameTeam.onActionUser += UpdateNameTeam;
        _managerPlayerData.PlayerHUDs.NameUnion.onActionUser += UpdateNameUnion;
        _managerPlayerData.PlayerHUDs.Balance.onActionUser += UpdateBalance;
        _managerPlayerData.PlayerHUDs.CountMatch.onActionUser += UpdateCountMatch;
        _managerPlayerData.PlayerHUDs.CountWinMatch.onActionUser += UpdateCountWin;
        _managerPlayerData.PlayerHUDs.Avatar.onActionUser += UpdateAvatar;
        _managerPlayerData.PlayerHUDs.TeamEmblem.onActionUser += UpdateEmblem;
        _managerPlayerData.PlayerHUDs.UnionEmblem.onActionUser += UpdateUnion;
    }
    private void UpdateName(string NamePlayer)
    {
        m_NamePlayer.text = NamePlayer;
    }
    private void UpdateNameTeam(string NameTeam)
    {
        m_NameTeam.text = NameTeam;
    }
    private void UpdateNameUnion(string NameUnion)
    {
        m_NameUnion.text = NameUnion;
         
        if(panelUnion) panelUnion.SetActive(NameUnion != null && NameUnion.Length >= 4);
    }
    private void UpdateBalance(int Balance)
    {
        m_Money.text = Balance.ToString();
    }
    private void UpdateCountMatch(int CountMatch)
    {

        m_CountMatch.text = CountMatch.ToString();
    }
    private void UpdateCountWin(int CountWin)
    {
        m_CountWin.text = CountWin.ToString();
    }
    private void UpdateAvatar(Sprite sprite)
    {
        m_ImagePlayer.sprite = sprite;
    }
    
    private void UpdateEmblem(Sprite sprite)
    {
        if(m_ImageTeamEmblem) m_ImageTeamEmblem.sprite = sprite;
    }
    
    private void UpdateUnion(Sprite sprite)
    {
        if(m_ImageUnionEmblem && sprite) m_ImageUnionEmblem.sprite = sprite;
    }
}
