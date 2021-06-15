using System;
using System.Collections.Generic;
using Entity;
using UI.Jump_Run;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private int width;
    [SerializeField] private GameObject blockType;
    [SerializeField] private Transform slimeTransform;
    [SerializeField] private UIManagerJumpRun uiManager;
    private int lowest;
    private List<GameObject> level;
    private int currentLevel = 1;
    private int lives = 3;
    private GameObject finishObject;

    void Start()
    {
        currentLevel = 1;
        level = new List<GameObject>();
        GenerateLevel();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.ToggleSettings();
        }
    }

    void FixedUpdate()
    {
        uiManager.UpdateWayDescription(width, (int) Math.Floor(slimeTransform.transform.position.x));
        if (slimeTransform.position.y < lowest - 5)
        {
            if (lives > 0)
            {
                lives--;
                uiManager.UpdateHealth(lives);
                slimeTransform.position = new Vector3(0, 0, 0);
            }
            else
            {
                // he dead
                slimeTransform.position = new Vector3(0, 0, 0);
                uiManager.GameOver();
            }
        }


        Vector3 positionLastBlock = finishObject.transform.position;
        positionLastBlock.y += .5f;
        if (Physics.OverlapSphere(positionLastBlock, 0.1f, playerMask).Length >= 1)
        {
            Debug.Log("WON");
            slimeTransform.position = new Vector3(0, 0, 0);
            NextLevel();
        }
    }

    private void NextLevel()
    {
        DestroyLevel();
        SaveState.GetInstance().SaveLevel(currentLevel);
        currentLevel++;
        GenerateLevel();
    }

    private void DestroyLevel()
    {
        level.ForEach(Destroy);
        level = new List<GameObject>();
    }

    private void GenerateLevel()
    {
        int prevPosition = 0;
        int skipped = 0;
        int lowest = 0;
        float percentageForSpace = 1f * currentLevel;
        for (int i = 0; i <= width; i++)
        {
            bool isBeginningOrEnd = i == width || i == 0;
            float placeBlock = GenerateRandom(i);
            // Cannot leave a gap if there were 3 consecutive gaps or the block to place is the last block
            if (placeBlock > percentageForSpace || skipped >= 3 || isBeginningOrEnd)
            {
                skipped = 0;

                if (i != 0)
                {
                    if (placeBlock < 3.33f)
                    {
                        prevPosition--;
                        if (prevPosition < lowest)
                        {
                            lowest = prevPosition;
                        }
                    }
                    else if (placeBlock > 6.66f)
                    {
                        prevPosition++;
                    }
                }

                GameObject newBlock =
                    Instantiate(blockType, new Vector3(i, prevPosition - 0.5f, 0), Quaternion.identity);
                level.Add(newBlock);
                newBlock.transform.parent = transform;
            }
            else
            {
                skipped++;
            }
        }


        this.lowest = lowest;
        finishObject = level[level.Count - 1];
        CorrectLevel();
        uiManager.UpdateLevelDescription(currentLevel);
    }

    private float GenerateRandom(float x)
    {
        return Random.Range(0, 10);
    }

    private void CorrectLevel()
    {
        for (var i = 1; i < level.Count - 1; i++)
        {
            Vector3 curr = level[i].transform.localPosition;
            Vector3 prev = level[i - 1].transform.localPosition;
            Vector3 next = level[i + 1].transform.localPosition;

            const double tolerance = 0.10;
            // check only if there is no gap inbetween the prev and curr and next and curr
            if (curr.x - 1 == prev.x &&
                curr.x + 1 == next.x)
            {
                if (prev.y == next.y && prev.y != curr.y)
                {
                    Debug.Log("Change y of " + i + " from " + curr.y + " to " + prev.y);
                    level[i].transform.localPosition = new Vector3(curr.x, prev.y, curr.z);
                    //level[i].SetActive(true);
                }
            }
        }
    }
}