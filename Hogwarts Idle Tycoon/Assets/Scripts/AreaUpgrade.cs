using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUpgrade : MonoBehaviour
{
    public List<GameObject> tables;
    private List<Vector3> initialTablePositions = new List<Vector3>();
    [SerializeField] Transform areaBounds;
    [SerializeField] float lerpSpeed = 1f;

    private int currentTableIndex = 1;

    private void Start()
    {
        // Store the initial positions of the tables
        foreach (GameObject table in tables)
        {
            initialTablePositions.Add(table.transform.position);
        }
    }

    public void ArrangeTables()
    {
        if (currentTableIndex >= tables.Count)
        {
            Debug.Log("All tables have been arranged.");
            return;
        }

        // Activate the next table in the array
        tables[currentTableIndex].SetActive(true);

        int tableCount = currentTableIndex + 1; // Increment the table count on each call
        tableCount = Mathf.Min(tableCount, tables.Count); // Limit the table count to the total number of tables
        Vector3 roomSize = areaBounds.localScale;
        int lineCount = Mathf.CeilToInt((float)tableCount / 3);
        float distanceBetweenLines = roomSize.x / (lineCount + 1);
        float startLinePosition = lineCount * distanceBetweenLines / 2f;

        for (int i = 0; i < tableCount; i++)
        {
            int lineIndex = i / 3;
            int tableIndexInLine = i % 3;

            float linePosition = startLinePosition - (lineIndex * distanceBetweenLines);
            float distanceBetweenTables = roomSize.z / 4;
            float tableOffset = distanceBetweenTables * (tableIndexInLine - (lineCount == 1 ? 0.5f : 1));

            Vector3 targetPosition = new Vector3(linePosition, 0f, tableOffset);
            StartCoroutine(LerpTablePosition(tables[i].transform, targetPosition));
        }

        currentTableIndex++; // Increment the current table index after arranging the tables
    }

    private IEnumerator LerpTablePosition(Transform table, Vector3 targetPosition)
    {
        Vector3 startPosition = table.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < lerpSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpSpeed);
            table.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        table.localPosition = targetPosition; // Ensure the table reaches the exact target position
    }
}