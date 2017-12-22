using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 offset;

	void Update () {
        transform.position = target.transform.position + offset;
	}
}
