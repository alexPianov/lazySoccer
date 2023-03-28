using UnityEngine;

namespace LazySoccer.Status
{
    public class BuildingWindowStatus : BaseStatus<StatusBuilding>
    {
        [SerializeField] private StatusBuilding back;
        private StatusBuilding currentBuilding;
        public override StatusBuilding StatusAction
        {
            set
            {
                if (value == StatusBuilding.Back)
                {
                    status = back;
                }
                else
                {
                    status = value;
                }
                
                if (status != StatusBuilding.QuickLoading)
                {
                    currentBuilding = status;
                }
                
                OnStateStatusAction?.Invoke(status);
            }
        }

        public void OpenQuickLoading(bool state)
        {
            if (state)
            {
                if (status != StatusBuilding.QuickLoading)
                {
                    currentBuilding = status;
                }
                
                SetAction(StatusBuilding.QuickLoading);
            }
            else
            {
                SetAction(currentBuilding);
            }
        }
    }
}