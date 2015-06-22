using UnityEngine;
using System.Collections;

public class GravityFieldHelper: MonoBehaviour  {

	public SolarSystem sol;

	public Vector3[,] CalculatePoints() {
		
		
		var cam = Camera.main;
		Vector3 p1 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		Vector3 p2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		Vector3 p3 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		float width = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;
		
		int density = 500;
		float delta = width / density;
		
		Vector3[,] points = new Vector3 [density, density];
		float xpos = 0;
		float ypos = 0;
		float currentForce = 0;
		for (int x=0; x<density; x++) {
			
			for(int y=0;y<density;y++){
				
				xpos = (float)x*delta - width/2;
				ypos = (float)y*delta - height/2;
				
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
