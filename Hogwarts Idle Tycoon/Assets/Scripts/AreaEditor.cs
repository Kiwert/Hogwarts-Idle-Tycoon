using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEditor : MonoBehaviour
{
    [SerializeField] List<GameObject> ObjectToActivate = new List<GameObject>();
    [SerializeField] List<GameObject> ObjectToDeactivate = new List<GameObject>();
    [SerializeField] Animator studentSlots;
    //[SerializeField] GameObject unlockObjects;
    //[SerializeField] GameObject mainAreaObjects;
    [SerializeField] AreaInteraction interaction;
    [SerializeField] float delay;
    Area area;
    public bool looked;
    AreaData areaData;
    
    void Start()
    {
        area = GetComponent<Area>();
        looked = area.locked;
       areaData = area.areaData;
       

    }
    
   
    public void UnlockArea()
    {
        StartCoroutine(UnlockAreaCourotine());
    }
   
    IEnumerator UnlockAreaCourotine( )
    {

        //unlockObjects.SetActive(false);
        //mainAreaObjects.SetActive(true);
        GameManager.Instance.UpdateCoins(-areaData.unlockCost);
        GameManager.Instance.UpdateExp(areaData.schoolExperienceOnUnlock);
        UIManager.Instance.HideAreaUnlockPanel();
        UIManager.Instance.HideAreaEditorPanel();
        foreach (GameObject obj in ObjectToDeactivate)
        {
            obj.SetActive(false);
           
            
        }
        foreach (GameObject obj in ObjectToActivate)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(delay);
            studentSlots.SetInteger("TableCount", 1);
        }
        
        area.UnlockArea();
        area.locked = false;
        looked = false;
        //CameraController.Instance.isFocusing = true; 


        //interaction.SelectArea();
        CameraController.Instance.Zoomin();

    }
    
    
    public void UpgradeCapacity()
    {
        area.UpgradeCapacity();
        GameManager.Instance.UpdateCoins(-areaData.upgradeCost);
        //SelectArea();
        studentSlots.SetInteger("TableCount", areaData.capacity);
    }
    
}
