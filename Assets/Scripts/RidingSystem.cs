using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RidingSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ridingMessage;
    private InputAction rideAction;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject robot;

    private void Start()
    {
        rideAction = new InputAction("Ride", binding: "<Keyboard>/F");
        rideAction.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        ridingMessage.enabled = true;
        ridingMessage.text = "Press F To Ride The Robot";
    }

    private void OnTriggerStay(Collider other)
    {
        if (rideAction.triggered)
        {
            Ride();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ridingMessage.enabled = false;
    }

    private void Ride()
    {
        player.SetActive(false);
        robot.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}