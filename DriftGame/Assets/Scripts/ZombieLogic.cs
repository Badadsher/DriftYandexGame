
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using YG;

public class ZombieLogic : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;
    public float attackDistance = 1.5f; // Расстояние для атаки
    public float rotationSpeed = 10f; // Увеличенная скорость поворота
    private Animator animator;
    private Rigidbody rb; // Rigidbody для физики
    private bool isAttacking = false; // Флаг для отслеживания состояния атаки
    private ParticleSystem dyingPart;
    private TextMeshProUGUI countZombie;
    private GameManager _gameManager;
    public delegate void ZombieDestroyedHandler();
    public event ZombieDestroyedHandler OnZombieDestroyed;

    void Start()
    {
        countZombie = GameObject.Find("KilledCount").GetComponent<TMPro.TextMeshProUGUI>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player")?.transform;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

        if (navMeshAgent.enabled)
        {
            // Если игрок далеко, зомби движется к нему
            if (distanceToPlayer > attackDistance)
            {
                navMeshAgent.SetDestination(player.position);
                RotateTowardsPlayer();
            }
            else // Если игрок в зоне атаки
            {
                if (!isAttacking) // Проверка на состояние атаки
                {
                    StartAttack();
                }
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void StartAttack()
    {
        isAttacking = true; // Устанавливаем флаг атаки
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true; // Остановить зомби перед атакой
        animator.SetBool("Punching", true); // Запуск анимации удара
        StartCoroutine(AttackCooldown()); // Запускаем корутину атаки
    }

    private System.Collections.IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f); // Задержка перед проверкой атаки
        float distanceToPlayerForKill = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayerForKill <= attackDistance)
        {
            _gameManager.MinusHp();
        }
        yield return new WaitForSeconds(1f); // Задержка после удара
        animator.SetBool("Punching", false); // Вернуться к состоянию ожидания или бега
        yield return new WaitForSeconds(2f); // Задержка между атаками
        isAttacking = false; // Сбрасываем флаг атаки
        navMeshAgent.isStopped = false; // Возобновить движение после атаки
    }

    void OnTriggerEnter(Collider other)
    {
        PrometeoCarController prometeoCarController = other.GetComponent<PrometeoCarController>();
        if (prometeoCarController != null)
        {
            float carSpeed = prometeoCarController.carSpeed;
            if (carSpeed >= 40 && other.CompareTag("Player"))
            {
                Vector3 forceDirection = (transform.position - other.transform.position).normalized; // Направление от автомобиля
                rb.isKinematic = false; // Убедитесь, что Rigidbody не кинематический
                rb.velocity = Vector3.zero; // Отключаем текущее движение перед применением силы
                rb.AddForce(forceDirection * 500f, ForceMode.Impulse); // Применяем силу (настройте значение по необходимости)
                StartCoroutine(KillZombie());
            }
        }
    }

    private System.Collections.IEnumerator KillZombie()
    {
        dyingPart = transform.Find("DyingParticle").GetComponent<ParticleSystem>();
        dyingPart.Play();

        animator.Play("die");
        OnZombieDestroyed?.Invoke(); // Проверяем, есть ли подписчики и вызываем событие

        yield return new WaitForSeconds(2f); // Время анимации смерти

        SaveManager.SetKilledZombiesCount();

        int zb = SaveManager.LoadKilledZombies();
        countZombie.text = zb + "/20";

        Destroy(gameObject); // Уничтожаем объект после исчезновения
    }
}