using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float kilometersPerHour;
    [Tooltip("Meters")]
    public float wheelCircumference;

    public List<TrainCar> trainCars;

    [System.Serializable]
    public class TrainCar
    {
        public Transform trainBody;
        public List<Transform> wheels;

        public void RotateWheels(float degrees)
        {
            foreach (Transform wheel in wheels)
            {
                wheel.Rotate(-trainBody.up, degrees);
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
        RotateWheels();
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
