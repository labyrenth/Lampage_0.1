using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2DObject : MonoBehaviour {

    public enum RotateAxis
    {
        X,
        Y,
        Z
    }

    private float iconRotationValue;
    [Range(0, 360)]
    public int rotateAnglePerFrame;
    public RotateAxis rotateAxis;
    private Vector3 iconRotationVector;
    private void OnEnable()
    {
        iconRotationValue = Mathf.Round(rotateAnglePerFrame * GameTime.FrameRate_60_Time ) * 0.01f;
        if (rotateAxis.Equals(RotateAxis.X))
        {
            iconRotationVector = Vector3.right * iconRotationValue;
        }
        else if (rotateAxis.Equals(RotateAxis.Y))
        {
            iconRotationVector = Vector3.up * iconRotationValue;
        }
        else if(rotateAxis.Equals(RotateAxis.Z))
        {
            iconRotationVector = Vector3.forward * iconRotationValue;
        }
    }

    private void TargetIconRotate()
    {
        this.transform.localRotation *= Quaternion.Euler(iconRotationVector);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        TargetIconRotate();
	}
}
