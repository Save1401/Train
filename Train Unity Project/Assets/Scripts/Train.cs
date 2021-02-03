using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Train : MonoBehaviour
{
    Vector3 movementInput;
    public PlayerInput input;
    public float accelerationSpeed = 0.5f;
    public float maxSpeed = 10;
    public float drag = 0.25f;
    public float turnSpeed = 10;
    public float turnTilt = -5;
    //public float tiltSpeed;
    public float kilometersPerHour;
    [Tooltip("Meters")]
    public float wheelCircumference;

    public List<TrainCar> trainCars;

    public Transform cam;

    [System.Serializable]
    public class TrainCar
    {
        public Transform trainBody;
        public List<Transform> wheels;

        public void RotateWheels(float degrees)
        {
            foreach (Transform wheel in wheels)
            {
                wheel.Rotate(Vector3.up, degrees);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveTrain();
        RotateWheels();

        cam.transform.position = transform.position + transform.up * 5 + transform.forward * -10;
        cam.LookAt(transform);
    }
    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        //Debug.Log(movementInput);
    }
    float targetTilt = 0;
    void MoveTrain()
    {
        kilometersPerHour = kilometersPerHour < maxSpeed || movementInput.y - drag <= 0 ? kilometersPerHour + (movementInput.y - (kilometersPerHour > 0 ? drag : kilometersPerHour < 0 ? -drag : 0)) * accelerationSpeed * Time.deltaTime : maxSpeed;
        if (kilometersPerHour < 0)
            kilometersPerHour = 0;
        transform.position += transform.forward * (kilometersPerHour * 3.6f) * Time.deltaTime;
        if (kilometersPerHour != 0)
        {
            transform.Rotate(Vector3.up, movementInput.x * kilometersPerHour * turnSpeed * Time.deltaTime);
            targetTilt = turnTilt * movementInput.x * (kilometersPerHour * (kilometersPerHour < 0 ? -1 : 1));
            trainCars[0].trainBody.localEulerAngles = new Vector3(targetTilt, -90, 0);
        }
    }
    /// <summary>
    /// Makes the wheels revolve
    /// </summary>
    void RotateWheels()
    {
        float kilometersPerSecond = kilometersPerHour / 3600;
        float metersPerSecond = kilometersPerSecond * 1000;
        float rotationsPerSecond = metersPerSecond / wheelCircumference;
        float degrees = 360 * rotationsPerSecond * Time.deltaTime;

        foreach(TrainCar trainCar in trainCars)
        {
            trainCar.RotateWheels(degrees);
        }
    }
}
