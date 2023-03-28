using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class PlayerPlaceholder : MonoBehaviour
{
    [SerializeField]
    private TMP_Text positionText;
    [SerializeField]
    private TMP_Text nameText;

    private string playerName;
    private int playerId;
    private int positionIndex;
    private int playerPositionId;
    private string roleFullName;

    public string PlayerName
    {
        get => playerName;
    }
    public int PlayerID
    {
        get => playerId;
    }
    public int PositionIndex
    {
        get => positionIndex;
    }
    public int PlayerPositionId
    {
        get => playerPositionId;
    }
    
    public string RoleFullName
    {
        get => roleFullName;
    }

    public void ResetPlaceholder()
    {
        nameText.text = "";
        playerName = "";
        nameText.gameObject.SetActive(false);
        playerId = -1;
    }

    public void SetRoleAndPosition(Vector3 coords, PositionInfo role, Vector2 cellCoords)
    {
        ResetPlaceholder();

        transform.position = coords;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

        positionText.text = role.positionAbbreviation;
        roleFullName = role.positionFullName;

        InterpretPosition(cellCoords);
    }

    public void SetPlayerIdentificationInfo(TeamPlayer player)
    {
        playerId = player.playerId;
        playerName = player.name;
        nameText.text = playerName;

        nameText.gameObject.SetActive(true);
    }

    private void InterpretPosition(Vector2 cellCoords)
    {
        if (roleFullName == "Goalkeeper")
            playerPositionId = 1;
        else if (roleFullName == "Forward")
            playerPositionId = 2;
        else if (roleFullName == "Winger")
        {
            if (cellCoords.x == 2)
                playerPositionId = 3;
            else if (cellCoords.x == 8)
                playerPositionId = 4;
        }
        else if (roleFullName == "Lateral")
        {
            if (cellCoords.x == 2)
                playerPositionId = 5;
            else if (cellCoords.x == 8)
                playerPositionId = 6;
        }
        else if(roleFullName == "Defender")
        {
            if (cellCoords.x < 4)
                playerPositionId = 8;
            else if (cellCoords.x < 7)
                playerPositionId = 7;
            else
                playerPositionId = 9;
        }
        else if(roleFullName == "Midfielder")
        {
            if (cellCoords.y == 4)
                playerPositionId = 13;
            else if (cellCoords.y == 6)
                playerPositionId = 14;
            else if (cellCoords.x == 5)
                playerPositionId = 10;
            else if (cellCoords.x == 2)
                playerPositionId = 11;
            else if (cellCoords.x == 8)
                playerPositionId = 12;
        }

        if (cellCoords.x == 1 || cellCoords.x == 4 || cellCoords.x == 7)
            positionIndex = 0;
        else if (cellCoords.x == 2 || cellCoords.x == 5 || cellCoords.x == 8)
            positionIndex = 1;
        else
            positionIndex = 2;
    }
}


