﻿using UnityEngine;
using System.Collections;

public enum Cargo {

	Food, Medical, Technology
}

public class Ship : Body {

	public float cargo;
	public Cargo cargoType;
	private float rechargeTime = 0;
	private Transform shipRendererTransform;
	private bool isWrappingX;
	private bool isWrappingY;
	private Renderer[] renderers;

	public float fuel = 1f;
	public float burnRate = 0.01f;

	public void Start(){
		shipRendererTransform = this.gameObject.transform.GetChild (0);
		renderers = this.GetComponentsInChildren<Renderer> ();
	}


	public void Thrust(Vector2 thrust){
		if (fuel <= 0)
			return;
		additionalForce = thrust;
	}

	public void Update() {
	
		if (additionalForce != Vector2.zero && canMove) {
			fuel -= burnRate;
			thruster.Play();
			if(fuel<0){
				fuel = 0;
				rechargeTime =0;
			}
			
			var angle = Mathf.Atan2 (additionalForce.y, additionalForce.x) * Mathf.Rad2Deg;
			
			if (additionalForce.x < 0 && additionalForce.y > 0) {
				//topleft
				angle -= 90;
			}
			if (additionalForce.x > 0 && additionalForce.y > 0) {
				angle += 270;
			}
			if (additionalForce.x > 0 && additionalForce.y < 0) {
				angle -= 90;
			}
			if (additionalForce.x < 0 && additionalForce.y < 0) {
				angle += 270;
			}
			
			shipRendererTransform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			
		} else {
			
			if(thruster!=null)thruster.Stop();
			if(rechargeTime>5f){ 
				fuel +=  Time.deltaTime/10;
				if(fuel>1){
					fuel = 1;
				}
			}
			
		}

		base.Update ();
		

		ScreenWrap ();
		rechargeTime += Time.deltaTime;
	
	
	}

	private bool IsRendererVisible() {
		foreach (Renderer r in renderers) {
			if(r.isVisible==true){
				return true;
			}
		}
		return false;
	}

	void ScreenWrap()
	{
		
		
		if(IsRendererVisible())
		{
			isWrappingX = false;
			isWrappingY = false;
			return;
		}
		
		if(isWrappingX && isWrappingY) {
			return;
		}
		
		var cam = Camera.main;
		var viewportPosition = cam.WorldToViewportPoint(transform.position);
		var newPosition = transform.position;
		
		if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
		{
			newPosition.x = -newPosition.x;
			
			isWrappingX = true;
		}
		
		if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
		{
			newPosition.y = -newPosition.y;
			
			isWrappingY = true;
		}
		
		position = new Vector2(newPosition.x,newPosition.y) * scale;
		
		this.transform.position = newPosition;
	}
}
