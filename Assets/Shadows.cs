using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Shadows : MonoBehaviour, IBuild {

	public Vector2 origin;
	public Body[] bodies;
	public float radius;
	
	// These represent the map and light location:        
	private List<EndPoint> endpoints = new List<EndPoint>();
	private List<Segment> segments = new List<Segment>();
	private EndPointComparer radialComparer  = new EndPointComparer();
	private MeshBuilder meshBuilder = new MeshBuilder();
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	List<Vector2> points = new List<Vector2> ();

	
	public void Update() {
		if(start)Build ();
	}
	bool start = false;
	public void Build(){
		start = true;
		meshBuilder.Normals.Clear ();
		meshBuilder.Vertices.Clear ();
		meshBuilder.Indices.Clear ();
		points.Clear ();
		//meshBuilder.
		//endpoints.Clear ();
	//	segments.Clear ();

		//bodies = FindObjectsOfType<Body> ();
	//	LoadBoundaries ();

	//	foreach(Body b in bodies){ 

			//this.AddSquareOccluder(b.transform.position,1f,0);
			//public void AddSquareOccluder(Vector2 position, float width, float rotation)
	//	}
			Vector2 previousHit;

		int previousAngle = 0;
		for (int angle=0; angle<360; angle+=1) {

			Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad *angle ), Mathf.Sin(Mathf.Deg2Rad * angle), 0);

			RaycastHit2D hit = Physics2D.Raycast (transform.position, direction,10f);
			if(hit.collider!=null) {

				points.Add(new Vector2(hit.point.x,hit.point.y));

			} else {
				//Vector2 dir = new Vector2(transform.position - hit.point).normalized;

				points.Add(direction * 10f);
			
			}

		}



		meshBuilder.Vertices.Add (new Vector3(0,0,0));

		foreach (Vector2 v in points) {
			meshBuilder.Vertices.Add (new Vector3(v.x,v.y,0));
		}

		for (int i = 0; i < points.Count-1; i++)
		{
			meshBuilder.AddTriangle(0,i+2,i+1);
			
			
			
			//	indices.Add(0);
			//	indices.Add((short)(i + 2));
			//	indices.Add((short)(i + 1));                
		}

		//List<short> indices = new List<short>();
		//for (int i = 0; i < points.Count; i += 2)
//		{
//			meshBuilder.AddTriangle(0,i+2,i+1);



		//	indices.Add(0);
		//	indices.Add((short)(i + 2));
		//	indices.Add((short)(i + 1));                
//		}

		Mesh m = meshBuilder.CreateMesh ();
		meshFilter.mesh = m;


		//AddTriangle


	}

	
	/// <summary>
	/// Helper function to construct segments along the outside perimiter
	/// in order to limit the radius of the light
	/// </summary>        
	private void LoadBoundaries()
	{
		//Top
		AddSegment(new Vector2(origin.x - radius, origin.y - radius),
		           new Vector2(origin.x + radius, origin.y - radius));
		
		//Bottom
		AddSegment(new Vector2(origin.x - radius, origin.y + radius),
		           new Vector2(origin.x + radius, origin.y + radius));
		
		//Left
		AddSegment(new Vector2(origin.x - radius, origin.y - radius),
		           new Vector2(origin.x - radius, origin.y + radius));
		
		//Right
		AddSegment(new Vector2(origin.x + radius, origin.y - radius),
		           new Vector2(origin.x + radius, origin.y + radius));
	}        


	// Add a segment, where the first point shows up in the
	// visualization but the second one does not. (Every endpoint is
	// part of two segments, but we want to only show them once.)
	private void AddSegment(Vector2 p1, Vector2 p2)
	{
		Segment segment = new Segment();
		EndPoint endPoint1 = new EndPoint();
		EndPoint endPoint2 = new EndPoint();
		
		endPoint1.Position = p1;
		endPoint1.Segment = segment;
		
		endPoint2.Position = p2;
		endPoint2.Segment = segment;
		
		segment.P1 = endPoint1;
		segment.P2 = endPoint2;
		
		segments.Add(segment);
		endpoints.Add(endPoint1);
		endpoints.Add(endPoint2);
	}

	/// <summary>
	/// Add a square shaped occluder
	/// </summary>        
	public void AddSquareOccluder(Vector2 position, float width, float rotation)
	{
		float x = position.x;
		float y = position.y;
		
		// The distance to each corner is the half of the width times sqrt(2)
		float radius = width * 0.5f * 1.41f;
		
		// Add Pi/4 to get the corners
		rotation += (Mathf.PI / 4f); 
		
		Vector2[] corners = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			corners[i] = new Vector2
				(
					(float)Math.Cos(rotation + i * Mathf.PI * 0.5) * radius + x,
					(float)Math.Sin(rotation + i * Mathf.PI * 0.5) * radius + y
					);
		}
		
		AddSegment(corners[0], corners[1]);
		AddSegment(corners[1], corners[2]);
		AddSegment(corners[2], corners[3]);
		AddSegment(corners[3], corners[0]);
	}          

	// Processess segments so that we can sort them later
	private void UpdateSegments()
	{            
		foreach(Segment segment in segments)
		{                               
			// NOTE: future optimization: we could record the quadrant
			// and the y/x or x/y ratio, and sort by (quadrant,
			// ratio), instead of calling atan2. See
			// <https://github.com/mikolalysenko/compare-slope> for a
			// library that does this.

			segment.P1.Angle = (float)Math.Atan2(segment.P1.Position.y - origin.y,
			                                     segment.P1.Position.x - origin.x);
			segment.P2.Angle = (float)Math.Atan2(segment.P2.Position.y - origin.y,
			                                     segment.P2.Position.x - origin.x);
			
			// Map angle between -Pi and Pi
			float dAngle = segment.P2.Angle - segment.P1.Angle;
			if (dAngle <= -Mathf.PI) { dAngle += (Mathf.PI/2f); }
			if (dAngle > Mathf.PI) { dAngle -= (Mathf.PI/2f); }
			
			segment.P1.Begin = (dAngle > 0.0f);
			segment.P2.Begin = !segment.P1.Begin;                
		}
	}          

	// Helper: do we know that segment a is in front of b?
	// Implementation not anti-symmetric (that is to say,
	// _segment_in_front_of(a, b) != (!_segment_in_front_of(b, a)).
	// Also note that it only has to work in a restricted set of cases
	// in the visibility algorithm; I don't think it handles all
	// cases. See http://www.redblobgames.com/articles/visibility/segment-sorting.html
	private bool SegmentInFrontOf(Segment a, Segment b, Vector2 relativeTo)
	{
		// NOTE: we slightly shorten the segments so that
		// intersections of the endpoints (common) don't count as
		// intersections in this algorithm                        
		
		bool a1 = VectorMath.LeftOf(a.P2.Position, a.P1.Position, VectorMath.Interpolate(b.P1.Position, b.P2.Position, 0.01f));
		bool a2 = VectorMath.LeftOf(a.P2.Position, a.P1.Position, VectorMath.Interpolate(b.P2.Position, b.P1.Position, 0.01f));
		bool a3 = VectorMath.LeftOf(a.P2.Position, a.P1.Position, relativeTo);
		
		bool b1 = VectorMath.LeftOf(b.P2.Position, b.P1.Position, VectorMath.Interpolate(a.P1.Position, a.P2.Position, 0.01f));
		bool b2 = VectorMath.LeftOf(b.P2.Position, b.P1.Position, VectorMath.Interpolate(a.P2.Position, a.P1.Position, 0.01f));
		bool b3 = VectorMath.LeftOf(b.P2.Position, b.P1.Position, relativeTo);                        
		
		// NOTE: this algorithm is probably worthy of a short article
		// but for now, draw it on paper to see how it works. Consider
		// the line A1-A2. If both B1 and B2 are on one side and
		// relativeTo is on the other side, then A is in between the
		// viewer and B. We can do the same with B1-B2: if A1 and A2
		// are on one side, and relativeTo is on the other side, then
		// B is in between the viewer and A.
		if (b1 == b2 && b2 != b3) return true;
		if (a1 == a2 && a2 == a3) return true;
		if (a1 == a2 && a2 != a3) return false;
		if (b1 == b2 && b2 == b3) return false;
		
		// If A1 != A2 and B1 != B2 then we have an intersection.
		// Expose it for the GUI to show a message. A more robust
		// implementation would split segments at intersections so
		// that part of the segment is in front and part is behind.
		
		//demo_intersectionsDetected.push([a.p1, a.p2, b.p1, b.p2]);
		return false;
		
		// NOTE: previous implementation was a.d < b.d. That's simpler
		// but trouble when the segments are of dissimilar sizes. If
		// you're on a grid and the segments are similarly sized, then
		// using distance will be a simpler and faster implementation.
	}
	
	/// <summary>
	/// Computes the visibility polygon and returns the vertices
	/// of the triangle fan (minus the center vertex)
	/// </summary>        
	public List<Vector2> Compute()
	{
		List<Vector2> output = new List<Vector2>();
		LinkedList<Segment> open = new LinkedList<Segment>();
		
		UpdateSegments();
		
		endpoints.Sort(radialComparer);
		
		float currentAngle = 0;
		
		// At the beginning of the sweep we want to know which
		// segments are active. The simplest way to do this is to make
		// a pass collecting the segments, and make another pass to
		// both collect and process them. However it would be more
		// efficient to go through all the segments, figure out which
		// ones intersect the initial sweep line, and then sort them.
		for(int pass = 0; pass < 2; pass++)
		{
			foreach(EndPoint p in endpoints)
			{
				Segment currentOld = open.Count == 0 ? null : open.First.Value;
				
				if (p.Begin)                    
				{
					// Insert into the right place in the list
					var node = open.First;
					while (node != null && SegmentInFrontOf(p.Segment, node.Value, origin))
					{
						node = node.Next;
					}
					
					if (node == null)
					{
						open.AddLast(p.Segment);
					}
					else
					{
						open.AddBefore(node, p.Segment);
					}
				}
				else
				{
					open.Remove(p.Segment);
				}
				
				
				Segment currentNew = null;
				if(open.Count != 0)
				{                
					currentNew = open.First.Value;
				}
				
				if(currentOld != currentNew)
				{
					if(pass == 1)
					{
						AddTriangle(output, currentAngle, p.Angle, currentOld);
						
					}
					currentAngle = p.Angle;
				}
			}
		}
		
		return output;
	}   

	private void AddTriangle(List<Vector2> triangles, float angle1, float angle2, Segment segment)
	{
		Vector2 p1 = origin;
		Vector2 p2 = new Vector2(origin.x + (float)Math.Cos(angle1), origin.y + (float)Math.Sin(angle1));
		Vector2 p3 = Vector2.zero;
		Vector2 p4 = Vector2.zero;
		
		if(segment != null)
		{
			// Stop the triangle at the intersecting segment
			p3.x = segment.P1.Position.x;
			p3.y = segment.P1.Position.y;
			p4.x = segment.P2.Position.x;
			p4.y = segment.P2.Position.y;
		}
		else
		{
			// Stop the triangle at a fixed distance; this probably is
			// not what we want, but it never gets used in the demo
			p3.x = origin.x + (float)Math.Cos(angle1) * radius * 2;
			p3.y = origin.y + (float)Math.Sin(angle1) * radius * 2;
			p4.x = origin.x + (float)Math.Cos(angle2) * radius * 2;
			p4.y = origin.y + (float)Math.Sin(angle2) * radius * 2;
		}

		Vector2 pBegin = VectorMath.LineLineIntersection(p3, p4, p1, p2);
		
		p2.x = origin.x + (float)Math.Cos(angle2);
		p2.y = origin.y + (float)Math.Sin(angle2);
		
		Vector2 pEnd = VectorMath.LineLineIntersection(p3, p4, p1, p2);
		
		triangles.Add(pBegin);
		triangles.Add(pEnd);
	}
  
}

internal class EndPointComparer : IComparer<EndPoint>
{
	internal EndPointComparer() { }
	
	// Helper: comparison function for sorting points by angle
	public int Compare(EndPoint a, EndPoint b)
	{          
		// Traverse in angle order
		if (a.Angle > b.Angle) { return 1; }
		if (a.Angle < b.Angle) { return -1; }
		// But for ties we want Begin nodes before End nodes
		if (!a.Begin && b.Begin) { return 1; }
		if (a.Begin && !b.Begin) { return -1; }
		
		return 0;
	}
}


/// <summary>    
/// The end-point of a segment    
/// </summary>
internal class EndPoint
{
	/// <summary>
	/// Position of the segment
	/// </summary>
	internal Vector2 Position { get; set; }
	
	/// <summary>
	/// If this end-point is a begin or end end-point
	/// of a segment (each segment has only one begin and one end end-point
	/// </summary>
	internal bool Begin { get; set; }
	
	/// <summary>
	/// The segment this end-point belongs to
	/// </summary>
	internal Segment Segment { get; set; }
	
	/// <summary>
	/// The angle of the end-point relative to the location of the visibility test
	/// </summary>
	internal float Angle { get; set; }
	
	internal EndPoint()
	{
		Position = Vector2.zero;
		Begin = false;
		Segment = null;
		Angle = 0;
	}
	
	public override bool Equals(object obj)
	{
		if(obj is EndPoint)
		{
			EndPoint other = (EndPoint)obj;
			
			return  Position.Equals(other.Position) &&
				Begin.Equals(other.Begin) &&                        
					Angle.Equals(other.Angle);
			
			// We do not care about the segment beeing the same 
			// since that would create a circular reference
		}
		
		return false;
	}
	
	public override int GetHashCode()
	{
		return  Position.GetHashCode() +
			Begin.GetHashCode() +                    
				Angle.GetHashCode();
	}
	
	public override string ToString()
	{
		return "{ p:" + Position.ToString() + "a: " + Angle + " in " + Segment.ToString() + "}";
	}
}

internal class Segment
{
	/// <summary>
	/// First end-point of the segment
	/// </summary>
	internal EndPoint P1 { get; set; }
	
	/// <summary>
	/// Second end-point of the segment
	/// </summary>
	internal EndPoint P2 { get; set; }
	
	internal Segment()
	{
		P1 = null;
		P2 = null;            
	}
	
	public override bool Equals(object obj)
	{
		if(obj is Segment)
		{
			Segment other = (Segment)obj;
			
			return  P1.Equals(other.P1) &&
				P2.Equals(other.P2);
		}
		
		return false;
	}
	
	public override int GetHashCode()
	{
		return  P1.GetHashCode() +
			P2.GetHashCode();
	}
	
	public override string ToString()
	{
		return "{" + P1.Position.ToString() + ", " + P2.Position.ToString() + "}";
	}
}

/// <summary>
/// Common mathematical functions with vectors
/// </summary>
public static class VectorMath
{
	
	/// <summary>
	/// Computes the intersection point of the line p1-p2 with p3-p4
	/// </summary>        
	public static Vector2 LineLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		// From http://paulbourke.net/geometry/lineline2d/
		var s = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x))
			/ ((p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y));
		return new Vector2(p1.x + s * (p2.x - p1.x), p1.y + s * (p2.y - p1.y));
	}
	
	/// <summary>
	/// Returns if the point is 'left' of the line p1-p2
	/// </summary>        
	public static bool LeftOf(Vector2 p1, Vector2 p2, Vector2 point)
	{
		float cross = (p2.x - p1.x) * (point.y - p1.y)
			- (p2.y - p1.y) * (point.x - p1.x);
		
		return cross < 0;
	}
	
	/// <summary>
	/// Returns a slightly shortened version of the vector:
	/// p * (1 - f) + q * f
	/// </summary>        
	public static Vector2 Interpolate(Vector2 p, Vector2 q, float f)
	{
		return new Vector2(p.x * (1.0f - f) + q.x * f, p.y * (1.0f - f) + q.y * f);
	}
}
