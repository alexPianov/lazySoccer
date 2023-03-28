namespace LazySoccer.Network
{
    public enum TypesRequests
    {
        Auth,
        Change,
    }

    public enum AuthOfRequests
    {
        None,
        Login,
        MailConfirm,
        SendMailCode,
        RestorePassword,
        EnableTwoFactorAuth,
        TwoFactorAuth,
        RefreshToken,
        SignUp,
        GenerateTwoFactorAuth
    }
    public enum ChangeOfRequests
    {
        None,
        ChangePassword,
        ChangeNickName,
        ChangeMail,
        SendCodeToNewMail,
    }
    
    public enum GetUserOfRequests 
    {
        None,
        GetUser
    }

    public enum BuildingRequests
    {
        None,
        AllBuilding,
        UpgradeBulding,
        ImmediateUpgrade,
        Downgrade
    }
    
    public enum CreateTeamRequests
    {
        None,
        CreateTeam,
        ChangeUniforms,
        GeneratePlayers,
        SavePlayersGeneration
    }
    
    public enum TeamRequests
    {
        None,
        Players,
        Player,
        ChangeUniforms,
        Rewards,
        Trophies,
        PlayerRewards,
        PlayerTrophies,
        PlayerTransfers,
        PlayerGameStats,
        PlayerSkills,
        Season
    }
    
    public enum OfficeRequests
    {
        None,
        PastDay,
        Season,
        Team,
        Month,
        ChangeTeamName,
        ChangeEmblem
    }

    public enum StadiumRequests
    {
        Positions,
        UserMatchesHistory,
        UserMatchesUpcoming, 
        GameTierAll,
        NationalCountries, 
        DivisionTornament,
        NationalLeague,
        NationalLeagueStatistic,
        NationalCup,
        NationalCupStatistics,
        Cup,
        CupStatistics,
        Match,
        MatchTime,
        SetMatchLineup
    }
    
    public enum MedicalCenterRequests
    {
        None,
        HealingPlayers,
        CancelHealingPlayers,
        InstantHealingPlayers,
        ExaminationPlayers,
        CancelExaminationPlayers,
        InstantExaminationPlayers
    }
    
    public enum FitnessCenterRequests
    {
        None,
        RestorePlayerForm,
        CancelRestorePlayerForm,
        InstantRestorePlayerForm
    }

    public enum MarketRequests
    {
        TransfersAll, Transfers, 
        PlayerForTransfer, PlayerForTransferCancel, PlayerForTransferSendPriceOffer,
        InGameShop, Inventory, 
        InGameShopBuyBox, InGameShopOpenBox, InGameShopUseLoot, Offers
    }


    public enum CommandRequests
    {
        None,
        AddTransactionIdTest,
        TopUpAccountTest,
        AddNFTTests,
        AddPlayersTraumasTest,
        FameTest,
        AddPlayersTraitsTest,
        AddPlayersTrophiesRewardsTest,
        AddPlayersLoseFormTest,
        DeleteUserTest,
        AddTeamTrophiesRewardsTest,
        GameGeneration,
        GenerateGamesTest,
        OutputToConsoleGameTest,
        RecallNewGameStepAfter
    }

    public enum CommunityRequests
    {
        RatingWorld, RatingDivisionTest, Users, 
        
        Friends, FriendsRequests, FriendsRequestFriendship, 
        FriendsRequestFriendshipConfirm, FriendsRequestReject, FriendsDelete, 
        
        Unions, UnionProfile, UnionProfileById, UnionRequests, 
        UnionCreate, UnionRequestJoin, UnionRequestAccept, UnionRequestDelete,
        UnionMemberDemote, UnionMemberKick, UnionMemberPromote, 
        UnionUpdate, UnionUpdateBuilding,
        UnionDonationStart, UnionDonationAbort, UnionDonate,
        
        FriendMatchRequest, UnionEmblem,
        UnionRequestInviteUsers, UnionRequestsUser, FriendMatchConfirm
    }

    public enum TrainingCenterRequests
    {
        TrainingPlayers, InstantTrainingPlayers, CancelTrainingPlayer
    }
    
}