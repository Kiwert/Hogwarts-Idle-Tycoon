using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] EditorPanel editorPanel;
    [SerializeField] UnlockPanel unlockPanel;
    [SerializeField] TMP_Text schoolLevelText;
    [SerializeField] TMP_Text currentExpText;
    [SerializeField] TMP_Text currentCoinsText;
    [SerializeField] Slider LevelExpSlider;
    AreaEditor areaEditor;
     
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);

    }

    public void UpdateSchoolLevel(int level)
    {
        
        
        
        schoolLevelText.text = level.ToString();
        
    }
    public void UpdateSchoolExperiece(float experience,float reqExpToLVLUp)
    {
        LevelExpSlider.maxValue = reqExpToLVLUp;
        LevelExpSlider.value = experience;
        currentExpText.text = experience.ToString()+"/"+reqExpToLVLUp.ToString();
    }
    public void UpdateCoins(float amount)
    {
        currentCoinsText.text = amount.ToString();
    }
    public void SetSelectedArea(AreaEditor areaE)
    {
        areaEditor = areaE;
    }
    public void SetAndShowAreaEditorPanel(string name, float experience, float lessonDuration, string lessonDurationText, string experienceText, string UpgradeCostText,int areaCapacity)
    {
        editorPanel.SetEditorPanel(name,experience, lessonDuration, lessonDurationText, experienceText, UpgradeCostText,areaCapacity);
    }
    public void HideAreaEditorPanel()
    {
        editorPanel.ShowHide(false);
    }
    public void SetAndShowAreaUnlockPanel(string areaName, float cost)
    {
        unlockPanel.SetUnlockPanel(areaName, cost);
    }
    public void HideAreaUnlockPanel()
    {
        unlockPanel.ShowHide(false);
    }
   

    public void UnlockArea()
    {
        areaEditor.UnlockArea();
    }
    public void UpgradeAreaCapacity()
    {
        areaEditor.UpgradeCapacity();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
}
