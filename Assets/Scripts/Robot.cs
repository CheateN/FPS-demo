using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Robot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ridingMessage;
    private InputAction exitAction;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ridingSystem;

    private void OnEnable()
    {
        exitAction = new InputAction("Exit", binding: "<Keyboard>/E");
        exitAction.Enable();
        
        ridingMessage.text = "Press E to exit the robot";
    }


    private void Update()
    {
        if (exitAction.triggered)
        {
            ExitRobot();
        }
    }


    private void ExitRobot()
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.SetActive(true);
        transform.gameObject.SetActive(false);
        ridingSystem.SetActive(true);
        ridingSystem.transform.position = player.transform.position;
    }
}