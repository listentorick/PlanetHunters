using UnityEngine;
using System.Collections;

public class GravityFieldHelper: MonoBehaviour  {

	public SolarSystem sol;

	public Vector3[,] CalculatePoints(int xdensity, int ydensity) {
		
		
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
				
				Vector2 solPos = new Vector2(xpos * 100000f,ypos * 100000f);
				//if(!sol.IsInAnySOI(solPos)){
				currentForce = sol.CalculateForceAtPoint(solPos).magnitude;
				points[x,y] = new Vector3(xpos,ypos,currentForce);
				
				//if(currentForce<minForce) {
				//	minForce = currentForce;
				//}
				//if(currentForce>maxForce){
				///	maxForce = currentForce;
			//	}
				//} else {
				//	points[x,y] = new Vector3(xpos,ypos,lastForce);
				//}
				
			}
		}
		
		return points;
		
	}
}
