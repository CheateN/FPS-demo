using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scope : MonoBehaviour
{
    InputAction scopeAction;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject scopeOverlay;
    private bool isScoped = false;
    [SerializeField] private Camera camera;
    private Gun gun;

    private void Start()
    {
        scopeAction = new InputAction("Scope", binding: "<mouse>/rightButton");
        scopeAction.Enable();
        gun = GetComponent<Gun>();
    }

    private void Update()
    {
        if (gun.isReloading)
        {
            OnUnScoped();
        }
        else
        {
            if (scopeAction.triggered)
            {
                isScoped = !isScoped;
                if (isScoped)
                {
                    StartCoroutine(OnScoped());
                }
                else
                {
                    OnUnScoped();
                }
            }
        }
    }

    IEnumerator OnScoped()
    {
        animator.SetBool("isScoped", true);
        yield return new WaitForSeconds(0.25f);
        scopeOverlay.SetActive(true);
        camera.cullingMask = camera.cullingMask & ~(1 << 7);
        camera.fieldOfView = 45;
        // Debug.Log(Convert.ToString(~(1 << 7), 2));
    }

    void OnUnScoped()
    {
        animator.SetBool("isScoped", false);
        scopeOverlay.SetActive(false);
        camera.cullingMask = camera.cullingMask | (1 << 7);
        camera.fieldOfView = 60;
    }
}