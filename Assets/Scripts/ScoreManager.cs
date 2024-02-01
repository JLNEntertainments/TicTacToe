using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonGeneric<ScoreManager>
{
    static int playerScore, AIScore;
    [HideInInspector]
    public int tempPlayerScore, tempAIScore;

    // Start is called before the first frame update
    void Start()
    {
        tempPlayerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerScore = tempPlayerScore;
        AIScore = tempAIScore;
    }
}
