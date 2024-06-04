using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHP = 100f; // HP: Health Point

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectilePoint;

    public void TakeDemage(int demageAmount)
    {
        enemyHP -= demageAmount;
        if (enemyHP <= 0)
        {
            animator.SetTrigger("death");
            // 获取当前物体的CapsuleCollider组件并禁用它
            // 防止炸死后再次被攻击
            GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            animator.SetTrigger("demage");
        }
    }

    public void Shoot()
    {
        // 在指定位置实例化一个物体，并获取实例化后的物体的刚体组件赋值给变量rb
        Rigidbody rb = Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        // 给刚体rb施加一个向前的力，大小为30f
        rb.AddForce(transform.forward * 30f, ForceMode.Impulse);
        // 给刚体rb施加一个向上的力，大小为7f
        rb.AddForce(transform.up * 7f, ForceMode.Impulse);
    }
}