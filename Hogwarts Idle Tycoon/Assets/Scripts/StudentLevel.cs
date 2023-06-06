using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentLevel : MonoBehaviour
{
    [SerializeField] float schoolGainedExpPerLevelUp; 
    float curretExperience;
    int currentLevel = 1;
    float expReqToLvlUp = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Checklevel()
    {

        if (curretExperience >= expReqToLvlUp)
        {
            float rest = curretExperience - expReqToLvlUp;
            currentLevel++;
            expReqToLvlUp += expReqToLvlUp / 2;
            curretExperience = rest;
            GameManager.Instance.UpdateExp(schoolGainedExpPerLevelUp);
        }

    }
    public void AddExperience(float amount)
    {
        curretExperience += amount;
    }
    // Update is called once per frame
   
}
