using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MegeXR.Core;
using XTC.MegeXR.Decorator;
using XTC.MegeXR.SDK;

public class XRProxy : MonoBehaviour
{
	public enum VRMode
    {
        OFF,
        ON
    }

	public VRMode modeVR = VRMode.OFF;

	public Transform originalCanvas2D;
	public Camera originalCamera;
	public Transform originalEventSystem;

	public Transform reticle;
	public Transform HUD;
	public Transform canvasXR;
	public Transform eventSystemXR;

	private IXR xr {get;set;}

	private Dictionary<int, CameraClearFlags> cameraClearFlags = new Dictionary<int, CameraClearFlags>();


	public void DoAwake()
	{
		if(modeVR == VRMode.OFF)
			return;
		
		Color cameraColor = originalCamera.backgroundColor;
		
		HUD.SetParent(canvasXR);
		RectTransform rtHUD = HUD.GetComponent<RectTransform>();
		rtHUD.sizeDelta = Vector2.zero;
		rtHUD.localPosition = Vector3.zero;
		rtHUD.localScale = Vector3.one;
		GameObject.Destroy(originalCanvas2D.gameObject);
		GameObject.Destroy(originalCamera.gameObject);
		GameObject.Destroy(originalEventSystem.gameObject);
		reticle.gameObject.SetActive(true);
		canvasXR.gameObject.SetActive(true);
		eventSystemXR.gameObject.SetActive(true);

		xr = new DummyXR();
		Engine.InjectXR(xr);
		Engine.InjectReticle(reticle);
		Engine.InjectCanvas3D(canvasXR);
		Engine.Preload();
		Engine.Initialize();

		eventSystemXR.gameObject.AddComponent<PointerInputDecorator>();
		ReticlePointerDecorator reticlPointer = reticle.gameObject.AddComponent<ReticlePointerDecorator>();
		xr.camera.gameObject.AddComponent<XPointerPhysicsRaycaster>();
		canvasXR.gameObject.AddComponent<XPointerGraphicRaycaster>();
		XPointerInputModule.Pointer.overridePointerCamera = xr.camera.GetComponent<Camera>();

		// switch camera use solid color
		cameraClearFlags.Clear();
		Camera[] xrCameras = xr.camera.GetComponentsInChildren<Camera>();
		foreach(Camera camera in xrCameras)
		{
			cameraClearFlags[camera.gameObject.GetInstanceID()] = camera.clearFlags;
			camera.backgroundColor = cameraColor;
			camera.clearFlags = CameraClearFlags.SolidColor;
		}
	}

	public void DoOnEnable()
	{
		if(modeVR == VRMode.OFF)
			return;
		Engine.Run();
	}

	public void DoStart()
	{
		if(modeVR == VRMode.OFF)
			return;
	}

	public void DoUpdate()
	{
		if(modeVR == VRMode.OFF)
			return;
		Engine.Update();
	}

	public void DoOnDisable()
	{
		if(modeVR == VRMode.OFF)
			return;
		Engine.Stop();
	}

	public void DoOnDestroy()
	{
		if(modeVR == VRMode.OFF)
			return;
		Engine.Release();
	}

	public void ResetCameraClearFlags()
	{
		// switch camera use solid color
		Camera[] xrCameras = xr.camera.GetComponentsInChildren<Camera>();
		foreach(Camera camera in xrCameras)
		{
			if(!cameraClearFlags.ContainsKey(camera.gameObject.GetInstanceID()))
				continue;
			camera.clearFlags = cameraClearFlags[camera.gameObject.GetInstanceID()];
		}
	}

}
