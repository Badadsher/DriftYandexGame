using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCarController : MonoBehaviour
{
    public Transform[] waypoints; // Массив путевых точек
    public float speed = 10f; // Скорость NPC
    public float turnSpeed = 3f; // Скорость поворота
    public float waypointThreshold = 3f; // Радиус достижения точки
    public float steeringSmoothness = 0.3f; // Коэффициент сглаживания поворота
    
    private int currentWaypointIndex = 0;
    
    void Update()
    {
        if (waypoints.Length == 0) return;
        
        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;
        
        // Предсказание будущего направления
        Vector3 futureDirection = Vector3.Lerp(transform.forward, directionToWaypoint, steeringSmoothness);
        
        // Поворот в сторону будущего направления
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(futureDirection.x, 0, futureDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        
        // Движение вперед
        transform.position += transform.forward * speed * Time.deltaTime;
        
        // Проверка, достигли ли точки
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}