using System.Collections.Generic;

#region Errors Register
public class Errors
{
    public List<string> Email;
    public List<string> Password;
    public List<string> UserName;
}
public class ErrorsReg
{
    public Errors errors;
    public string type;
    public string title;
    public int status;
    public string traceId;
}
#endregion

#region TwoFactorAuth
public class TwoFactorAuthentication
{
    public string authenticationBarCodeImage;
    public string authenticationManualCode;
}
#endregion

#region Player

public class PlayerIdentificator
{
    public int playerId;
}

public class Amount
{
    public int amount;
}

public class TransactionId
{
    public string signature;
}

#endregion

#region Change Player
public class ChangePassword
{
    public string password;
    public string newPassword;
}
public class ChangeName
{
    public string userName;
}
public class ChangeEmail
{
    public string email;
    public string password;
    public string code;
}
#endregion