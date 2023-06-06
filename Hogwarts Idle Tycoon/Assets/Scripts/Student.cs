using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public enum StudentState
{
    GoingToArea,
    WaitingInArea,
}

public class Student : MonoBehaviour
{
    
    public StudentState currentState;
    public int currentStateIndex = 0;
    public int currentAreaIndex = 0;
    public List<Area> areas = new List<Area>();
    List<StudentState> studentStates = new List<StudentState>();
    NavMeshAgent navMeshAgent;
    SchoolManager schoolManager;
    Animator animator;
    StudentUI studentUi;
    StudentLevel studentLevel;
    public Vector3 currentDestination;
    void Start()
    {
      
        navMeshAgent = GetComponent<NavMeshAgent>();
        schoolManager = SchoolManager.Instance;
        animator = GetComponent<Animator>();
        studentUi = GetComponent<StudentUI>();
        studentLevel = GetComponent<StudentLevel>();
        if (!schoolManager.Students.Contains(this))
             schoolManager.Students.Add(this);
        CheckAreas();


    }
    void CheckAreas()
   {
        
        areas = schoolManager.areas;
        studentStates.Clear();
        foreach (Area a in areas)
        {
            studentStates.Add(StudentState.GoingToArea);
            studentStates.Add(StudentState.WaitingInArea);
        }
        CheckSates();
   }
    public void CheckSates()
    {
        switch (studentStates[currentStateIndex])
        {
            case StudentState.GoingToArea:
                GoingToArea(currentAreaIndex);
                break;
            case StudentState.WaitingInArea:
                WaitingInArea(currentAreaIndex);
                break;
        }
    }

    void Update()
    {
        //this part is temporary to fix a bug, it should not be in updates
        if (currentDestination != null)
        {
            MoveToPosition(currentDestination);
        }

        if (navMeshAgent.velocity.sqrMagnitude < 0.1f)
        {

            if (studentStates[currentStateIndex] == StudentState.WaitingInArea && areas[currentAreaIndex].HasAvailablePlace())
            {
                ChangeState(-1);
            }
            ChangeAnimation("IsWalking", false);
            Rotate();
        }
        else
            ChangeAnimation("IsWalking", true);



    }
    public void ChangeState(int index)
    {
       
       currentStateIndex +=index;
       if (currentStateIndex >= studentStates.Count)
       {
            currentStateIndex = 2;
            currentAreaIndex = 1;
       }
       currentState = studentStates[currentStateIndex];
        
        CheckAreas();
    }

    void ChangeAnimation(string boolName,bool change)
    {
        if (animator.GetBool(boolName) != change)
            animator.SetBool(boolName, change);
    }
    public void MoveToPosition(Vector3 targetPosition)
    {
       navMeshAgent.SetDestination(targetPosition);

    }
    void GoingToArea(int areaIndex)
    {
        ChangeAnimation("IsWalking", true);
        if (areas[areaIndex].HasAvailablePlace())
            IsInsideArea(areaIndex);
        else
            ChangeState(1);
    }
    void WaitingInArea(int areaIndex)
    {
        currentDestination = (areas[areaIndex].GoToWaitLine().position);
       // MoveToPosition(areas[areaIndex].GoToWaitLine().position);
    }
    void IsInsideArea(int areaIndex)
    {
        //MoveToPosition(areas[areaIndex].AddOccupied(this).position);
        currentDestination = areas[areaIndex].AddOccupied(this).position;
        areas[areaIndex].RemoveFromWaiting(this);
    }
   
    IEnumerator StartActionInArea(int areaIndex)
    {
        studentUi.LoadingBar(areas[areaIndex].GetInActionDuration() - .3f);
        yield return new WaitForSeconds(areas[areaIndex].GetInActionDuration());
        currentAreaIndex++;
        studentLevel.AddExperience (areas[areaIndex].GetStudentExperiencePerLesson());
        areas[areaIndex].RemoveOccupied(this);
        GameManager.Instance.UpdateCoins(areas[areaIndex].GetSubscriptionFee());
        transform.SetParent(null);
        navMeshAgent.Resume();
        studentLevel.Checklevel();
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seat"))
        {
            switch (studentStates[currentStateIndex])
            {
                case StudentState.GoingToArea:
                    if(Vector3.Distance(other.transform.position,currentDestination) < .5f)
                    {
                        transform.SetParent(other.transform);
                        Invoke("StopAgent", 1);


                        StartCoroutine(StartActionInArea(currentAreaIndex));
                    }
                   
                    break;
            }
        }
    }
    void StopAgent()
    {
        navMeshAgent.Stop();
     
        
    }
    private void OnTriggerStay(Collider other) // this part needs to be changed too for optimisation, its temporary to fix a bug , it should not be called  OntriggerStay
    {
        if (other.CompareTag("Wait"))
        {
            switch (studentStates[currentStateIndex])
            {
                case StudentState.WaitingInArea:
                    currentDestination = areas[currentAreaIndex].AddToWaiting(this).position;
                    //MoveToPosition(areas[currentAreaIndex].AddToWaiting(this).position);
                    
                    break;

            }

        }
    }
   

    public void Rotate()
    {
        Quaternion targetRot = Quaternion.Euler(0f, 90, 0f);
        if (transform.rotation!= targetRot)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 6* Time.deltaTime);
      
    }

    private IEnumerator WaitForAvailableWaitingPosition(int areaIndex)
    {
        yield return new WaitUntil(() => areas[areaIndex].HasAvailablePlace());


        MoveToPosition(areas[currentAreaIndex].AddToWaiting(this).position);
    }






}
