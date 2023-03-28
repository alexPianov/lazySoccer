using LazySoccer.SceneLoading;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

[CreateAssetMenu(fileName = "HUD", menuName = "Player/HUD", order = 2)]
public class PlayerHUD : ScriptableObject
{
    [SerializeField] public BaseAction<string> Name;
    [SerializeField] public BaseAction<string> NameTeam;
    [SerializeField] public BaseAction<string> NameUnion;
    [SerializeField] public BaseAction<int>  Balance;
    [SerializeField] public BaseAction<int> CountMatch;
    [SerializeField] public BaseAction<int> CountWinMatch;
    [SerializeField] public BaseAction<int> TeamEmblemId;
    [SerializeField] public BaseAction<int> UnionEmblemId;
    [SerializeField] public BaseAction<Sprite> Avatar;
    [SerializeField] public BaseAction<Sprite> TeamEmblem;
    [SerializeField] public BaseAction<Sprite> UnionEmblem;

    [Title("No Base")]
    [SerializeField] public BaseAction<string> NameShortTeam;

    public void AddParam(UserData user)
    {
        Name.Value = user.userName;
        NameTeam.Value = user.teamName;
        Balance.Value = (int)user.balance;
        CountMatch.Value = user.matchCount;
        CountWinMatch.Value = user.winsCount;
    }

    public void SetUnionName(string unionName)
    {
        NameUnion.Value = unionName;
    }
    
    public void SetUnionEmblem(int unionEmblemId)
    {
        UnionEmblemId.Value = unionEmblemId;
        
        UnionEmblem.Value = ServiceLocator.GetService<ManagerSprites>()
            .GetUnionSprite(unionEmblemId);
    }

    public void SetEmblem(int embelmId)
    {
        TeamEmblemId.Value = embelmId;
        
        TeamEmblem.Value = ServiceLocator.GetService<ManagerSprites>()
            .GetTeamSprite(embelmId);
    }
}

