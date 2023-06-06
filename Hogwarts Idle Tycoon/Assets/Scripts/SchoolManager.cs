using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SchoolManager : MonoBehaviour
{
    public static SchoolManager Instance;

    [SerializeField] bool spawn;

    public GameObject studentPrefab;
    public Transform spawnPosition;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;
    public List<Student> Students = new List<Student>();
    public List<Area> areas = new List<Area>();
    int studentNameIndex=1;
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
       
        StartCoroutine(SpawnStudentsWithRandomDelay());
    }
    public void SetAreas()
    {
        SortedDictionary<int, Area> orderedInstances = new SortedDictionary<int, Area>();

        foreach (KeyValuePair<string, Area> kvp in Area.Instances)
        {
            int order = kvp.Value.areaData.preriority;
            if (!orderedInstances.ContainsKey(order))
            {
                orderedInstances.Add(order, kvp.Value);
            }
        }

        areas.Clear();

        foreach (KeyValuePair<int, Area> kvp in orderedInstances)
        {
            areas.Add(kvp.Value);
        }
    }
   
   
    IEnumerator SpawnStudentsWithRandomDelay()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
            if (spawn)
            {
                Student st = Instantiate(studentPrefab, spawnPosition.position, spawnPosition.rotation).GetComponent<Student>();
                st.areas = areas;
                st.name = ("Student" + studentNameIndex.ToString());
                studentNameIndex++;
                Students.Add(st);
            }
        }
    }
    

}
