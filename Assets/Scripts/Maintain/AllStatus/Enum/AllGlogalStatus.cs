using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllGlogalStatus
{
    None,
    Loading,
    BackGround,
    LoginSignup,
    Game,
    Popup
}

public enum StatusLoading
{
    None,
    Active,
    Deactive
}

public enum StatusBackground
{
    None,
    Active,
    Deactive
}
public enum StatusAuthentication
{
    LogIn,
    SignUp,
    CreateNewPassword,
    EnterCode,
    TwoFA,
    RestorePassword,
    RestorePasswordCode,
    Back = 99,
    TermsOfService
}

public enum StatusOffice
{
    Roster,
    Statistics,
    Balance,
    Customize,
    Back = 99,
    None
}

public enum StatusOfficePlayer
{
    Traits,
    History,
    Statistics,
    Back = 99,
    None,
    Offers
}

public enum StatusOfficeCustomize
{
    TeamMenu,
    TeamEmblem,
    TeamForm,
    TeamName,
    Back = 99
}

public enum StatusOfficeCustomizeForm
{
    GoalKeeper,
    Home,
    Guest,
    Back = 99,
    None
}

public enum StatusGame
{
    None,
}
public enum StatusPopup
{
    None,
    Profile,
    Setting,
    InfoBuilding,
    Back = 99
}

public enum StatusModalWindows
{
    None,
    NFT,
    Wallet,
    ProfilePicture,
    ChangeEmail,
    ChangePassword,
    Apply2FA,
    ChangeName,
    ChangeLanguage,
    ChangeEmailCode
}

public enum StatusCreateTeam
{
    None,
    CreateTeam,
    CustomizeUniform,
    Tutorial,
    CreateTeamLoading,
    StartingPlayers,
    Back = 99
}

public enum StatusBuilding
{
    None,
    MedicalCenterMain,
    MedicalCenterTeam,
    Office,
    OfficePlayer,
    FitnessCenterMain,
    FitnessCenterTeam,
    TrainingCenterMain,
    TrainingCenterTeam,
    Back = 99,
    CommunityCenter,
    CommunityCenterUnion,
    CommunityCenterFriend,
    CommunityCenterUnionCreate,
    QuickLoading, 
    MarketMain,
    MarketPlayer,
    TrainingCenterSkills,
    StadiumMain,
    StadiumTournaments,
    StadiumMatchHistory,
    StadiumMatchFuture,
    SportSchool
}

public enum StatusQuickLoading
{
    None, QuickLoading
}

public enum StatusCommunityCenter
{
    Leaders, Unions, Friends, FriendlyMatches, Back
}

public enum StatusCommunityCenterPopup
{
    None, UnionsFilter, FreindsFind, UnionBuilding, UnionInvite, 
    MatchPlanner, CreateUnion, UnionFriend, Back
}

public enum StatusCommunityCenterFriend
{
    Roster, Statistics, Back
}

public enum StatusCommunityCenterUnion
{
    Members, Buildings, Requests, Settings, Back
}

public enum StatusCreateTeamOptions
{
    GenerationOne, GenerationTwo, GenerationThree, Back
}

public enum StatusMarket
{
    Shop, Inventory, Transfers, MyTransfers, Back
}

public enum StatusMarketPopup
{
    ItemDisplay, NewPlayer, Filter, DeadlinePlanner, RosterMarket, Back, None, OfferPrice, ChangePrice, EditPlayerTransfer
}

public enum StatusMarketPlayer
{
    TraitsMarket, HistoryMarket, StatisticsMarket, OffersMarket, Back
}

public enum StatusStadium
{
    UpcomingMatches, Standings, BuildingFeatures, MatchSettings, HistoryMatches, Back
} 

public enum StatusStadiumTournament
{
    NationalUpcomingMatches, NationalGroupStandings, NationalCountries, GamesTree, GamesTreeDetail, FriendlyMatches
}

public enum StatusStadiumMatchOld
{
    Replay, Statistics, Log, TeamLineup, TeamReplaces
}
