using UnityEngine;
using UnityEngine.AI;

public class ZombieLogic : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;
    public float attackDistance = 1.5f; // Расстояние для атаки
    public float rotationSpeed = 5f; // Скорость поворота
    public Animator animator;
    private Rigidbody rb; // Rigidbody для физики
    private bool isAttacking = false; // Флаг для отслеживания состояния атаки

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the tag 'Player'.");
        }
        
        rb = GetComponent<Rigidbody>();
        // Настройки NavMeshAgent
        navMeshAgent.speed = 6f; // Установите максимальную скорость
        navMeshAgent.acceleration = 20f; // Установите максимальное ускорение
        navMeshAgent.angularSpeed = 360f; // Установите угловую скорость
    }

    void Update()
    {
        if (player == null) return; // Проверка на случай, если игрок не найден

        void OnCollisionEnter(Collision collision)
        {
            // Проверка на столкновение с игроком
            if (collision.gameObject.CompareTag("Player"))
            {
                PrometeoCarController carController = gameObject.GetComponent<PrometeoCarController>();
                float carSpeed = carController.carSpeed;

                if (carSpeed >= 30)
                {
                    Debug.Log("naam");
                    // Отключаем анимацию
                    animator.enabled = false;

                    // Включаем физику: добавляем силу в сторону игрока
                    Vector3 forceDirection = (transform.position - collision.transform.position).normalized; // Направление от игрока
                    rb.isKinematic = false; // Убедитесь, что Rigidbody не кинематический
                    rb.AddForce(forceDirection * 500f); // Применяем силу

                    // Отключаем NavMeshAgent
                    navMeshAgent.enabled = false; 
                }
               
            }
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackDistance)
        {
            if (!isAttacking) // Проверяем, не атакует ли зомби в данный момент
            {
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    navMeshAgent.SetDestination(player.position);
                }

              

                // Плавный поворот к игроку
                Vector3 direction = (player.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                }
            }
        }
        else
        {
            if (!isAttacking) // Проверка на состояние атаки
            {
                isAttacking = true; // Устанавливаем флаг атаки
                navMeshAgent.isStopped = true; // Остановить зомби перед атакой
                animator.SetBool("Punching", true); // Запуск анимации удара
                navMeshAgent.ResetPath();
                StartCoroutine(AttackCooldown()); // Запускаем корутину атаки
            }
        }
    }

    private System.Collections.IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f); // Задержка между атаками (например, 1 секунда)

        animator.SetBool("Punching", false); // Вернуться к состоянию ожидания или бега

        isAttacking = false; // Сбрасываем флаг атаки

        // После завершения атаки возобновляем движение только если зомби не в пределах расстояния атаки
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackDistance)
        {
            navMeshAgent.isStopped = false; // Возобновить движение после атаки
        }
    }
}