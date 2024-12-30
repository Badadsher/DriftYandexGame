using Unity.VisualScripting;
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
    private ParticleSystem dyingPart;
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

       
        
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (  navMeshAgent.enabled = true)
        {
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

    void OnTriggerEnter(Collider other)
    {
        PrometeoCarController prometeoCarController = other.GetComponent<PrometeoCarController>();
        float carSpeed = prometeoCarController.carSpeed;
        if (carSpeed >= 40)
        {

            if (other.CompareTag("Player"))
            {
                Vector3 forceDirection = (transform.position - other.transform.position).normalized; // Направление от автомобиля
                rb.isKinematic = false; // Убедитесь, что Rigidbody не кинематический
                rb.AddForce(forceDirection * 100000f); // Применяем силу (настройте значение по необходимости)
                StartCoroutine(KillZombie());
            }
        }
    }

    private System.Collections.IEnumerator KillZombie()
    {
        dyingPart = transform.Find("DyingParticle").GetComponent<ParticleSystem>();
        dyingPart.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
