
public enum TypeHouse
{
    Stadium = 0,
    Office = 1,
    Communications_Center = 2,
    Training_Center = 3,
    Medical_Center = 4,
    Fitness_Center = 5,
    Sport_School = 6,
    Market = 7,
    Union_Office = 8,
    Union_Strategy_Center = 9,
    Union_Staff_Center = 10,
    None = 11
}

public static class TypeHouseGetter
{
    public static string GetHouseName(TypeHouse house)
    {
        switch (house)
        {
            case TypeHouse.Stadium:
                return "Stadium";
            case TypeHouse.Office:
                return "Office";
            case TypeHouse.Communications_Center:
                return "Communications Center";
            case TypeHouse.Training_Center:
                return "Training Center";
            case TypeHouse.Medical_Center:
                return "Medical Center";
            case TypeHouse.Fitness_Center:
                return "Fitness Center";
            case TypeHouse.Sport_School:
                return "Sport School";
            case TypeHouse.Market:
                return "Market";
            case TypeHouse.Union_Office:
                return "Union Office";
            case TypeHouse.Union_Strategy_Center:
                return "Union Strategy Center";
            case TypeHouse.Union_Staff_Center:
                return "Union Staff Center";
            case TypeHouse.None:
                return "None";
            default:
                return "None";
        }
    }
}