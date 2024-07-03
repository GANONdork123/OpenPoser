using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
	public GameObject selectedObject;
	public GameObject selectedGizmo;
	
	public GameObject cameraSystem;
	public Camera cam;
	
	public Vector2 touchPosition;
	public Vector2 touchDelta;
	public bool dragging;
	public bool tapping;
	
	public GameObject Gizmo3D;
	
	public void OnPoint(InputAction.CallbackContext context)
	{
		touchPosition = context.ReadValue<Vector2>();
	}
	
	public void OnMove(InputAction.CallbackContext context)
	{
		touchDelta = context.ReadValue<Vector2>();
	}
	
	public void OnTap (InputAction.CallbackContext context)
	{
		tapping = context.performed;
		
		if (context.performed)
		{
			RaycastHit[] hits;
			Ray ray = cam.ScreenPointToRay(touchPosition);
			LayerMask mask = LayerMask.GetMask("UI");
			hits = Physics.RaycastAll(ray.origin, ray.direction, 200f, mask);
			Debug.DrawRay(ray.origin, ray.direction * 200, Color.yellow);
			
			bool hitController = false;
			bool hitGizmo = false;
			
			for (int i = 0; i < hits.Length; i++)
			{
				RaycastHit hit = hits[i];
				Debug.Log("Raycast hit " + hit.transform.gameObject.name);
				
				if (!hitController && !hitGizmo && hit.transform.gameObject.tag != "Controller")
				{
					selectedObject = null;
					selectedGizmo = null;
				}
				
				if (hit.transform.gameObject.tag == "Controller" && hit.transform.parent != Gizmo3D.transform  && !hitController)
				{
					selectedObject = hit.transform.parent.gameObject;
					hitController = true;
				}
				
				if (hit.transform.parent.gameObject == Gizmo3D && !hitGizmo)
				{
					selectedGizmo = hit.transform.gameObject;
					hitGizmo = true;
				}
			}
			
			if (hits.Length == 0)
			{
				selectedObject = null;
				selectedGizmo = null;
			}
		}
	}
	
	public void OnDrag(InputAction.CallbackContext context)
	{
		dragging = context.performed;
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		Ray ray = cam.ScreenPointToRay(touchPosition);
		Debug.DrawRay(ray.origin, ray.direction * 200, Color.yellow);
		
		if (dragging && selectedGizmo == null)
		{	
			cameraSystem.transform.Rotate(0,touchDelta.x * Time.deltaTime * 25f, 0f);
			cameraSystem.transform.GetChild(0).Rotate (-touchDelta.y * Time.deltaTime * 25f, 0f, 0f, Space.Self);	
		}
		
		MeshRenderer[] Gizmos = Gizmo3D.GetComponentsInChildren<MeshRenderer>();
		
		foreach (MeshRenderer gizmo in Gizmos)
		{
			gizmo.material.SetInt("_Highlighted", 0);
		}
		
		if (selectedGizmo != null && selectedObject != null)
		{
			selectedGizmo.GetComponent<MeshRenderer>().material.SetInt("_Highlighted", 1);
			
			if (dragging)
			{
				switch (selectedGizmo.name)
				{
				case "RotatorX":
					break;
				case "RotatorY":
					break;
				case "RotatorZ":
					break;
				case "Scaler":
					break;
				case "TranslatorX":
					selectedObject.transform.position += Gizmo3D.transform.right * (-touchDelta.x + touchDelta.y) * 0.01f;
					break;
				case "TranslatorY":
					selectedObject.transform.position += Gizmo3D.transform.up * (touchDelta.x + touchDelta.y) * 0.01f;
					break;
				case "TranslatorZ":
					selectedObject.transform.position += Gizmo3D.transform.forward * (touchDelta.x + touchDelta.y) * 0.01f;
					break;
				
				
				}
			}
		}
		
		if (selectedObject != null)
		{
			Gizmo3D.transform.position = selectedObject.transform.position;
			float gizmoDistance = Vector3.Distance(Gizmo3D.transform.position, cam.transform.position) * 0.1f;
			Gizmo3D.transform.localScale = new Vector3 (gizmoDistance, gizmoDistance, gizmoDistance);
			Gizmo3D.SetActive(true);
		}
		else
		{
			Gizmo3D.SetActive(false);
		}
		
	}
}
