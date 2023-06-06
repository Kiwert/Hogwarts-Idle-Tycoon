using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] int schoolLevel = 1;
    [SerializeField] float currentExp;
    [SerializeField] float expReqToLvlUp = 100;
    [SerializeField] float currentCoins;
    [SerializeField] float currentGold;



    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);

    }
    void Start()
    {
        UIManager.Instance.UpdateSchoolLevel(schoolLevel);
        UIManager.Instance.UpdateSchoolExperiece(currentExp, expReqToLvlUp);
        UIManager.Instance.UpdateCoins(currentCoins);
    }
    void CheckLevel()
    {
        if (currentExp >= expReqToLvlUp)
        {
            float rest = currentExp - expReqToLvlUp;
            schoolLevel++;
            UIManager.Instance.UpdateSchoolLevel(schoolLevel);
            expReqToLvlUp += expReqToLvlUp / 2;
            currentExp = rest;
            UpdateExp(currentExp);
        }
    }
    public void UpdateExp(float amount)
    {
        currentExp += amount;
        UIManager.Instance.UpdateSchoolExperiece(currentExp, expReqToLvlUp);
        
        CheckLevel();
    }
    public void UpdateCoins(float amount)
    {
        currentCoins += amount;
        UIManager.Instance.UpdateCoins(currentCoins);
        
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
