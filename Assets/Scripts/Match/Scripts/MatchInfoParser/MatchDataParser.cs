using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchDataParser : MonoBehaviour
{
    [SerializeField]
    private TextAsset jsonFile;

    private List<TeamStep> teamSteps;

    public List<TeamStep> TeamSteps
    {
        get => teamSteps;
        set => teamSteps = value;
    }

    public void ParseMatchData(string matchData)
    {
        teamSteps = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TeamStep>>(matchData);
    }
    
    public void ClearMatchData()
    {
        teamSteps = null;
    }

    public TeamStep GetStep(int step)
    {
        if (teamSteps == null) return null;
        if (step < teamSteps.Count)
            return teamSteps[step];
        else 
            return null;
    }

    public int GetStepsSize()
    {
        if (teamSteps == null) return 0;
        return teamSteps.Count;
    }
}