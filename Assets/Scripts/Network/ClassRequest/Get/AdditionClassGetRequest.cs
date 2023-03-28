using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network.Get
{
    public class AdditionClassGetRequest
    {
        public class TeamPlayerStatistics
        {
            public int playerId;
            [CanBeNull] public string name;
            [CanBeNull] public string lastName;
            public int age;
            public int dailySalary;
            public TeamPlayerStatus status;
            public DateTime? dateOfCompletion;
            public double injuryChance;
            public int matchInRow;
            public int power;
            public Team team;
            public Position position;
            public Statistics statistics;
            public List<Trait> playerTraits;
            public List<Injury> playerTraumas;
        }
        
        public class Statistics
        {
            public int playerGlobalStatId;
            public int goalFor;
            public int matchPlayed;
            public int minutesOnField;
            public double averageScore;
            //public List<PlayerReward> rewards;
        }

        public class TeamPlayerReward
        {
            public string rewardSTR;
            public PlayerRewardFor rewardType;
            public int rewardLVL;
            public PlayerRewardType reward;
            public string description;
        }
        
        public class TeamPlayerTrophy
        {
            public PlayerTrophyFor trophyFor;
            public TrophyLVL trophy;
            public string trophySTR;
            public Season season;
            public string description;
        }
        
        public enum PlayerTrophyFor
        {
            Boot,
            Glove,
            Ball
        }
        public enum PlayerRewardFor
        {
            InLine,
            Veteran,
            OldTimer,
            Experienced,
            Professional,
            Expert,
            Master,
            Unlucky,
            Bully,
            GoodPlaye,
            ExcellentPlayer,
            FlawlessPlayer,
            Champion,
            Legend
        }
        
        public enum PlayerRewardType
        {
            PlayedNonFriendlyMatches,
            ReachedRawPower,
            GotTraumas,
            GotYellowCards,
            AveragePerformanceRating,
            RewardedTrophies
        }
        
        public class Match
        {
            public string gameId;
            public DateTime gameDate;
            public double ticketPrice;
            public double attendance;
            public GameStatus status;
            public Division division;
            public Tournament category;
            public MatchStep match;
            public Season season;
            public Team hostTeam;
            public int hostGoalFor;
            public Team guestTeam;
            public int guestGoalFor;
            public GameStatus guestGameStatus;
            public GameStatus hostGameStatus;
        } 
        
        public class MatchStatistics
        {
            public string gameId;
            public DateTime gameDate;
            public double ticketPrice; 
            public GameStatus status;
            public Division division;
            public Tournament category;
            public MatchStep match;
            public Season season;
            public Team hostTeam; 
            public Team guestTeam;
            [CanBeNull] public List<TeamGameStats> teamGameStats;
            [CanBeNull] public List<PlayerGameStats> playerGameStats;
        } 
        
        public class TeamGameStats
        {
            public string teamGameStatId { get; set; }
            public int goalFor { get; set; }
            public int goalAgainst { get; set; }
            public int goalDifference { get; set; }
            public int yellowCards { get; set; }
            public int redCards { get; set; }
            public int power { get; set; }
            public int possessionOnBallPercent { get; set; }
            public int strikesQuantity { get; set; }
            public int successfulPassesQuantity { get; set; }
            public int unsuccessfulPassesQuantity { get; set; }
            public int fallsQuantity { get; set; }
            public int quantityOfCornerStrikes { get; set; }
            public int offsidesQuantity { get; set; }
            public GameStyle style { get; set; }
            public GameTactic tactic { get; set; }
            public bool isLose { get; set; }
            public int teamId { get; set; }
            [CanBeNull] public string team { get; set; }
        }
        
        public enum GameStyle
        {
            SuperDefensive, 
            Defensive, 
            Neutral,
            Attacking,
            SuperAttacking
        }
        public enum GameTactic
        {
            Bus,
            Bulk,
            TikiTaka,
            LongBall,
            Flank,
            Brazilian
        }

        public class PlayerGameStats
        {
            public int id { get; set; }
            public int teamId { get; set; }
            public int? goalScored { get; set; }
            public int? power { get; set; }
            public int? form { get; set; }
            [CanBeNull] public string playerName { get; set; }
            public int possessionOnBallPercent { get; set; }
            public int strikesQuantity { get; set; }
            public int successfulPassesQuantity { get; set; }
            public int unsuccessfulPassesQuantity { get; set; }
            public int fallsQuantity { get; set; }
            public int offsidesQuantity { get; set; }
            public int yellowCard { get; set; }
            public bool isRedCard { get; set; }
            public bool isPlayerForBall { get; set; }
            public int iterationCount { get; set; }
            public PositionIndex positionIndex { get; set; }
            public Pressing pressing { get; set; }
            public ImpactRangeAllow shotsAllow { get; set; }
            public DefensiveLineHeight defensiveLineHeight { get; set; }
            public Mark mark { get; set; }
            public int? markPlayerId { get; set; }
            [CanBeNull] public string substituteId { get; set; }
            public Position playerPosition { get; set; }
        }
        
        public enum PositionIndex
        {
            Left = 0,
            Right = 2,
            Center = 1,
            Substitute = -1,
            Relegated = -2
        }
        
        public enum Mark
        {
            None,
            Personal,
            Zonal
        }
        public enum DefensiveLineHeight
        {
            High,
            Medium,
            Low
        }
        public enum ImpactRangeAllow
        {
            LongShots,
            MediumShots,
            CloseShots
        }
        public enum Pressing
        {
            Intense,
            Defensive,
            None
        }
        
        public enum StepStatus
        {
            None,
            Goal,
            ThrowIn ,
            Corner,
            GoalKick,
            Penalty ,
            Freekick
        }
        
        public enum GameStatus
        {
            Finished,
            Interrupted,
            Pending,
            WaitingForLineup
        }
        
        public enum Tournament
        {
            National_League,
            National_Cup,
            Leaders_Cup,
            Champions_Cup,
            Amateur_conference,
            Friendly,
            All
        }
        
        public enum MatchStep
        {
            Any_third_or_lower_division,
            All_2nd_division,
            All_1st_division,
            Semifinal,
            Final,
            Group,
            one_64,
            one_32,
            one_16,
            one_8,
            one_4,
            None
        }
        
        public class TeamPlayerTransfer
        {
            public string playerTransferId;
            public Team seller;
            public Team buyer;
            public DateTime? startDate;
            public DateTime? deadLineDate;
            public int status;
            public int price;
        }

        public class TeamPlayerSkill
        {
            public int playerSkillId;
            public SkillName name;
            public int level;
            public SkillStatus status;
            public DateTime? dateOfCompletionUpgrade;
        }

        public enum SkillStatus
        { 
            NotImproving,
            TrainingImproving,
            MatchImproving
        }
        
        public enum SkillName
        {
            Defense,
            Pass,
            Speed,
            Technique,
            HeadPlay,
            Strike,
            Clearance,
            Intuition,
            Reflex,
            Saves,
            CrossesAndHighBalls,
            Positioning
        }
    }
}