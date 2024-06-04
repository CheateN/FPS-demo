using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    private CharacterController controller; // CharacterController组件引用
    [SerializeField] private float speed = 15f; // 移动速度
    private Vector3 move; // 移动向量
    [SerializeField] private float mouseSensitivity = 100f; // 鼠标灵敏度
    [SerializeField] private Camera playerHead; // 玩家头部相机引用
    private float xRotation = 0; // X轴旋转角度
    private float gravity = -9.81f; // 重力加速度
    private Vector3 velocity; // 垂直速度
    [SerializeField] private float jumpForce = 8f; // 跳跃力
    private bool isGrounded = false; // 是否接触地面
    [SerializeField] private GameObject groundCheck; // 地面检查对象
    [SerializeField] private LayerMask groundMask; // 地面检查层掩码

    private InputAction moveAction; // 移动输入
    private InputAction jumpAction; // 跳跃输入

    [SerializeField] private Animator animator;

    private float playerHP = 100f;
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private GameObject bloodOverlay;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        controller = GetComponent<CharacterController>(); // 获取CharacterController组件
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标

        jumpAction = new InputAction("Jump", binding: "<Gamepad>/a");
        jumpAction.AddBinding("<keyboard>/space");
        jumpAction.Enable();

        moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<keyboard>/w")
            .With("Up", "<keyboard>/upArrow")
            .With("Down", "<keyboard>/s")
            .With("Down", "<keyboard>/downArrow")
            .With("Left", "<keyboard>/a")
            .With("Left", "<keyboard>/leftArrow")
            .With("Right", "<keyboard>/d")
            .With("Right", "<keyboard>/rightArrow");
        moveAction.Enable();
    }

    void Update()
    {
        // 玩家移动: 旧输入系统
        // float x = Input.GetAxis("Horizontal");  // 获取水平方向的输入值
        // float z = Input.GetAxis("Vertical");  // 获取垂直方向的输入值

        // 玩家移动: 新输入系统
        float x = moveAction.ReadValue<Vector2>().x;
        float z = moveAction.ReadValue<Vector2>().y;
        // 根据当前物体的右向量和前向向量计算移动向量
        move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime); // 移动CharacterController
        animator.SetFloat("speed", Mathf.Abs(x) + Mathf.Abs(z));

        // 鼠标转向: 旧输入系统
        // 获取鼠标在水平方向上的移动距离，并根据鼠标灵敏度计算水平旋转角度
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        // 获取鼠标在垂直方向上的移动距离，并根据鼠标灵敏度计算垂直旋转角度
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        // 以Y轴为轴心，按照水平旋转角度旋转物体

        // 鼠标转向: 新输入系统
        float mouseX = 0;
        float mouseY = 0;

        if (Mouse.current != null) // 如果使用的是电脑鼠标的输入
        {
            mouseX = Mouse.current.delta.ReadValue().x;
            mouseY = Mouse.current.delta.ReadValue().y;
        }

        if (Gamepad.current != null) // 如果使用的是游戏手柄的输入
        {
            mouseX = Gamepad.current.rightStick.ReadValue().x;
            mouseY = Gamepad.current.rightStick.ReadValue().y;
        }

        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX * Time.deltaTime);

        // 更新X轴旋转角度
        xRotation -= mouseY * Time.deltaTime;
        // 将X轴旋转角度限制在-80f和80f之间
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        // 将头部物体的局部旋转设为(XRotation, 0, 0)
        playerHead.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        isGrounded = Physics.CheckSphere(groundCheck.transform.position, 0.3f, groundMask); // 判断是否接触地面
        // 地面检查
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; // 跳跃力方向向上
        }

        // 玩家跳跃: 旧输入系统
        // if (Input.GetButtonDown("Jump") && isGrounded)
        // if(jumpAction.ReadValue<float>() > 0 && isGrounded)
        if (Mathf.Approximately(jumpAction.ReadValue<float>(), 1f) && isGrounded)
        {
            velocity.y = jumpForce; // 设置垂直速度为跳跃力
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // 垂直速度增加重力
        }

        controller.Move(velocity * Time.deltaTime); // 控制CharacterController移动
    }

    public IEnumerator TakeDamage(int damageAmount)
    {
        bloodOverlay.SetActive(true);
        playerHP -= damageAmount;
        playerHPText.text = "+" + playerHP;
        if (playerHP <= 0)
        {
            playerHPText.text = "0";
            gameManager.GameOver();
        }

        yield return new WaitForSeconds(1);
        bloodOverlay.SetActive(false);
    }
}