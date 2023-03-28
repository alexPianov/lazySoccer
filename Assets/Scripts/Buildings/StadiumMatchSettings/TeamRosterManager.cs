using System.Collections;
using System.Collections.Generic;
using LazySoccer.SceneLoading.Buildings.StadiumMatchSettings;
using UnityEngine;
using UnityEngine.UIElements;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class TeamRosterManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerSlot;

    [SerializeField]
    private GameObject teamScroll;

    [SerializeField]
    private List<PlayerSlotConstructor> playerSlots;

    [SerializeField] private StadiumMatchLineupTeamTable _teamTable;

    [SerializeField]
    private int selectedPlayerId = -1;

    public void SetupList(List<TeamPlayer> teamPlayers)
    {
        
        _teamTable.CreateLineupTable(teamPlayers);
        
        return;
        for (int i = 1; i <= teamPlayers.Count; i++)
        {
            var slot = Instantiate(playerSlot, teamScroll.transform);
            var slotScript = slot.GetComponent<PlayerSlotConstructor>();

            slotScript.teamRosterManager = this;

            playerSlots.Add(slotScript);
            slotScript.SetupPlayerSlot(teamPlayers[i-1], i);
        }
    }

    public void SetSelectedPlayerId(int id)
    {
        selectedPlayerId = id;
    }

    public int GetPickedPlayer()
    {
        return selectedPlayerId;
    }

    public void ResetRoster()
    {
        foreach(var playerSlot in playerSlots)
        {
            Destroy(playerSlot.gameObject);
        }

        playerSlots = new List<PlayerSlotConstructor>();
        selectedPlayerId = -1;

        gameObject.SetActive(false);
    }
}
