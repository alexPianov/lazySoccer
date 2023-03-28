using UnityEngine;

public class ManagerPlayerData : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData 
    { 
        get => _playerData;
        private set => _playerData = value; 
    }
    
    [SerializeField] private PlayerHUD _playerHUD;
    public PlayerHUD PlayerHUDs
    {
        get => _playerHUD;
        private set => _playerHUD = value;
    }
    void Awake()
    {
        if (_playerData == null)
            Debug.LogError($"PlayerData is null");           
    }

}
