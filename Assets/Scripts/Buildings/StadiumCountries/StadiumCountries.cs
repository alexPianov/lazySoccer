using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using LazySoccer.Table;
using LazySoccer.Windows;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumCountries
{
    public class StadiumCountries : CenterSlotList
    {
        [SerializeField] private StadiumDivision.StadiumDivision stadiumDivision;
        
        [Header("Expanding prefabs")]
        [SerializeField] private GameObject prefabCountryExpandButton;
        [SerializeField] private GameObject prefabCountryExpandContainer;

        [Header("Refs")] 
        [SerializeField] private UiUtilsExpandGroup ExpandGroup;
        
        public async UniTask SetTournament(Tournament tournament)
        {
            var nationalCountries = await ServiceLocator
                .GetService<StadiumTypesOfRequests>().GetNationalCountries();

            DestroyAllSlots();
            
            foreach (var countryInfo in nationalCountries)
            {
                if(countryInfo.divisions == null || countryInfo.divisions.Count == 0) continue; 

                var expandButton = CreateSlot(prefabCountryExpandButton)
                    .GetComponent<StadiumCountryExpandButton>();
                
                var expandContainer = CreateSlot(prefabCountryExpandContainer).
                    GetComponent<StadiumCountryExpandContainer>();

                expandButton.expandingObject = expandContainer.gameObject;
                expandButton.SetExpandGroup(ExpandGroup);
                ExpandGroup.AddExpandingObject(expandButton);
                
                expandButton.SetCountry(countryInfo.countryId);

                expandContainer.UtilsExpand.expandingObject = expandButton.gameObject;
                expandContainer.UtilsExpand.SetExpandGroup(ExpandGroup);
                ExpandGroup.AddExpandingObject(expandContainer.UtilsExpand);

                expandContainer.CreateDivisionList(countryInfo, tournament, stadiumDivision);
                
                expandContainer.gameObject.SetActive(false);
            }
        }
    }
}