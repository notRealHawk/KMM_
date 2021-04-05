using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class MultiTargetCam : MonoBehaviour {
	public List<Transform> targets;
	public float smoothTime=.5f;
	public Text debug;
	public float MinZoom=40f;
	public float MaxZoom = 10f;
	public float ZoomLimiter = 50f;
	public Vector3 offset;
	Camera cam;
	Vector3 velocity;
	void Start(){
		cam = GetComponent<Camera> ();
	}
	void LateUpdate(){
//		targets = GameSetup.GS.PlayersTransform;
		if (targets.Count == 0) {
			cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, MinZoom, Time.deltaTime); 
			return;
		}

		Move ();
		Zoom ();

	}
	void Zoom(){
		
		float newZoom = Mathf.Lerp (MaxZoom, MinZoom, GetGreatestDistance () / ZoomLimiter);
		cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, newZoom, Time.deltaTime); 	
	}
	Bounds Bounds ;
	float GetGreatestDistance(){
		if (targets [Reference] != null) {
			Bounds = new Bounds (targets [Reference].position, Vector3.zero);
		} 
		for (int i = 0; i < targets.Count; i++) {
			if (targets [i] != null) {
				Bounds.Encapsulate (targets [i].transform.position);
			}
		}
//		Debug.Log ("Size=" + bounds.size.z);
		return Bounds.size.x;
	}
	void Move(){
		Vector3 CenterPoint = GetCenterPoints ();
		Vector3 newPosition = CenterPoint + offset;
		transform.position = Vector3.SmoothDamp (transform.position, newPosition, ref velocity, smoothTime);
	}
	Bounds bounds ;
	public	int Reference=0;
	Vector3 GetCenterPoints(){
		if (targets.Count == 1 && targets [Reference] != null) {
			return targets [Reference].position;
		} 
		if (targets [Reference] != null) {
			bounds = new Bounds (targets [Reference].position, Vector3.zero);
		} else {
			if (Reference < targets.Count) {
				Reference++;
			} else {
				Reference--;
			}
		}
		for (int i = 0; i < targets.Count; i++) {
			if (targets [i] != null) {
				bounds.Encapsulate (targets [i].position);
			}
		}
		return bounds.center;
	}
}
