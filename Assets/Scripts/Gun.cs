using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private float fireRate = 10f; // 重复射击间隔
    [SerializeField] private float range = 20f; // 射程
    [SerializeField] private float impactForce = 150f; // 击中物体时的冲击力

    [SerializeField] private ParticleSystem muzzleFlash; // 射击时产生的火光粒子系统

    private InputAction shootAction; // 输入动作

    [SerializeField] private Transform muzzle; // 瞄准器
    [SerializeField] private GameObject impactEffectd; // 击中效果

    private float nextTimeToFire = 0; // 下次射击的时间

    [SerializeField] private Animator animator;

    [SerializeField] private TextMeshProUGUI ammoInfoText;
    
    public int currentAmmo;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private int magazineAmmo = 30;

    public bool isReloading = false;
    
    private void Start()
    {
        shootAction = new InputAction("Shoot", binding: "<mouse>/leftButton");
        shootAction.AddBinding("<Gamepad>/x");
        shootAction.Enable(); // 启用输入动作

        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("isReloading", false);
    }

    private void Update()
    {
                // 设置ammoInfoText的文本为当前弹药数除以弹夹弹药数
        ammoInfoText.text = currentAmmo + "/" + magazineAmmo;

        // 如果当前弹药数等于0且弹夹弹药数等于0
        if (currentAmmo == 0 && magazineAmmo == 0)
        {
            // 将isShooting的布尔值设置为false
            animator.SetBool("isShooting", false);
            // 返回
            return;
        }

        // 如果正在重新装填
        if (isReloading)
        {
            // 返回
            return;
        }

        
        bool isShooting = Mathf.Approximately(shootAction.ReadValue<float>(), 1f); // 判断是否正在射击
        animator.SetBool("isShooting", isShooting);
        // Time.time: 返回以秒为单位的从游戏启动到当前时间的秒数（就是游戏运行时间）
        if (isShooting && Time.time >= nextTimeToFire) // 如果正在射击且达到下次射击的时间
        {
            nextTimeToFire = Time.time + 1f / fireRate; // 更新下次射击时间
            Fire(); // 开始射击
        }

        if (currentAmmo == 0 && magazineAmmo > 0 && !isReloading)
        {
            // reload...
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        AudioManager.instance.Play("Shoot"); // 播放射击音效
        muzzleFlash.Play(); // 播放火光粒子系统

        currentAmmo--;

        RaycastHit hit; // 创建一个RaycastHit类型的变量hit，用于存储碰撞信息

        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range)) // 判断是否击中物体
        {
            if (hit.rigidbody) // 如果击中的物体有刚体的话
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce); // 对击中的物体沿法线的反方向施加一个力
            }
            
            // 调用Enemy的TakeDemage方法伤害Enemy
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDemage(10);
                return;
            }
            
            Quaternion impactRotation = Quaternion.LookRotation(hit.normal); // 创建一个沿着击中方向的旋转四元数
            GameObject impactEffect = Instantiate(impactEffectd, hit.point, impactRotation); // 实例化击中效果的物体
            impactEffect.transform.parent = hit.transform; // 将击中效果的父节点设置为击中物体的父节点
        }
    }
    
   IEnumerator Reload()
    {
        // 判断是否正在装填
        isReloading = true;
        AudioManager.instance.Play("Reload"); // 播放射击音效
        // 设置动画播放状态为正在装填
        animator.SetBool("isReloading", true);
        // 等待2秒
        yield return new WaitForSeconds(2f);
        // 设置动画播放状态为非装填
        animator.SetBool("isReloading", false);
        
        // 检查弹夹剩余子弹数是否大于等于最大子弹数
        if (magazineAmmo >= maxAmmo)
        {
            // 将当前子弹数设为最大子弹数
            currentAmmo = maxAmmo;
            // 减去满载的子弹数
            magazineAmmo -= maxAmmo;
        }
        else
        {
            // 将当前子弹数设为弹夹剩余子弹数
            currentAmmo = magazineAmmo;
            // 清空弹夹剩余子弹数
            magazineAmmo = 0;
        }

        // 判断是否正在装填
        isReloading = false;
    }

}