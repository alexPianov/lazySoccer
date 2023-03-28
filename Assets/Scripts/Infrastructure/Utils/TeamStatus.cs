using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace Utils
{
    public static class TeamStatus
    {
        public enum Region
        {
            Forestria, Jungland, Savannia, Newlandia
        }

        public static Region GetRegion(int number)
        {
            switch (number)
            {
                case 0: return Region.Forestria;
                case 1: return Region.Jungland;
                case 2: return Region.Savannia;
                case 3: return Region.Newlandia;
                default: return Region.Forestria;
            }
        }
    }
}