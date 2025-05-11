/*
MESSAGE FROM CREATOR: This script was coded by Mena. You can use it in your games either these are commercial or
personal projects. You can even add or remove functions as you wish. However, you cannot sell copies of this
script by itself, since it is originally distributed as a free product.
I wish you the best for your project. Good luck!

P.S: If you need more cars, you can check my other vehicle assets on the Unity Asset Store, perhaps you could find
something useful for your game. Best regards, Mena.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PrometeoCarController : MonoBehaviour
{
  [Space(10)]
  [Header("Drift Scoring")]
  public bool useDriftScoring = false;
  private DriftScoreUI driftScoreUI; // Ссылка на скрипт UI
  public float driftScore = 0f;
  public float scorePerSecond = 100f;
  public float speedMultiplier = 0.1f; // Множитель от скорости
  private bool wasDrifting = false; // Для отслеживания изменения состояния дрифта
  private float driftStartTime; // Время начала
  private SaveLoadManager _saveLoadManager;
  [Space(20)]
  private CinemachineVirtualCamera cinemachineCamera; // Ваша Cinemachine камера
  private float minOrthoSize = 8f;  // Минимальное значение размера
  private float maxOrthoSize = 15f; // Максимальное значение размера
  [SerializeField] private float zoomSpeed = 1f;     // Скорость изменения размера
  private float targetOrthoSize;

  //CAR SETUP
  [Space(20)]
  [Range(20, 190)]
  public int maxSpeed = 90; //The maximum speed that the car can reach in km/h.
  [Range(10, 120)]
  public int maxReverseSpeed = 45; //The maximum speed that the car can reach while going on reverse in km/h.
  [Range(1, 10)]
  public int accelerationMultiplier = 2; // How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.
  [Space(10)]
  [Range(10, 45)]
  public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
  [Range(0.1f, 1f)]
  public float steeringSpeed = 0.5f; // How fast the steering wheel turns.
  [Space(10)]
  [Range(100, 600)]
  public int brakeForce = 350; // The strength of the wheel brakes.
  [Range(1, 10)]
  public int decelerationMultiplier = 2; // How fast the car decelerates when the user is not using the throttle.
  [Range(1, 10)]
  public int handbrakeDriftMultiplier = 5; // How much grip the car loses when the user hit the handbrake.
  [Space(10)]
  public Vector3 bodyMassCenter; // This is a vector that contains the center of mass of the car.

  //WHEELS
  public GameObject frontLeftMesh;
  public WheelCollider frontLeftCollider;
  [Space(10)]
  public GameObject frontRightMesh;
  public WheelCollider frontRightCollider;
  [Space(10)]
  public GameObject rearLeftMesh;
  public WheelCollider rearLeftCollider;
  [Space(10)]
  public GameObject rearRightMesh;
  public WheelCollider rearRightCollider;

  //PARTICLE SYSTEMS
  [Space(20)]
  public bool useEffects = false;
  public ParticleSystem RLWParticleSystem;
  public ParticleSystem RRWParticleSystem;
  [Space(10)]
  public TrailRenderer RLWTireSkid;
  public TrailRenderer RRWTireSkid;

  //SPEED TEXT (UI)
  [Space(20)]
  public bool useUI = false;
  public Text carSpeedText; // Used to store the UI object that is going to show the speed of the car.

  //SOUNDS
  [Space(20)]
  public bool useSounds = false;
  public AudioSource carEngineSound; // This variable stores the sound of the car engine.
  public AudioSource tireScreechSound; // This variable stores the sound of the tire screech (when the car is drifting).
  float initialCarEngineSoundPitch; // Used to store the initial pitch of the car engine sound.

  //CONTROLS
  [Space(20)]
  public bool useTouchControls = false;
  public GameObject throttleButton;
  PrometeoTouchInput throttlePTI;
  public GameObject reverseButton;
  PrometeoTouchInput reversePTI;
  public GameObject turnRightButton;
  PrometeoTouchInput turnRightPTI;
  public GameObject turnLeftButton;
  PrometeoTouchInput turnLeftPTI;
  public GameObject handbrakeButton;
  PrometeoTouchInput handbrakePTI;

  //CAR DATA
  [HideInInspector]
  public float carSpeed; // Used to store the speed of the car.
  [HideInInspector]
  public bool isDrifting; // Used to know whether the car is drifting or not.
  [HideInInspector]
  public bool isTractionLocked; // Used to know whether the traction of the car is locked or not.

  //PRIVATE VARIABLES
  [SerializeField] private CinemachineVirtualCamera virtualCamera;
  Rigidbody carRigidbody; // Stores the car's rigidbody.
  float steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
  float throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
  float driftingAxis;
  float localVelocityZ;
  float localVelocityX;
  bool deceleratingCar;
  bool touchControlsSetup = false;
  WheelFrictionCurve FLwheelFriction;
  float FLWextremumSlip;
  WheelFrictionCurve FRwheelFriction;
  float FRWextremumSlip;
  WheelFrictionCurve RLwheelFriction;
  float RLWextremumSlip;
  WheelFrictionCurve RRwheelFriction;
  float RRWextremumSlip;

  [Inject]
  private void Construct(SaveLoadManager saveLoadManager)
  {
    _saveLoadManager = saveLoadManager;
  }
  
  void Start()
  {
    if (Application.isMobilePlatform == false)
    {
      useTouchControls = false;
    }
    else
    {
      useTouchControls = true;
    }

    cinemachineCamera = GameObject.Find("CarCamera").GetComponent<CinemachineVirtualCamera>();
    carRigidbody = gameObject.GetComponent<Rigidbody>();
    carRigidbody.centerOfMass = bodyMassCenter;

    // Находим компонент DriftScoreUI в сцене
    driftScoreUI = FindObjectOfType<DriftScoreUI>();
    Debug.Log(driftScoreUI);
    FLwheelFriction = new WheelFrictionCurve();
    FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
    FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
    FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
    FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
    FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
    FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;

    FRwheelFriction = new WheelFrictionCurve();
    FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
    FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
    FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
    FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
    FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
    FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;

    RLwheelFriction = new WheelFrictionCurve();
    RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
    RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
    RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
    RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
    RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
    RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;

    RRwheelFriction = new WheelFrictionCurve();
    RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
    RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
    RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
    RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
    RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
    RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;

    if (carEngineSound != null)
    {
      initialCarEngineSoundPitch = carEngineSound.pitch;
    }

    if (useUI)
    {
      InvokeRepeating("CarSpeedUI", 0f, 0.1f);
    }
    else if (!useUI)
    {
      if (carSpeedText != null)
      {
        carSpeedText.text = "0";
      }
    }

    if (useSounds)
    {
      InvokeRepeating("CarSounds", 0f, 0.1f);
    }
    else if (!useSounds)
    {
      if (carEngineSound != null)
      {
        carEngineSound.Stop();
      }
      if (tireScreechSound != null)
      {
        tireScreechSound.Stop();
      }
    }

    if (!useEffects)
    {
      if (RLWParticleSystem != null)
      {
        RLWParticleSystem.Stop();
      }
      if (RRWParticleSystem != null)
      {
        RRWParticleSystem.Stop();
      }
      if (RLWTireSkid != null)
      {
        RLWTireSkid.emitting = false;
      }
      if (RRWTireSkid != null)
      {
        RRWTireSkid.emitting = false;
      }
    }

    if (useTouchControls)
    {
      if (throttleButton != null && reverseButton != null &&
          turnRightButton != null && turnLeftButton != null
          && handbrakeButton != null)
      {
        throttlePTI = throttleButton.GetComponent<PrometeoTouchInput>();
        reversePTI = reverseButton.GetComponent<PrometeoTouchInput>();
        turnLeftPTI = turnLeftButton.GetComponent<PrometeoTouchInput>();
        turnRightPTI = turnRightButton.GetComponent<PrometeoTouchInput>();
        handbrakePTI = handbrakeButton.GetComponent<PrometeoTouchInput>();
        touchControlsSetup = true;
      }
      else
      {
        String ex = "Touch controls are not completely set up. You must drag and drop your scene buttons in the" +
        " PrometeoCarController component.";
        Debug.LogWarning(ex);
      }
    }

    // Инициализация текста очков дрифта
    if (driftScoreUI != null)
    {
      driftScoreUI.UpdateScore(driftScore + 10);
    }
    else
    {
      Debug.LogWarning("DriftScoreUI not found in the scene!");
    }
  }

  void Update()
  {
   
      if (isDrifting)
      {
        if (!wasDrifting)
        {
          // Дрифт только начался
          wasDrifting = true;
          driftStartTime = Time.time;
        }
        // Рассчитываем время дрифта
        float driftDuration = Time.time - driftStartTime;

        // Начисляем очки с учетом скорости
        float speedFactor = 1 + Mathf.Abs(carSpeed) * speedMultiplier;
        driftScore += scorePerSecond * speedFactor * Time.deltaTime;
        driftScoreUI.UpdateScore(driftScore);

        if (_saveLoadManager.GetScoreDrift() > (Mathf.RoundToInt(driftScore)))
        {
          _saveLoadManager.SetScoreDrift(Mathf.RoundToInt(driftScore));
        }

      }
      else if (wasDrifting)
      {
        // Дрифт закончился
        wasDrifting = false;
      }
      

    carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
    localVelocityX = transform.InverseTransformDirection(carRigidbody.velocity).x;
    localVelocityZ = transform.InverseTransformDirection(carRigidbody.velocity).z;

    if (carSpeed > 65)
    {
      targetOrthoSize = Mathf.Lerp(minOrthoSize, maxOrthoSize, carSpeed / maxSpeed);
      float currentOrthoSize = cinemachineCamera.m_Lens.OrthographicSize;
      cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentOrthoSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
    }
    else
    {
      float currentOrthoSize = cinemachineCamera.m_Lens.OrthographicSize;
      cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentOrthoSize, 10, Time.deltaTime * zoomSpeed);
    }

    if (useTouchControls && touchControlsSetup)
    {
      if (throttlePTI.buttonPressed)
      {
        CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        GoForward();
      }
      if (reversePTI.buttonPressed)
      {
        CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        GoReverse();
      }
      if (turnLeftPTI.buttonPressed)
      {
        TurnLeft();
      }
      if (turnRightPTI.buttonPressed)
      {
        TurnRight();
      }
      if (handbrakePTI.buttonPressed)
      {
        CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        Handbrake();
      }
      if (!handbrakePTI.buttonPressed)
      {
        RecoverTraction();
      }
      if (!throttlePTI.buttonPressed && !reversePTI.buttonPressed)
      {
        ThrottleOff();
      }
      if (!reversePTI.buttonPressed && !throttlePTI.buttonPressed && !handbrakePTI.buttonPressed && !deceleratingCar)
      {
        InvokeRepeating("DecelerateCar", 0f, 0.1f);
        deceleratingCar = true;
      }
      if (!turnLeftPTI.buttonPressed && !turnRightPTI.buttonPressed && steeringAxis != 0f)
      {
        ResetSteeringAngle();
      }
    }
    else
    {
      if (Input.GetKey(KeyCode.W))
      {
        CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        GoForward();
      }
      if (Input.GetKey(KeyCode.S))
      {
        CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        GoReverse();
      }
      if (Input.GetKey(KeyCode.A))
      {
        TurnLeft();
      }
      if (Input.GetKey(KeyCode.D))
      {
        TurnRight();
      }
      if (Input.GetKey(KeyCode.Space))
      {
        CancelInvoke("DecelerateCar");
        deceleratingCar = false;
        Handbrake();
      }
      if (Input.GetKeyUp(KeyCode.Space))
      {
        RecoverTraction();
      }
      if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
      {
        ThrottleOff();
      }
      if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Space) && !deceleratingCar)
      {
        InvokeRepeating("DecelerateCar", 0f, 0.1f);
        deceleratingCar = true;
      }
      if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && steeringAxis != 0f)
      {
        ResetSteeringAngle();
      }
    }

    AnimateWheelMeshes();
  }

  // Метод для сброса очков дрифта
  public void ResetDriftScore()
  {
    driftScore = 0f;
    wasDrifting = false;
    if (driftScoreUI != null)
    {
      driftScoreUI.UpdateScore(driftScore);
    }
  }

  public void CarSpeedUI()
  {
    if (useUI)
    {
      try
      {
        float absoluteCarSpeed = Mathf.Abs(carSpeed);
        carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
      }
      catch (Exception ex)
      {
        Debug.LogWarning(ex);
      }
    }
  }

  public void CarSounds()
  {
    if (useSounds)
    {
      try
      {
        if (carEngineSound != null)
        {
          float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.velocity.magnitude) / 25f);
          carEngineSound.pitch = engineSoundPitch;
        }
        if ((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f))
        {
          if (!tireScreechSound.isPlaying)
          {
            tireScreechSound.Play();
          }
        }
        else if (!isDrifting && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f))
        {
          tireScreechSound.Stop();
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarning(ex);
      }
    }
    else if (!useSounds)
    {
      if (carEngineSound != null && carEngineSound.isPlaying)
      {
        carEngineSound.Stop();
      }
      if (tireScreechSound != null && tireScreechSound.isPlaying)
      {
        tireScreechSound.Stop();
      }
    }
  }

  public void TurnLeft()
  {
    steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
    if (steeringAxis < -1f)
    {
      steeringAxis = -1f;
    }
    var steeringAngle = steeringAxis * maxSteeringAngle;
    frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
    frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
  }

  public void TurnRight()
  {
    steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
    if (steeringAxis > 1f)
    {
      steeringAxis = 1f;
    }
    var steeringAngle = steeringAxis * maxSteeringAngle;
    frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
    frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
  }

  public void ResetSteeringAngle()
  {
    if (steeringAxis < 0f)
    {
      steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
    }
    else if (steeringAxis > 0f)
    {
      steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
    }
    if (Mathf.Abs(frontLeftCollider.steerAngle) < 1f)
    {
      steeringAxis = 0f;
    }
    var steeringAngle = steeringAxis * maxSteeringAngle;
    frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
    frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
  }

  void AnimateWheelMeshes()
  {
    try
    {
      Quaternion FLWRotation;
      Vector3 FLWPosition;
      frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
      frontLeftMesh.transform.position = FLWPosition;
      frontLeftMesh.transform.rotation = FLWRotation;

      Quaternion FRWRotation;
      Vector3 FRWPosition;
      frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
      frontRightMesh.transform.position = FRWPosition;
      frontRightMesh.transform.rotation = FRWRotation;

      Quaternion RLWRotation;
      Vector3 RLWPosition;
      rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
      rearLeftMesh.transform.position = RLWPosition;
      rearLeftMesh.transform.rotation = RLWRotation;

      Quaternion RRWRotation;
      Vector3 RRWPosition;
      rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
      rearRightMesh.transform.position = RRWPosition;
      rearRightMesh.transform.rotation = RRWRotation;
    }
    catch (Exception ex)
    {
      Debug.LogWarning(ex);
    }
  }

  public void GoForward()
  {
    if (Mathf.Abs(localVelocityX) > 2.5f)
    {
      isDrifting = true;
      DriftCarPS();
    }
    else
    {
      isDrifting = false;
      DriftCarPS();
    }
    throttleAxis = throttleAxis + (Time.deltaTime * 3f);
    if (throttleAxis > 1f)
    {
      throttleAxis = 1f;
    }
    if (localVelocityZ < -1f)
    {
      Brakes();
    }
    else
    {
      if (Mathf.RoundToInt(carSpeed) < maxSpeed)
      {
        frontLeftCollider.brakeTorque = 0;
        frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        frontRightCollider.brakeTorque = 0;
        frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        rearLeftCollider.brakeTorque = 0;
        rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        rearRightCollider.brakeTorque = 0;
        rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
      }
      else
      {
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
      }
    }
  }

  public void GoReverse()
  {
    if (Mathf.Abs(localVelocityX) > 2.5f)
    {
      isDrifting = true;
      DriftCarPS();
    }
    else
    {
      isDrifting = false;
      DriftCarPS();
    }
    throttleAxis = throttleAxis - (Time.deltaTime * 3f);
    if (throttleAxis < -1f)
    {
      throttleAxis = -1f;
    }
    if (localVelocityZ > 1f)
    {
      Brakes();
    }
    else
    {
      if (Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxReverseSpeed)
      {
        frontLeftCollider.brakeTorque = 0;
        frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        frontRightCollider.brakeTorque = 0;
        frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        rearLeftCollider.brakeTorque = 0;
        rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        rearRightCollider.brakeTorque = 0;
        rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
      }
      else
      {
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
      }
    }
  }

  public void ThrottleOff()
  {
    frontLeftCollider.motorTorque = 0;
    frontRightCollider.motorTorque = 0;
    rearLeftCollider.motorTorque = 0;
    rearRightCollider.motorTorque = 0;
  }

  public void DecelerateCar()
  {
    if (Mathf.Abs(localVelocityX) > 2.5f)
    {
      isDrifting = true;
      DriftCarPS();
    }
    else
    {
      isDrifting = false;
      DriftCarPS();
    }
    if (throttleAxis != 0f)
    {
      if (throttleAxis > 0f)
      {
        throttleAxis = throttleAxis - (Time.deltaTime * 10f);
      }
      else if (throttleAxis < 0f)
      {
        throttleAxis = throttleAxis + (Time.deltaTime * 10f);
      }
      if (Mathf.Abs(throttleAxis) < 0.15f)
      {
        throttleAxis = 0f;
      }
    }
    carRigidbody.velocity = carRigidbody.velocity * (1f / (1f + (0.025f * decelerationMultiplier)));
    frontLeftCollider.motorTorque = 0;
    frontRightCollider.motorTorque = 0;
    rearLeftCollider.motorTorque = 0;
    rearRightCollider.motorTorque = 0;
    if (carRigidbody.velocity.magnitude < 0.25f)
    {
      carRigidbody.velocity = Vector3.zero;
      CancelInvoke("DecelerateCar");
    }
  }

  public void Brakes()
  {
    frontLeftCollider.brakeTorque = brakeForce;
    frontRightCollider.brakeTorque = brakeForce;
    rearLeftCollider.brakeTorque = brakeForce;
    rearRightCollider.brakeTorque = brakeForce;
  }

  public void Handbrake()
  {
    CancelInvoke("RecoverTraction");
    driftingAxis = driftingAxis + (Time.deltaTime);
    float secureStartingPoint = driftingAxis * FLWextremumSlip * handbrakeDriftMultiplier;

    if (secureStartingPoint < FLWextremumSlip)
    {
      driftingAxis = FLWextremumSlip / (FLWextremumSlip * handbrakeDriftMultiplier);
    }
    if (driftingAxis > 1f)
    {
      driftingAxis = 1f;
    }
    if (Mathf.Abs(localVelocityX) > 2.5f)
    {
      isDrifting = true;
    }
    else
    {
      isDrifting = false;
    }
    if (driftingAxis < 1f)
    {
      FLwheelFriction.extremumSlip = FLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      frontLeftCollider.sidewaysFriction = FLwheelFriction;

      FRwheelFriction.extremumSlip = FRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      frontRightCollider.sidewaysFriction = FRwheelFriction;

      RLwheelFriction.extremumSlip = RLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      rearLeftCollider.sidewaysFriction = RLwheelFriction;

      RRwheelFriction.extremumSlip = RRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      rearRightCollider.sidewaysFriction = RRwheelFriction;
    }

    isTractionLocked = true;
    DriftCarPS();
  }

  public void DriftCarPS()
  {
    if (useEffects)
    {
      try
      {
        if (isDrifting)
        {
          RLWParticleSystem.Play();
          RRWParticleSystem.Play();
        }
        else if (!isDrifting)
        {
          RLWParticleSystem.Stop();
          RRWParticleSystem.Stop();
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarning(ex);
      }

      try
      {
        if ((isTractionLocked || Mathf.Abs(localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f)
        {
          RLWTireSkid.emitting = true;
          RRWTireSkid.emitting = true;
        }
        else
        {
          RLWTireSkid.emitting = false;
          RRWTireSkid.emitting = false;
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarning(ex);
      }
    }
    else if (!useEffects)
    {
      if (RLWParticleSystem != null)
      {
        RLWParticleSystem.Stop();
      }
      if (RRWParticleSystem != null)
      {
        RRWParticleSystem.Stop();
      }
      if (RLWTireSkid != null)
      {
        RLWTireSkid.emitting = false;
      }
      if (RRWTireSkid != null)
      {
        RRWTireSkid.emitting = false;
      }
    }
  }

  public void RecoverTraction()
  {
    isTractionLocked = false;
    driftingAxis = driftingAxis - (Time.deltaTime / 1.5f);
    if (driftingAxis < 0f)
    {
      driftingAxis = 0f;
    }

    if (FLwheelFriction.extremumSlip > FLWextremumSlip)
    {
      FLwheelFriction.extremumSlip = FLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      frontLeftCollider.sidewaysFriction = FLwheelFriction;

      FRwheelFriction.extremumSlip = FRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      frontRightCollider.sidewaysFriction = FRwheelFriction;

      RLwheelFriction.extremumSlip = RLWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      rearLeftCollider.sidewaysFriction = RLwheelFriction;

      RRwheelFriction.extremumSlip = RRWextremumSlip * handbrakeDriftMultiplier * driftingAxis;
      rearRightCollider.sidewaysFriction = RRwheelFriction;

      Invoke("RecoverTraction", Time.deltaTime);
    }
    else if (FLwheelFriction.extremumSlip < FLWextremumSlip)
    {
      FLwheelFriction.extremumSlip = FLWextremumSlip;
      frontLeftCollider.sidewaysFriction = FLwheelFriction;

      FRwheelFriction.extremumSlip = FRWextremumSlip;
      frontRightCollider.sidewaysFriction = FRwheelFriction;

      RLwheelFriction.extremumSlip = RLWextremumSlip;
      rearLeftCollider.sidewaysFriction = RLwheelFriction;

      RRwheelFriction.extremumSlip = RRWextremumSlip;
      rearRightCollider.sidewaysFriction = RRwheelFriction;

      driftingAxis = 0f;
    }
  }
}