using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.Network.Get
{
    public class GeneralClassGETRequest
    {
        #region Request User

        public class Role
        {
            public string roleName;
        }

        public class UserData
        {
            public string id;
            [CanBeNull] public string email;
            [CanBeNull] public string userName;
            [CanBeNull] public string country;
            [CanBeNull] public string teamName;
            public double balance;
            public int matchCount;
            public int winsCount;
            public bool firstLogin;
            public bool twoFactorEnabled;
            public bool lockoutEnabled;
            public bool emailConfirmed;
            public bool nftEnabled;
            public bool isFreeNameChange;
            public int nftCount;
            public List<Role> roles;

            public override string ToString()
            {
                return $"{email}  {userName}";
            }
        }

        public class User
        {
            public string userId { get; set; }
            public string userName { get; set; }
            public Team team { get; set; }
        }

        public class FriendshipRequest
        {
            public string requestId;
            public User recipient;
            public bool isConfirmRecipient;
            public User sender;
            public FriendshipType type;
            public DateTime date;
            public int? bet;
        }

        public class UserUnionRequest
        {
            public int requestId;
            [CanBeNull] public User user;
            public bool isConfirmUser;
            public UnionHeader union;
            public bool isConfirmUnion;
        }

        public enum FriendshipType
        {
            Game = 0,
            Friends = 1
        }


        public class FinancialStatistics
        {
            public DateTime createDate;
            public double amount;
            public string amountOf;
            public int season;
        }

        #endregion

        #region Request Team

        public class Trait
        {
            public int traitId;
            public TraitName name;
            public DropChanceType rarity;
        }

        public enum TraitName
        {
            Ineducable,
            Dumb,
            Smart,
            Prodigy,
            Fit,
            Athletic,
            Unfit,
            Weak,
            Idol,
            Star, 
            Famous,
            Careless,
            Fragile,
            Careful,
            Tough,
            Sprinter,
            GoodSprinter,
            BadRunner,
            AwfulRunner,
            AdditionalPosition_GK,
            AdditionalPosition_DC,
            AdditionalPosition_DL,
            AdditionalPosition_DR,
            AdditionalPosition_LR,
            AdditionalPosition_LL,
            AdditionalPosition_MC,
            AdditionalPosition_ML,
            AdditionalPosition_MR,
            AdditionalPosition_MD,
            AdditionalPosition_MA,
            AdditionalPosition_FC,
            AdditionalPosition_WL,
            AdditionalPosition_WR,
            AdditionalPreferredStyle_SuperDefensive,
            AdditionalPreferredStyle_Defensive,
            AdditionalPreferredStyle_Neutral,
            AdditionalPreferredStyle_Attacking,
            AdditionalPreferredStyle_SuperAttacking,
            WeatherResistant,
            WeatherDependant,
            Individualist,
            Conflict,
            Helpful,
            MutualAassistance,
            Demanding,
            Greedy,
            Modest,
            Ascetic,
            Leader,
            ReachedTheLimit,
            PositionProficiency,
            StyleProficiency
        }
        public class Position
        {
            public int playerPositionId;
            [CanBeNull] public string position { get; set; }
            [CanBeNull] public string fieldLocation { get; set; }
        }

        public class Injury
        {
            public int playerTraumaId;
            public string trauma;
            public DateTime? startDate;
            public int healPeriod;
        }

        public class TeamPlayer
        {
            public int playerId;
            public TeamPlayerStatus status;
            public int form;
            public string name;
            public string team;
            public int age;
            public Position position;
            public int dailySalary;
            public int power;
            public int goalCount;
            public int favoriteGameStyle;
            public DateTime? dateOfCompletion;
            public List<Trait> traits;
            public List<Injury> injuries;
        }
        
        public class TeamPlayerId
        {
            public int playerId;
            public TeamPlayerStatus status;
            public int form;
            public string name;
            public string team;
            public int age;
            public Position position;
            public int dailySalary;
            public int power;
            public int goalCount;
            public int favoriteGameStyle;
            public DateTime? dateOfCompletion;
            public List<Trait> traits;
            public List<Injury> playerTraumas;
        }

        public enum TeamPlayerStatus
        {
            SentOnMatch,
            Healing,
            NotHealing,
            Exhausted,
            RestoringForm,
            Training,
            OnExamination,
            OnTransferMarket,
            Charged,
            Healthy,
            OnTrainingAndMatch,
            GenerationOptionOne = 11,
            GenerationOptionTwo = 12,
            GenerationOptionThree = 13,
            None
        }

        #endregion

        #region Team Statistic

        public class TeamStatistic
        {
            public int teamId;
            public Division division;
            public int divisionPlace;
            public int worldPlace;
            public int totalPower;
            public Emblem emblem;
            public UnionHeader union;
            public double avgLineUpPower;
            public double winRate;
            public GlobalStat globalStat;
            public List<Uniform> uniforms;
        }

        public class GlobalStat
        {
            public int teamGlobalStatId;
            public int wins;
            public int loses;
            public int draws;
            public int matchPlayed;
            public int goalFor;
            public int goalAgainst;
            public int goalDifference;
        }

        public class Uniform
        {
            public int uniformsId;
            public UniformType type;
            public string uniformsColor;
            public string emblemColor;
        }

        public enum UniformType : int
        {
            GK = 0,
            Home = 1,
            Guest = 2
        }

        #endregion

        #region Building

        public class BuildingAll
        {
            public int id;
            public DateTime? dateOfCompletion;
            public DateTime? dateOfStart;
            public TypeHouse buildingType;
            public int level;
            [CanBeNull] public string description;
            public int buildingLvLId;
            public int maintenanceCost;
            [CanBeNull] public int? buildingCost;
            [CanBeNull] public int? costInstantBuilding;
            [CanBeNull] public double? buildTime;
            [CanBeNull] public List<Influence> influence;
        }
        
        public class Influence
        {
            public int buildingInfluenceId;
            public InfluenceFor influence;
            public double value;
        }

        public class UpgradeBuilding
        {
            public int teamBuildingId;
        }

        public enum InfluenceFor
        {
            MarketCommission,
            MarketMaxTradeOfferssimultaneouslyCount,
            MarketMaxTradeOffersCount,
            SportSchoolMinPlayersPower,
            SportSchoolMaxPlayersPower,
            SportSchoolMaxNewPlayersWeekCount,
            SportSchoolLowerChanceGettingNegativeTraits,
            FitnessCenterMaxChargeEffectiveness,
            FitnessCenterMaxChargeSimultaneousPlayersCount,
            MedicalCenterMaxTraumaChanceCount,
            MedicalCenterMaxHealedPlayersCount,
            TrainingCenterMaxTrainedPlayersCount,
            TrainingCenterMinPowerGainCount,
            TrainingCenterMaxPowerGainCount,
            CommunicationsCenterMaxFriendlyMatchesPerSeasonCount,
            CommunicationsCenterIsPossibleJoinUnion,
            CommunicationsCenterIsPossibleTradeChat,
            CommunicationsCenterIsPossibleCreateUnion,
            CommunicationsCenterIsPossibleSetUpCommercialTournament,
            StadiumMaxTicketPrice,
            StadiumMaxVisitorsCount,
            StadiumAdvertisementCampaignEfficiency,
            OfficeMaxRosterCount,
            UnionOfficeMaxMembersCount,
            Union_Staff_CenterMaxIncreaseNFTExpGainFactor,
            Union_Strategy_CenterMaxIncreasePlayerPower
        }

        #endregion

        #region Team

        public class Team
        {
            public int teamId;
            [CanBeNull] public string name;
            [CanBeNull] public Emblem teamEmblem;
            [CanBeNull] public Division division;
        }

        public class Emblem
        {
            public int emblemId;
            [CanBeNull] public string type;
            [CanBeNull] public string[] colors;
        }
        
        public class DivisionStadium
        {
            public int divisionId;
            public int rank;
            public int? baseDivisionId;
        }

        public class Division
        {
            public int divisionId;
            public int rank;
            public Country country;
        }

        public class Country
        {
            public int countryId;
            [CanBeNull] public string name;
        }

        public class TeamReward
        {
            public string rewardSTR;
            public string title;
            public TeamRewardFor reward;
            public int rewardLVL;
        }

        public class TeamTrophy
        {
            public Season season { get; set; }
            public TeamTrophyFor trophy { get; set; }
            public TrophyLVL trophyLVL { get; set; }
            public string trophySTR { get; set; }
            
            public string country { get; set; }
        }

        public enum TrophyLVL
        {
            Platinum,
            Golden,
            Silver,
            Bronze
        }
        
        public enum TeamTrophyFor
        {
            NationalLeague,
            NationalCup,
            LeadersCup,
            ChampionsCup
        }
        
        public enum TeamRewardFor
        {
            Winner,
            SoldOut,
            Trader,
            Comrade
        }
        
        public class Season
        {
            public int seasonId;
            public int serialNumber;
            public DateTime? startDate;
            public DateTime? finishDate;
        }

        #endregion

        #region Communication Center

        public class WorldRatingUser
        {
            public int id { get; set; }
            public int teamId { get; set; }
            [CanBeNull] public string teamName { get; set; }
            public int power { get; set; }
            public int performance { get; set; }
            public int balance { get; set; }
            public Emblem emblem { get; set; }
        }

        public class DivisionTournament
        {
            public int pts { get; set; }
            public int wins { get; set; }
            public int loses { get; set; }
            public int draws { get; set; }
            public int matchPlayed { get; set; }
            public int goalFor { get; set; }
            public int goalAgainst { get; set; }
            public int goalDifference { get; set; }
            public int yellowCards { get; set; }
            public int redCards { get; set; }
            public Tournament tournament { get; set; }
            public Team team { get; set; }
            public List<TeamGameStats> teamlastGamesStat { get; set; }
        }

        #endregion

        #region Community Unions

        public class UnionHeader
        {
            public int unionId { get; set; }
            [CanBeNull] public string name { get; set; }
            public Emblem emblem { get; set; }
        }

        public class Union
        {
            public int unionId { get; set; }
            public int place { get; set; }
            [CanBeNull] public string name { get; set; }
            public int rating { get; set; }
            public int currentMembersCount { get; set; }
            public int maxMembersCount { get; set; }
            public RecruitingPolicy policy { get; set; }
            public Emblem emblem { get; set; }
        }

        public class UnionProfile
        {
            public int unionId { get; set; }
            public List<UnionTeam> unionTeams { get; set; }
            public List<UnionBuilding> unionBuildings { get; set; }
            public RecruitingPolicy policy { get; set; }
            public string name { get; set; }
            public int rating { get; set; }
            public int place { get; set; }
            public int maxMembersCount { get; set; }
        }

        public class UnionTeam
        {
            public int teamUnionId { get; set; }
            public User user { get; set; }
            public MemberType type { get; set; }
            public int contribution { get; set; }
        }

        public enum MemberType
        {
            Master,
            Officer,
            Simple
        }

        public class UnionBuilding
        {
            public int unionBuildingId { get; set; }
            public int status { get; set; }
            public DateTime? dateOfCompletion { get; set; }
            public BuildingLvL buildingLvL { get; set; }
            public bool isDonationEnabled { get; set; }
            public int donations { get; set; }
        }

        public class BuildingLvL
        {
            public int buildingLvLId { get; set; }
            public int level { get; set; }
            public int buildingCost { get; set; }
            public int costInstantBuilding { get; set; }
            public int maintenanceCost { get; set; }
            [CanBeNull] public string description { get; set; }
            public int buildTimeInHours { get; set; }
            public TypeHouse building { get; set; }
            [CanBeNull] public BuildingLevelInfo previousLvL { get; set; }
            [CanBeNull] public BuildingLevelInfo nextLvL { get; set; }
        }

        public class BuildingLevelInfo
        {
            public int level { get; set; }
            public int buildingCost { get; set; }
            public int costInstantBuilding { get; set; }
            public int maintenanceCost { get; set; }
            [CanBeNull] public string description { get; set; }
            public int buildTimeInHours { get; set; }
        }

        public class UnionUpdateRequest
        {
            public int emblemId;
            public string? type;
            public string[] colors;
            public RecruitingPolicy recruitingPolicy;
        }

        public enum RecruitingPolicy
        {
            Open = 0,
            Close = 1
        }

        #endregion

        #region Market Inventory

        public class MarketInGameShop
        {
            public int tierId;
            [CanBeNull] public string name;
            public int price;
            [CanBeNull] public string description;
        }

        public class MarketInventory
        {
            public int inventoryId;
            public int tierId;
            [CanBeNull] public string tier;
            public int? lootBoxId;
            public LootBox lootBox;
            public InventoryStatus status;
        }

        public class LootBox
        {
            public DropChanceType dropChance;
            public LootName loot;
            public LootType lootType;
            [CanBeNull] public string description;
        }

        public enum InventoryStatus
        {
            NotUnpacked,
            NotActive,
            Active
        }

        public enum DropChanceType
        {
            Common,
            Rare,
            Very_Rare
        }

        public enum LootName
        {
            StaffIncentivesProgramme,
            StaffTrainingProgramme,
            SumOfMoney,
            MiracleMedicine,
            IntensiveTrainingProgramme,
            ScoutInsight
        }

        public enum LootType
        {
            Little,
            Moderate,
            Quite,
            Big
        }

        #endregion

        #region Market Transfer

        public class MarketPlayerTransfer
        {
            public string playerTransferId;
            public MarketTeamPlayer player;
            public Team seller;
            public Team buyer;
            public DateTime? startDate;
            public DateTime? deadLineDate;
            public int startPrice;
            public int currentPrice;
            public int? priceStep;
            public TransferStatus status;
        }

        public enum TransferStatus
        {
            OnPending,
            Finished,
            Canceled
        }

        public class MarketTeamPlayer
        {
            public int playerId;
            [CanBeNull] public string name;
            [CanBeNull] public string team;
            public int age;
            public Position position;
            public int dailySalary;
            public int power;
            public int goalCount;
            public List<Trait> traits;
        }

        public class MarketOffer
        {
            public string purchaseCandidanteId;
            public int price;
            public MarketOfferManager manager;
            public DateTime date;
        }

        public class MarketOfferManager
        {
            public string userId;
            [CanBeNull] public string userName;
            public Team team;
        }
        #endregion

        #region Stadium

        public class GameTier
        {
            public int tierId;
            public Tournament tournament;
            public MatchStep match;
        }
        
        public class NationalCountries
        {
            public int countryId;
            [CanBeNull] public string name;
            public List<DivisionStadium> divisions;
        }

        public class StadiumPositions
        {
            public int playerPositionId;
            [CanBeNull] public string position;
            [CanBeNull] public string fieldLocation;
        }

        #endregion

    }
}
