
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPositionData
{
    public Transform positionTransform;
    public bool available;
    public Student currentStudent;

}

public class Area : MonoBehaviour
{
    public static Dictionary<string, Area> Instances = new Dictionary<string, Area>();
    public AreaData areaData;
    public List<Student> waitingStudents = new List<Student>();
    [SerializeField] List<Transform> waitingPositions = new List<Transform>();
    [SerializeField] List<Transform> inPositions = new List<Transform>();
    [SerializeField] List<Student> inStudents = new List<Student>();
    [SerializeField] Transform waitLinePosition;
   

    Dictionary<Transform, bool> waitingPositionsAvailability;

    Dictionary<string, InPositionData> inPositionsAvailability;
    public bool locked;
    
    private void Awake()
    {
        if (!locked)
        {
            Instances.Add(areaData.areaName, this);
        }
       
    }
    public void UnlockArea()
    {
        Instances.Add(areaData.areaName, this);
        SchoolManager.Instance.SetAreas();
    }
    private void Start()
    {
       
        areaData.occupied = 0;
        areaData.capacity = 1;
        SchoolManager.Instance.SetAreas();
        SetInPositionsDictionaty();
        SetWaitingPositionsDictionaty();
    }
    void SetWaitingPositionsDictionaty()
    {
        waitingPositionsAvailability = new Dictionary<Transform, bool>();
       
        foreach (Transform pos in waitingPositions)
        {
            waitingPositionsAvailability.Add(pos, true);
        }
    }
    void SetInPositionsDictionaty()
    {
      
        inPositionsAvailability = new Dictionary<string, InPositionData>();
        foreach (Transform pos in inPositions)
        {
            InPositionData posData = new InPositionData();
            posData.positionTransform = pos;
            posData.available = true;
          //  Debug.Log("name " + i + "/" + posData);
            inPositionsAvailability.Add(inPositions.IndexOf(pos).ToString(), posData);
           

        }


       
       
        
    }
    public float GetInActionDuration()
    {
        return areaData.lessonDuration;
    }
    public float GetSubscriptionFee()
    {
        return areaData.subscriptionFee;
    }
    public float GetStudentExperiencePerLesson()
    {
        return areaData.studentExpPerLesson;
    }
    public bool HasAvailablePlace()
    {
        return (areaData.capacity > areaData.occupied);
    }

     string GetAvailableInPosition()
    {
        foreach (KeyValuePair<string, InPositionData> pos in inPositionsAvailability)
        {
            if (pos.Value.available)
            {
                return pos.Key;
            }
        }

        return null; 
    }
     string GetOccupiedInPosition(Student student)
    {
        foreach (KeyValuePair<string, InPositionData> pos in inPositionsAvailability)
        {
            if (!pos.Value.available && Vector3.Distance(pos.Value.positionTransform.transform.position , student.transform.position) <2)
            {

                return pos.Key ;
            }
        }

      
        return null; 
    }

    string GetCurrentStudentPosition(Student student)
    {
        foreach (KeyValuePair<string, InPositionData> pos in inPositionsAvailability)
        {
            if (pos.Value.currentStudent == student) 
            {

                return pos.Key;
            }
        }


        return null;
    }

    public Transform AddOccupied(Student student)
    {
       areaData.occupied++;
       inStudents.Add(student);
       Transform pos = inPositionsAvailability[GetAvailableInPosition()].positionTransform;
        inPositionsAvailability[GetAvailableInPosition()].currentStudent = student;
        inPositionsAvailability[GetAvailableInPosition()].available = false;
      
        return pos ;
    }
    public void RemoveOccupied(Student student)
    {
        areaData.occupied--; 
        inPositionsAvailability[GetOccupiedInPosition(student)].currentStudent = null;
        inPositionsAvailability[GetOccupiedInPosition(student)].available = true;
        
        inStudents.Remove(student);
        student.ChangeState(2);
        if (waitingStudents.Count != 0)
        {
            for (int i = 0; i < waitingStudents.Count ; i++)
            {
              
                    waitingStudents[i].ChangeState(-1);
               
            }
        }
    }
    public void UpgradeCapacity()
    {
        bool av = HasAvailablePlace();
        
        areaData.capacity++;
        Invoke("UpdateDictionary", 1);

        if (!av&& waitingStudents.Count!=0)
        {
           
            for (int i = 0; i <= waitingStudents.Count - 1; i++)
            {
                if (waitingStudents[i].currentAreaIndex == SchoolManager.Instance.areas.IndexOf(this))
                {
                    waitingStudents[i].ChangeState(-1);

                }
            }
        }
        

    }
    public void UpdateDictionary()
    {
        for (int i = 0; i < inStudents.Count; i++)
        {
            
            inStudents[i].currentDestination = (inPositionsAvailability[GetCurrentStudentPosition(inStudents[i])].positionTransform.position);
            
        }

       

    }
    public Transform AddToWaiting(Student student)
    {
        if (!waitingStudents.Contains(student))
        {
            waitingStudents.Add(student);
        }

        return waitingPositions[waitingStudents.IndexOf(student)];
    }
    private Transform GetAvailableWaitingPosition()
    {
        foreach (KeyValuePair<Transform, bool> pos in waitingPositionsAvailability)
        {
            if (pos.Value)
            {

                return pos.Key;
            }
        }

        return null;
    }
    private Transform GetOccupiedWaitingPosition(Student student)
    {
        foreach (KeyValuePair<Transform, bool> pos in waitingPositionsAvailability)
        {
            if (!pos.Value && Vector3.Distance(pos.Key.transform.position, student.transform.position) < 1)
            {

                return pos.Key;
            }
        }


        return null;
    }
    public Transform GoToWaitLine()
    {
       return waitLinePosition;
    }
    public void RemoveFromWaiting(Student student)
    {
        waitingStudents.Remove(student);
    }
}

