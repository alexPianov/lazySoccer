using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class QRcode : MonoBehaviour
{
    [SerializeField] private ManagerPlayerData playerData;
    [SerializeField] private RawImage image;

    [SerializeField] private TMP_Text m_TextCode;
    [SerializeField] private string code;

    [SerializeField] private Button CopyBuffer;

    private void Start()
    {
        playerData = ServiceLocator.GetService<ManagerPlayerData>();
        CopyBuffer.onClick.AddListener(ClickCopeBuffer);
    }
    
    private void ClickCopeBuffer()
    {
        GUIUtility.systemCopyBuffer = m_TextCode.text;
    }
    
    public void QRCode()
    {
        m_TextCode.text = playerData.PlayerData.AuthenticationManualCode;
        var base64 = playerData.PlayerData.AuthenticationBarCodeImage.Split(',')[1];
        image.texture = Base64ToTexture2D(base64);
    }

    [Button]
    public void TryGetQr()
    {
        image.texture = Base64ToTexture2D(code);
    }

    private Texture Base64ToTexture2D(string b64_string)
    {
        Debug.Log(b64_string);
        var b64_bytes = System.Convert.FromBase64String(b64_string);
 
        var tex = new Texture2D(1,1);
        tex.LoadImage( b64_bytes);

        return tex;
    }
}
