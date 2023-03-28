using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LazySoccer.Network;
using Cysharp.Threading.Tasks;

public class LayoutRestorePasswordCode : MonoBehaviour
{
    [SerializeField] private FindValidationContainer _container;
    [SerializeField] private TMPro.TMP_Text m_Text;
    [SerializeField] private Button _confirm;

    private AuthTypesOfRequests networkingManager;
    private ManagerPlayerData playerData;

    public void Code(string value)
    {
        playerData.PlayerData.Code = value;
        FirstCheck(value);
    }

    private void Start()
    {
        playerData = ServiceLocator.GetService<ManagerPlayerData>();
        networkingManager = ServiceLocator.GetService<AuthTypesOfRequests>();

        _container = GetComponent<FindValidationContainer>();
        _confirm.onClick.AddListener(SendCodeMesseng);
    }
    
    public void UpdateText()
    {
        return;
        
        if (m_Text != null)
            m_Text.text = $"Enter the confirmation code we sent to {playerData.PlayerData.Email}";
    }

    private void SendCodeMesseng()
    {
        if (_container.isRatification())
            CodeMesseng().Forget();
    }

    private async UniTask CodeMesseng()
    {
       await networkingManager.SendMessage(AuthOfRequests.MailConfirm);
    }

    private void FirstCheck(string value)
    {
        if (value.Length != 0)
            _confirm.interactable = true;
        else
            _confirm.interactable = false;
    }
}
