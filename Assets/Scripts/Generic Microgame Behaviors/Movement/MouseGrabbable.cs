﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MouseGrabbable : MonoBehaviour
{
	public bool centerOnCursor;
	public UnityEvent onGrab, onRelease;

	private Collider2D _colldider2D;
	private Vector2 grabOffset;
	private bool grabbed;

	void Awake ()
	{
		_colldider2D = GetComponent<Collider2D>();
		grabbed = false;
	}
	
	void Update ()
	{
		if (_colldider2D == null)
			return;

		updateStatus();

		if (grabbed)
			updateGrab();
	}

	void updateGrab()
	{
		Vector3 position = CameraHelper.getCursorPosition();
		if (!centerOnCursor)
			position += (Vector3)grabOffset;
		position.z = transform.position.z;
		transform.position = position;
	}

	void updateStatus()
	{
		if (!grabbed)
		{
			if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(_colldider2D))
			{
				grabbed = true;
				grabOffset = (Vector2)transform.position - (Vector2)CameraHelper.getCursorPosition();
				onGrab.Invoke();
			}
		}
		else
		{
			if (!Input.GetMouseButton(0))
			{
				grabbed = false;
				onRelease.Invoke();
			}
		}
		
	}
}
