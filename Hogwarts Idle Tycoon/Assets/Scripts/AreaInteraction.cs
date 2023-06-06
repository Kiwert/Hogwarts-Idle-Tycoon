using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaInteraction : MonoBehaviour
{
    [SerializeField] AreaEditor areaEditor;
    [SerializeField] AreaData areaData;
    float lessonDuration;
    float lessonExperience;
    int capacity;
    float unlockCost;
    string areaName;
    string lessonDurationText;
    string ExperienceText;
    float upgradeCost;
    void Start()
    {
        
    }

    public void SelectArea()
    {
        float zoom = 10;
        
        GetDataFromArea();
        UIManager.Instance.SetSelectedArea(areaEditor);
        if (areaEditor.looked)
        {
            CameraController.Instance.selectZoomAmount = -zoom;
            UIManager.Instance.SetAndShowAreaUnlockPanel(areaName,unlockCost);
            UIManager.Instance.HideAreaEditorPanel();
        }
        else
        {
            CameraController.Instance.selectZoomAmount = zoom;
            UIManager.Instance.SetAndShowAreaEditorPanel(areaName, lessonExperience, lessonDuration,lessonDurationText,ExperienceText,upgradeCost.ToString(), capacity);
            UIManager.Instance.HideAreaUnlockPanel();
        }
            
    }
    
    void GetDataFromArea()
    {
        lessonDuration = areaData.lessonDuration;
        lessonExperience = areaData.studentExpPerLesson;
        capacity = areaData.capacity;
        areaName = areaData.areaName;
        unlockCost = areaData.unlockCost;
        lessonDurationText = areaData.lessonDurationText;
        ExperienceText = areaData.ExperienceText;
        upgradeCost = areaData.upgradeCost;


    }
   
}
