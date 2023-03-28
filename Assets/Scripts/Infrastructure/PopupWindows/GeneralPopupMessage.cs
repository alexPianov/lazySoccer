using System;
using LazySoccer.Network.Error;
using LazySoccer.Utils;
using UnityEngine;
using UnityEngine.Networking;

public class GeneralPopupMessage : MonoBehaviour
{
    public Action<string, bool, string> OnActionMessageStatus { get; set; }

    private ManagerLoading _managerLoading;
    private void Start()
    {
        _managerLoading = ServiceLocator.GetService<ManagerLoading>();
    }

    public void ShowInfo(string message, bool positive = true, string Param1 = null)
    {
        if (string.IsNullOrEmpty(message)) return;
        Debug.Log("ShowInfo: " + message + " | Param: " + Param1);
        OnActionMessageStatus.Invoke(message, positive, Param1);
    }

    public bool ValidationCheck(UnityWebRequest request)
    {
        if (request == null)
        {
            _managerLoading.ControlLoading(false);
            return false;
        }
        
        var validationError = DataUtils.WebResultValidationError(request);

        if (validationError == null)
        {
            return true;
        }
        
        _managerLoading.ControlLoading(false);
        
        if (validationError.titleEnum != ErrorRequest.ErrorName.None)
        {
            ShowInfo(validationError.titleEnum.ToString(), false, validationError.description);
        }
        else
        {
            ShowInfo(validationError.description, false);
        }
        
        return false;
    }
}
