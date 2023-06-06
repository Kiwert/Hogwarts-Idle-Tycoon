using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArea", menuName = "Area")]
public class AreaData : ScriptableObject
{
    public float lessonDuration;
    public string lessonDurationText;
    public int capacity;
    public float unlockCost;
    public int occupied;
    public string areaName;
    public float studentExpPerLesson;
    public string ExperienceText;
    public float schoolExperienceOnUnlock;
    public float subscriptionFee;
    public float upgradeCost;
    public int preriority;
    
    

}
