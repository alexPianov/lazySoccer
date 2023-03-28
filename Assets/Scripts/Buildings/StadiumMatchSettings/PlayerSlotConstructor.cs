using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using UnityEngine.UI;
using System.Linq;

public class PlayerSlotConstructor : MonoBehaviour
{
    public TeamRosterManager teamRosterManager;

    [SerializeField]
    private Button slotButton;

    [SerializeField]
    private TMP_Text playerNumber;
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField]
    private TMP_Text playerAge;

    [SerializeField]
    private TMP_Text playerPower;
    [SerializeField]
    private TMP_Text playerSpecialization;
    [SerializeField]
    private TMP_Text playerEnergy;

    [SerializeField]
    private int playerId;

    public TeamPlayer player;


    public void SetupPlayerSlot(TeamPlayer player, int playerNumber)
    {
        playerId = player.playerId;

        this.playerNumber.text = playerNumber.ToString();

        playerName.text = player.name;
        playerAge.text = "(" + player.age.ToString() + " years)";
        playerPower.text = player.power.ToString();

        playerSpecialization.text = (player.position.fieldLocation != null ? player.position.fieldLocation[0] : "") + "" + player.position.position[0];

        Debug.Log("Player status " + player.status);
        Debug.Log(player.form);

        if (player.status != TeamPlayerStatus.Healthy && player.status != TeamPlayerStatus.Charged && player.status != TeamPlayerStatus.Training)
            slotButton.interactable = false;

        this.player = player;
    }

    public void SelectButton()
    {
        teamRosterManager.SetSelectedPlayerId(playerId);
    }
}
