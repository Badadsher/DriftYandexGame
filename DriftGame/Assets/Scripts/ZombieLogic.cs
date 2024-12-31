using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using YG;
using TMPro;

public class ZombieLogic : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;
    public float attackDistance = 1.5f; // Расстояние для атаки
    public float rotationSpeed = 5f; // Скорость поворота
    private Animator animator;
    private Rigidbody rb; // Rigidbody для физики
    private bool isAttacking = false; // Флаг для отслеживания состояния атаки
    private ParticleSystem dyingPart;
    private TextMeshProUGUI countZombie;
    
    public delegate void ZombieDestroyedHandler();
    public event ZombieDestroyedHandler OnZombieDestroyed;

    void Start()
    {
        countZombie = GameObject.Find("KilledCount").GetComponent<TMPro.TextMeshProUGUI>();
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

        if (  navMeshAgent.enabled == true)
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
        ParticleSystem.MainModule mainModule = dyingPart.main;
        float initialLifetime = mainModule.startLifetime.constant;
        // Постепенно уменьшаем альфа-канал материала и lifetime частиц
        animator.Play("die");

        // Получаем компонент SkinnedMeshRenderer
        SkinnedMeshRenderer skinnedMeshRenderer = transform.Find("zombie").GetComponent<SkinnedMeshRenderer>();
        Material material = skinnedMeshRenderer.material; // Получаем материал
         // Проверяем, есть ли подписчики и вызываем событие
        // Постепенно уменьшаем альфа-канал
        Color color = material.color; // Получаем текущий цвет материала
        OnZombieDestroyed?.Invoke(); // Проверяем, есть ли подписчики и вызываем событие
        Debug.Log("Событие уничтожения зомби вызвано."); // Лог для отладки
        for (float t = 0; t < 1; t += Time.deltaTime / 2) // 2 секунды для исчезновения
        {
            color.a = Mathf.Lerp(1, 0, t); // Плавно уменьшаем альфа-канал от 1 до 0
            material.color = color; // Применяем новый цвет
            mainModule.startColor = new ParticleSystem.MinMaxGradient(color); // Применяем новый цвет
            // Плавно уменьшаем lifetime частиц
            mainModule.startLifetime = Mathf.Lerp(initialLifetime, 0, t); // Уменьшаем lifetime
            yield return null; // Ждем один кадр
        }
// Вызываем событие перед уничтожением объекта
       
        SaveManager.SetKilledZombiesCount();
        int zb = SaveManager.LoadKilledZombies();
        countZombie.text = zb + "/20";
    
        Destroy(gameObject); // Уничтожаем объект после исчезновения
    }
}
