using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    private InputAction switchAction;
    [SerializeField] private int selectedWeaponIndex = 0;
    // public GameObject[] weapons;


    private void Start()
    {
        // 创建一个新的InputAction对象，名称为"Scroll"，绑定为"<Mouse>/scroll"
        switchAction = new InputAction("Scroll", binding: "<Mouse>/scroll");

        // 为switchAction添加一个新的绑定"<Gamepad>/Dpad"
        switchAction.AddBinding("<Gamepad>/Dpad");

        // 启用switchAction
        switchAction.Enable();

        SelectWeapon();
    }

    private void Update()
    {
        float scrollValue = switchAction.ReadValue<Vector2>().y;

        int prevSelectWeaponIndex = selectedWeaponIndex;
        if (scrollValue > 0) // 向下滚动
        {
            selectedWeaponIndex++;
            if (selectedWeaponIndex == transform.childCount)
            {
                selectedWeaponIndex = 0;
            }
        }
        else if (scrollValue < 0) // 向上滚动
        {
            selectedWeaponIndex--;
            if (selectedWeaponIndex == -1)
            {
                selectedWeaponIndex = transform.childCount - 1;
            }
        }

        if (prevSelectWeaponIndex != selectedWeaponIndex)
        {
            SelectWeapon();
        }
    }

    private void SelectWeapon()
    {
        // 遍历this.transform下的所有Transform对象
        foreach (Transform weapon in this.transform)
        {
            // 将武器对象设置为不可用
            weapon.gameObject.SetActive(false);
        }

        // 激活指定索引下的子对象
        transform.GetChild(selectedWeaponIndex).gameObject.SetActive(true);
    }
}