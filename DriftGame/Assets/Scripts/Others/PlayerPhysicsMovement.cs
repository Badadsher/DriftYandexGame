using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerPhysicsMovement : MonoBehaviour
{
    public float moveForce = 20f;                   // Сила движения
    public float maxSpeed = 5f;                     // Максимальная скорость
    [Range(0f, 360f)] public float rotationAngle = 0f; // Угол ориентации управления
    public Joystick joystick;                       // Виртуальный джойстик

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        if (Application.isMobilePlatform && joystick != null)
        {
            inputX = joystick.Horizontal;
            inputZ = joystick.Vertical;
        }

        Vector3 inputDir = new Vector3(inputX, 0f, inputZ);

        if (inputDir.sqrMagnitude > 0.01f)
        {
            // Поворачиваем направление ввода
            Quaternion rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            Vector3 forceDir = rotation * inputDir.normalized;

            // Только если скорость не превышена — добавляем силу
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(forceDir * moveForce, ForceMode.Acceleration);
            }
        }
    }
}
