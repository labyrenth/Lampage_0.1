using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerToCamera : MonoBehaviour {

    public GameObject target;
    public Transform pivot;
    public Transform markerParents;

    private void Start()
    {
        this.markerParents = this.transform.parent.transform;
    }

    private void LookTarget()
    {
        Vector3 temp = pivot.transform.position - target.transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(temp.y, temp.x);
        this.markerParents.rotation = pivot.rotation * Quaternion.Euler(0, 0, angle);
    }
	
	// Update is called once per frame
	void Update () {
        LookTarget();	
	}
}
