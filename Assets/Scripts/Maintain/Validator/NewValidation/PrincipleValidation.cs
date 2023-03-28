using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UnityEngine;

public static class PrincipleValidation
{
    private static Regex passwordRex = 
        new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^_&*-.]).{8,64}$");
    private static Regex emailRex = new Regex("^\\S+@\\S+\\.\\S+$");
    private static Regex userName = new Regex("^\\b(?:\\w|-)+\\b$");
    private static Regex teamName = new Regex("^[A-Za-z\\s]{4,32}$");
    private static Regex shortTeamName = new Regex("^[A-Za-z\\s]{3,3}$");
    private static Regex teamNameShort = 
        new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^_&*-.]).{0,3}$");

    public static string PrincipleUserName(string value)
    {
        if (userName.IsMatch(value))
        {
            if(value.Length > 3 && value.Length <= 32)
                return "";
            else 
                return "The user name is invalid";
        }
        else
        return "The user name is invalid";
    }
    
    public static string PrincipleUserNameNew(string value)
    {
        if (userName.IsMatch(value))
        {
            if(value.Length > 3 && value.Length <= 32)
                return "";
            else 
                return "The user name must contain latin words and 4-32 characters";
        }
        else
            return "The user name must contain latin words and 4-32 characters";
    }
    
    public static string PrincipleTeamName(string value)
    {
        if (teamName.IsMatch(value))
        {
            if(value.Length > 3 && value.Length <= 32)
                return "";
            else 
                return "The team name must contain latin words and 4-32 characters";
        }
        else
            return "The team name must contain latin words and and 4-32 characters";
    }
    public static string PrincipleEmail(string value)
    {
        if (emailRex.IsMatch(value))
        {
            if (value.Length > 3 && value.Length <= 64)
                return "";
            else
                return "The email is invalid";
        }
        else
            return "The email is invalid";
    }
    public static string PrinciplePassword(string value)
    {
        if (!passwordRex.IsMatch(value))
        {
            return "The password must contain 8-64 characters, including upper and lower case letters, numbers, special characters, no spaces";
        }
        return "";
    }
    
    public static string PrinciplePasswordLogin(string value)
    {
        if (!passwordRex.IsMatch(value))
        {
            return "The password is incorrect";
        }
        return "";
    }
    
    public static string PrinciplePasswordCurrent(string value)
    {
        if (!passwordRex.IsMatch(value))
        {
            return "The password doesn't’ match";
        }
        return "";
    }

    public static string PrinciplePasswordAndPasswordConfirm(string value1, string value2)
    {
        if (value1 != value2)
        {
            return "Password and confirm password fields must match";
        }
        return "";
    }
    public static string PrincipleCode(string value)
    {
        if (value.Length > 5 && value.Length < 16)
        {
            return "";
        }
        return "The code is invalid";
    }
    
    public static string PrincipleShortName(string value)
    {
        if (shortTeamName.IsMatch(value) && AllCapitalized(value))
        {
            return "";
        }
        else
            return "Three latin capital letters required";
        
        Debug.Log("PrincipleShortName: " + value);
        if (value.Length == 3 && AllCapitalized(value))
        {
            return "";
        }
        
        return "Three latin capital letters required";
    }
    
    public static string PrincipleTwoFactorPin(string value)
    {
        if (value.Length == 6)
        {
            return "";
        }
        
        return "Six digits required";
    }


    private static bool AllCapitalized(string value)
    {
        char[] array=value.ToCharArray();

        foreach (var letter in array)
        {
            if (!char.IsUpper(letter))
            {
                return false;
            }
        }

        return true;
    }
}
