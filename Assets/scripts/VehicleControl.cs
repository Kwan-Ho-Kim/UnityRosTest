using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControl : MonoBehaviour
{
    public WheelCollider RL_col;
    public WheelCollider RR_col;
    public WheelCollider FR_col;
    public WheelCollider FL_col;

    public float accel_power = 100;
    public float steer_power = 30;
    public float brake_power = 100;

    private float accel;
    private float steer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        accel = Input.GetAxis("Vertical") * accel_power;
        steer = Input.GetAxis("Horizontal") * steer_power;

        FL_col.motorTorque = accel;
        FR_col.motorTorque = accel;
        RL_col.motorTorque = accel;
        RR_col.motorTorque = accel;

        FR_col.steerAngle = steer;
        FL_col.steerAngle = steer;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FL_col.brakeTorque = brake_power;
            FR_col.brakeTorque = brake_power;
            RL_col.brakeTorque = brake_power;
            RR_col.brakeTorque = brake_power;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FL_col.brakeTorque = 0;
            FR_col.brakeTorque = 0;
            RL_col.brakeTorque = 0;
            RR_col.brakeTorque = 0;
        }
        
    }
}
