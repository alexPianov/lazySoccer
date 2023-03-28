using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.Table
{
    public class SlotPlayerFactorySort : MonoBehaviour
    {
        [Header("Refs")] 
        public SlotPlayerFactory PlayerFactory;
        
        [Header("Buttons")]
        public Button buttonNum;
        public Button buttonPos;
        public Button buttonName;
        public Button buttonAge;
        public Button buttonPower;
        public Button buttonForm;
        
        private enum SortType
        {
            None, Number, Position, Name, Age, Power, Form
        }

        private SortType lastSortType;

        private void Start()
        {
            buttonNum.onClick.AddListener(SortNumber);
            buttonPos.onClick.AddListener(SortPosition);
            buttonName.onClick.AddListener(SortName);
            buttonAge.onClick.AddListener(SortAge);
            buttonPower.onClick.AddListener(SortPower);
            buttonForm.onClick.AddListener(SortForm);
        }

        private void SortTransformList(List<SlotPlayer> orderedSlots)
        {
            for (int i = 0; i < orderedSlots.Count; i++)
            {
                orderedSlots[i].transform.SetSiblingIndex(i);
            }
        }

        private void SortNumber()
        {
            List<SlotPlayer> orderedSlots = new();
            
            if (lastSortType == SortType.Number)
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderBy(player => player.numberInList).ToList();
                
                lastSortType = SortType.None;
            }
            else
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderByDescending(player => player.numberInList).ToList();
                
                lastSortType = SortType.Number;
            }

            SortTransformList(orderedSlots);
        }
        
        private void SortPosition()
        {
            List<SlotPlayer> orderedSlots = new();
            
            if (lastSortType == SortType.Position)
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderBy(player => player.GetTeamPlayer().position.position).ToList();
                    
                lastSortType = SortType.None;
            }
            else
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderByDescending(player => player.GetTeamPlayer().position.position).ToList();
                
                lastSortType = SortType.Position;
            }

            SortTransformList(orderedSlots);
        }
        
        private void SortName()
        {
            List<SlotPlayer> orderedSlots = new();
            
            if (lastSortType == SortType.Name)
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderBy(player => player.GetTeamPlayer().name).ToList();
                
                lastSortType = SortType.None;
            }
            else
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderByDescending(player => player.GetTeamPlayer().name).ToList();
                
                lastSortType = SortType.Name;
            }

            SortTransformList(orderedSlots);
        }
        
        private void SortAge()
        {
            List<SlotPlayer> orderedSlots = new();
            
            if (lastSortType == SortType.Age)
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderBy(player => player.GetTeamPlayer().age).ToList();
                    
                lastSortType = SortType.None;
            }
            else
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderByDescending(player => player.GetTeamPlayer().age).ToList();
                
                lastSortType = SortType.Age;
            }

            SortTransformList(orderedSlots);
        }
        private void SortPower()
        {
            List<SlotPlayer> orderedSlots = new();
            
            if (lastSortType == SortType.Power)
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderBy(player => player.GetTeamPlayer().power).ToList();
                    
                lastSortType = SortType.None;
            }
            else
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderByDescending(player => player.GetTeamPlayer().power).ToList();
                
                lastSortType = SortType.Power;
            }

            SortTransformList(orderedSlots);
        }
        private void SortForm()
        {
            List<SlotPlayer> orderedSlots = new();
            
            if (lastSortType == SortType.Form)
            {
                orderedSlots = PlayerFactory
                                    .GetTable().OrderBy(player => player.GetTeamPlayer().form).ToList();
                                    
                lastSortType = SortType.None;
            }
            else
            {
                orderedSlots = PlayerFactory
                    .GetTable().OrderByDescending(player => player.GetTeamPlayer().form).ToList();
                
                lastSortType = SortType.Form;
            }

            SortTransformList(orderedSlots);
        }
    }
}