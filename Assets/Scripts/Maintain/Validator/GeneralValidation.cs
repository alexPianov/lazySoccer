using System;
using Cysharp.Threading.Tasks;
using LazySoccer.Network.Error;
using LazySoccer.Status;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using static LazySoccer.Network.Error.ErrorRequest;

public class GeneralValidation : MonoBehaviour
{
    public Action<ErrorsStatus, string, string> OnActionErrorsStatus;
    [SerializeField] private string _errorText;

    private ErrorsStatus _errorsStatus;

    public ErrorsStatus ErrorsStatus
    {
        get => _errorsStatus;
        set
        {
            _errorsStatus = value;
            OnActionErrorsStatus?.Invoke(_errorsStatus, _errorText, null);
        }
    }

    public string TextError(ErrorsStatus status, string value, string value2 = "")
    {
        var errorText = ErrorTextPrinciple(status, value, value2);
        
        if (string.IsNullOrEmpty(errorText)) return "";
        OpenPopup(status, errorText, value2);
        return errorText;
    }
    
    public void OpenPopup(ErrorsStatus status, string error, string param)
    {
        Debug.Log("Result: " + status + " error: " + error);
        
        if(string.IsNullOrEmpty(error)) return;
        
        _errorText = error;
        ErrorsStatus = status;
    }
    
    public string ErrorTextPrinciple(ErrorsStatus status, string value, string value2 = "")
    {
        switch (status)
        {
            case ErrorsStatus.Name:
                return PrincipleValidation.PrincipleUserName(value);
            case ErrorsStatus.NameNew:
                return PrincipleValidation.PrincipleUserNameNew(value);
            case ErrorsStatus.Email:
                return PrincipleValidation.PrincipleEmail(value);
            case ErrorsStatus.PasswordLogin:
                return PrincipleValidation.PrinciplePasswordLogin(value);
            case ErrorsStatus.PasswordCurrent:
                return PrincipleValidation.PrinciplePasswordCurrent(value);
            case ErrorsStatus.Password:
                return PrincipleValidation.PrinciplePassword(value);
            case ErrorsStatus.ConfirmPassword:
                return PrincipleValidation.PrinciplePassword(value);
            case ErrorsStatus.PasswordAndConfirmPassword:
                _errorText = PrincipleValidation.PrinciplePasswordAndPasswordConfirm(value, value2);
                if (_errorText.Length != 0) { ErrorsStatus = ErrorsStatus.ConfirmPassword; }
                return PrincipleValidation.PrinciplePasswordAndPasswordConfirm(value, value2);
            case ErrorsStatus.Code:
                return PrincipleValidation.PrincipleCode(value);
            case ErrorsStatus.TeamName:
                return PrincipleValidation.PrincipleTeamName(value);
            case ErrorsStatus.ShortTeamName:
                return PrincipleValidation.PrincipleShortName(value);
            case ErrorsStatus.TwoFactorAuth:
                return PrincipleValidation.PrincipleTwoFactorPin(value);

            default: return "";
        }
    }

    // public void ErrorTextRequest(ErrorsStatus status, string error)
    // {
    //     Debug.LogError("Error text request: " + status + " error: " + error);
    //     switch (status)
    //     {
    //          case ErrorsStatus.Name:
    //              OpenPopup(status, error); 
    //              break;
    //          case ErrorsStatus.Email:
    //              OpenPopup(status, error); 
    //              break;
    //          case ErrorsStatus.Password:
    //              OpenPopup(status, error); 
    //              break;
    //          case ErrorsStatus.ConfirmPassword:
    //              OpenPopup(status, error); 
    //              break;
    //          case ErrorsStatus.TwoFactorAuth:
    //              OpenPopup(status, error);
    //              break;
    //         default: 
    //             Debug.LogError(error); 
    //             break;
    //     }
    // }
    
    // public void GetErrorMessageFromServer(ErrorsReg errorsReg)
    // {
    //     if (errorsReg != null)
    //     {
    //         if (errorsReg.errors.UserName != null)
    //         {
    //             ErrorTextRequest(ErrorsStatus.Name, errorsReg.errors.UserName[0]);
    //         }
    //         if (errorsReg.errors.Password != null)
    //         {
    //             ErrorTextRequest(ErrorsStatus.Password, errorsReg.errors.Password[0]);
    //         }
    //         if (errorsReg.errors.Email != null)
    //         {
    //             ErrorTextRequest(ErrorsStatus.Email, errorsReg.errors.Email[0]);
    //         }
    //     }
    // }
    //
    // public void GetErrorMessageFromServer(ValidationErrors errorsReg)
    // {
    //     if (errorsReg != null)
    //     {
    //         foreach (var error in errorsReg.validationErrors)
    //         {
    //             if (error.field.Contains("Code"))
    //             {
    //                 ErrorTextRequest(ErrorsStatus.TwoFactorAuth, error.title);
    //             }
    //             if (error.field.Contains("Password"))
    //             {
    //                 ErrorTextRequest(ErrorsStatus.Password, error.title);
    //             }
    //             if (error.field.Contains("Email"))
    //             {
    //                 ErrorTextRequest(ErrorsStatus.Email, error.title);
    //             }
    //             if (error.field.Contains("UserName"))
    //             {
    //                 ErrorTextRequest(ErrorsStatus.Name, error.title);
    //             }
    //             if (error.field.Contains("Login"))
    //             {
    //                 ErrorTextRequest(ErrorsStatus.Email, error.title);
    //             }
    //         }
    //         
    //         ServiceLocator.GetService<ManagerLoading>().ControlLoading(false);
    //     }
    // }
    //
    // public async UniTask RequestOK(UnityWebRequest result, Action<UnityWebRequest> action)
    // {
    //     switch (result.responseCode)
    //     {
    //         case 200:
    //             Debug.Log("Hello");
    //             action(result);
    //             break;
    //         case 400:
    //             var error400 = JsonUtility.FromJson<ErrorRequest.ServerError>(result.downloadHandler.text);
    //             //GetErrorMessageFromServer(error400);
    //             break;
    //         case 403:
    //             var ValidationErrors = JsonConvert.DeserializeObject<ErrorRequest.ValidationErrors>(result.downloadHandler.text);
    //             GetErrorMessageFromServer(ValidationErrors);
    //             break;
    //         case 404:
    //             break;
    //         default:
    //             Debug.LogError($"Error code: {result.responseCode}");
    //             ServiceLocator.GetService<AuthenticationStatus>().SetAction(StatusAuthentication.Back);
    //             ServiceLocator.GetService<LoadingStatus>().SetAction(StatusLoading.None);
    //             break;
    //     }
    //}
}