
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using YG;
using Zenject;

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
    private bool isDead = false; // Флаг для отслеживания состояния смерти
    public AudioClip[] zombieSounds; // Аудиоклип, который будет воспроизводиться
    private AudioSource audioSource; // Компонент AudioSource
    public delegate void ZombieDestroyedHandler();
    public event ZombieDestroyedHandler OnZombieDestroyed;

    private SaveLoadManager _saveLoadManager;
    
    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
    }
    
    void Start()
    {
        countZombie = GameObject.Find("KilledCount").GetComponent<TMPro.TextMeshProUGUI>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindTargetedPlayer().transform;
        _gameManager = FindFirstObjectByType<GameManager>();
        rb = GetComponent<Rigidbody>();

        // Настройки NavMeshAgent
        navMeshAgent.speed = 6f; // Установите максимальную скорость
        navMeshAgent.acceleration = 20f; // Установите максимальное ускорение
        navMeshAgent.angularSpeed = 360f; // Установите угловую скорость
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
        StartCoroutine(PlayRandomSound());
    }

    void Update()
    {
        if (player == null || isDead || !player.GetComponent<Player>().isTarget)
        {
            print("Target Changed");
            player = FindTargetedPlayer().transform;
            return; // Проверка на случай, если игрок не найден или зомби мертв
        }

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
        if (isDead) return; // Проверка на состояние смерти перед атакой
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
        if (distanceToPlayerForKill <= attackDistance && !isDead) // Проверка на смерть перед нанесением урона
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
            if (Mathf.Abs(carSpeed) >= 30 && other.TryGetComponent<Player>(out Player player))
            {
                Vector3 forceDirection = (transform.position - other.transform.position).normalized; // Направление от автомобиля
                rb.isKinematic = false; // Убедитесь, что Rigidbody не кинематический
                rb.velocity = Vector3.zero; // Отключаем текущее движение перед применением силы
                rb.AddForce(forceDirection * 500f, ForceMode.Impulse); // Применяем силу (настройте значение по необходимости)
                StartCoroutine(KillZombie());
            }
        }

        float pushForce = 1000f;

        // Проверяем, есть ли Rigidbody у вошедшего объекта
        Rigidbody playerRB = other.attachedRigidbody;

        if (playerRB != null && playerRB.TryGetComponent<Player>(out Player playerPlayer))
        {
            // Вычисляем направление отталкивания от центра триггера
            Vector3 pushDirection = (other.transform.position - transform.position).normalized;

            // Применяем силу
            playerRB.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    private System.Collections.IEnumerator KillZombie()
    {
        isDead = true; // Устанавливаем флаг смерти
        dyingPart = transform.Find("DyingParticle").GetComponent<ParticleSystem>();
        dyingPart.Play();

        animator.Play("die");
        OnZombieDestroyed?.Invoke(); // Проверяем, есть ли подписчики и вызываем событие

        yield return new WaitForSeconds(2f); // Время анимации смерти

        _saveLoadManager.SetKilledZombiesCount();

        int zb = _saveLoadManager.LoadKilledZombies();
        countZombie.text = zb + "/20";

        Destroy(gameObject); // Уничтожаем объект после исчезновения
    }
    private System.Collections.IEnumerator PlayRandomSound()
    {
        while (!isDead) // Пока зомби не мертв
        {
            float randomDelay = Random.Range(5f, 10f); // Генерируем случайную задержку от 5 до 10 секунд
            
            yield return new WaitForSeconds(randomDelay); 

            audioSource.clip = zombieSounds[Random.Range(0, zombieSounds.Length)]; // Выбираем случайный звук из массива
            
            audioSource.Play(); // Воспроизводим звук
            
            yield return new WaitForSeconds(audioSource.clip.length); // Ждем окончания воспроизведения звука перед следующей задержкой
        }
    }

    private GameObject FindTargetedPlayer()
    {
        Player[] players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach(Player player in players)
        {
            if (player.isTarget)
                return player.gameObject;
        }
        return null;
    }
}