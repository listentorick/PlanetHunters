using UnityEngine;
using System.Collections;

public class GravityFieldHelper: MonoBehaviour  {

	public SolarSystem sol;
	public delegate void Ready(Vector3[,] data);


	public void CalculatePoints(int xdensity, int ydensity, Ready ready) {
		StartCoroutine (CalculatePointsAsync (xdensity, ydensity, ready));
	}
	
	private IEnumerator CalculatePointsAsync(int xdensity, int ydensity, Ready ready) {

		var cam = Camera.main;
		Vector3 p1 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		Vector3 p2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		Vector3 p3 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		float width = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;
		
		//int density = 1000;
		float xdelta = width / xdensity;
		float ydelta = height / ydensity;

		Vector3[,] points = new Vector3 [xdensity, ydensity];
		float xpos = 0;
		float ypos = 0;
		float currentForce = 0;
		for (int x=0; x<xdensity; x++) {
			
			for(int y=0;y<ydensity;y++){
				
				xpos = (float)x*xdelta - width/2;
				ypos = (float)y*ydelta - height/2;
				
				Vector2 solPos = new Vector2(xpos * GameController.SCALE,ypos * GameController.SCALE);

				currentForce = sol.CalculateForceAtPoint(solPos).magnitude;

				points[x,y] = new Vector3(xpos,ypos,currentForce);


			}
			//Debug.Log(x );
			yield return null;
		}
		ready (points);
		//return points;
		
	}
}
