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
            // Store the model
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
        UpdateTankControllerSpeeds();    
    }

    private void UpdateTankControllerSpeeds()
    {
        tankController.leftSpeed = _model.trackLeftSpeed;
        tankController.rightSpeed = _model.trackRightSpeed;
    }

    /// ==== PUBLIC =====

    public void SetSpeeds(Vector2 speed)
    {
        _model.trackLeftSpeed = speed.x;
        _model.trackRightSpeed = speed.y;
    }
}

