using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using HiFi;

public class TankSync : RealtimeComponent
{
    public TankController tankController;
    private TankSyncModel _model;

    private TankSyncModel model
    {
        set
        {
            /// Store the model
            _model = value;
        }
    }

    private void Awake()
    {
        if (tankController == null)
            tankController = GetComponent<TankController>();
    }

    private void Update()
    {
        tankController.leftSpeedNetworkReturn = _model.trackLeftSpeed;
        tankController.rightSpeedNetworkReturn = _model.trackRightSpeed;
        tankController.gunAngleNetworkReturn = _model.gunAngle;
    }


    /// ==== PUBLIC =====

    public void SetSpeeds(Vector2 speedInput)
    {
        _model.trackLeftSpeed = speedInput.x;
        _model.trackRightSpeed = speedInput.y;
    }

    public void SetGunAngle(float angleInput)
    {
        _model.gunAngle = angleInput;
    }
}

