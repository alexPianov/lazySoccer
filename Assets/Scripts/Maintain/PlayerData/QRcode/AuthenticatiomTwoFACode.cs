using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="QR", menuName = "DbData/QR", order = 3)]
public class AuthenticatiomTwoFACode : ScriptableObject
{
    [SerializeField] private string _byteCodeImage;
    [SerializeField] private string _code;



    private string Texture2DToBase64(Texture2D texture)
    {
        byte[] imageData = texture.EncodeToPNG();
        return Convert.ToBase64String(imageData);
    }

    private Texture2D Base64ToTexture2D(string encodedData)
    {
        encodedData = encodedData.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "")
            .Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");

        byte[] imageData = Convert.FromBase64String(encodedData);
        int width, height;
        GetImageSize(imageData, out width, out height);

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false, true);
        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.filterMode = FilterMode.Point;
        texture.LoadImage(imageData);

        return texture;
    }

    private void GetImageSize(byte[] imageData, out int width, out int height)
    {
        width = ReadInt(imageData, 3 + 15);
        height = ReadInt(imageData, 3 + 15 + 2 + 2);
        Debug.Log($"Width: {width} Height: {height}");
    }

    private int ReadInt(byte[] imageData, int offset)
    {
        return (imageData[offset] << 8) | imageData[offset + 1];
    }
}
