using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public Text textVictory;

    public GameObject collisionManager;

    private CollisionManager _collisionManager;
    // Start is called before the first frame update
    void Start()
    {
        _collisionManager = GameObject.Find("CollisionManager").GetComponent<CollisionManager>();

        textVictory = GameObject.Find("UI/VictoryText").GetComponent<Text>();
        textVictory.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (CollisionManager.victoryCondition)
        {
            textVictory.text = "Physics Exam Done !!";
        }
    }
}
