using LazySoccer.SceneLoading.Infrastructure.Customize;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.OfficeCustomize
{
    public class OfficeCustomizeUniformPatternButton : CustomizeTeamUniformButton
    {
        private void Awake()
        {
            ActiveColor = "974EF9";
            NeutralColor = "9A9A9A";
        }
    }
}