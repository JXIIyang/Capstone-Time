using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//RECTANGULAR TRAIL
//Instructions: Make an empty gameobject, put a MeshFilter and a MeshRenderer component on it
//Then drop this script on.
//As you move and rotate the game object a rectangular tube will be extruded
//If your tube seems inside-out, rotate the object 180° (it's going backwards)
//Don't forget to set the material on the Meshrenderer!
//this struct is used for the 4 vertices of each rectangular cross-section in the tube.
public struct QuadPos{
    public Vector3 nwPoint;
	public Vector3 nePoint;
	public Vector3 swPoint;
	public Vector3 sePoint;

// constructor
	public QuadPos(Vector3 nw, Vector3 ne, Vector3 sw, Vector3 se){
		nwPoint = nw;
		nePoint = ne;
		swPoint = sw;
		sePoint = se;
	} 
 }
[RequireComponent(typeof(MeshCollider), typeof(MeshRenderer), typeof(MeshFilter))]
public class RectangularMeshTrail : MonoBehaviour {
	private const int MAX_VERTEX_COUNT = 65000;

	MeshRenderer meshRenderer;
	public int numFrames = 300; //how many points to store in the line (we'll update it every frame)
	public float width = 3f; //width of the rectangle
	public float height = 1f; //height of the rectangle
	private List<QuadPos> transformList;
	private Mesh trailMesh;
	private MeshFilter meshFilter;
	private MeshCollider meshCollider;
	private Vector3[] verticesArray;
	private Vector3[] normalsArray;
	private int[] trianglesArray;
	private Color32[] colorsArray;
	private Vector2[] uvsArray;
	
	public Material gameMat;
	
	Bounds staticBounds;
	Vector3 nwVector;
	Vector3 swVector;
	Vector3 neVector;
	Vector3 seVector;

	float angle;

	bool updateTrail = true;
	public int dieFrame = 0;

	public CubicPlayerController myPlayer;

	void OnDestroy () {
		// you have to manually destroy procedural meshes otherwise you'll have a memory leak
		Destroy(trailMesh);
	}

	void Start () {
		transformList = new List<QuadPos>();
		trailMesh = new Mesh();
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();

		//get the camera location of the new vertices, converting local coords from this object to world coords
		Vector3 nwVertex = transform.TransformPoint(new Vector3(-0.5f*width,  0.5f*height,0));
		Vector3 neVertex = transform.TransformPoint(new Vector3( 0.5f*width,  0.5f*height,0));
		Vector3 swVertex = transform.TransformPoint(new Vector3(-0.5f*width, -0.5f*height,0));
		Vector3 seVertex = transform.TransformPoint(new Vector3( 0.5f*width, -0.5f*height,0));
		//seed transform list with some data
		verticesArray  = new Vector3[numFrames * 4];
		for (int i = 0; i < numFrames; i++) {
			//add the vertices from this frame to the list
			transformList.Add(new QuadPos(nwVertex, neVertex, swVertex, seVertex));
		}

		normalsArray   = new Vector3[numFrames * 4];
		colorsArray    = new Color32[numFrames * 4];
		trianglesArray = new int[((numFrames) * 8) * 3];
		uvsArray = new Vector2[numFrames * 4];

		swVector= new Vector3(-1,1,0);
		seVector= new Vector3(1,1,0);
		nwVector= new Vector3(-1,-1,0);
		neVector= new Vector3(-1,1,0);

		staticBounds = new Bounds(new Vector3(0,0,0), new Vector3(50f,50f,1f));

		meshRenderer.material = gameMat;
	}


	void OnEnable(){
		if (transformList != null) {
			
			transformList.Clear();
			for (int i = 0; i < numFrames; i++) {
				transformList.Add(new QuadPos(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero));
			}
		}	
	}

	
	void LateUpdate () {

		if (updateTrail)
		{
			transformList.RemoveAt(0); //remove the oldest one

			//get the camera location of the new vertices, converting local coords from this object to world coords
			Vector3 nwVertex = transform.TransformPoint(new Vector3(-0.5f*width,  0.5f*height,0));
			Vector3 neVertex = transform.TransformPoint(new Vector3( 0.5f*width,  0.5f*height,0));
			Vector3 swVertex = transform.TransformPoint(new Vector3(-0.5f*width, -0.5f*height,0));
			Vector3 seVertex = transform.TransformPoint(new Vector3( 0.5f*width, -0.5f*height,0));

			//add the vertices from this frame to the list
			transformList.Add(new QuadPos(nwVertex,neVertex,swVertex,seVertex));

			//construct the mesh
			for (int i = 0; i < numFrames; i++){
				//the vertex array is just a 1D list of vector3s in 
				verticesArray[i*4 + 0] = transform.InverseTransformPoint(transformList[i].nwPoint);
				verticesArray[i*4 + 1] = transform.InverseTransformPoint(transformList[i].nePoint);
				verticesArray[i*4 + 2] = transform.InverseTransformPoint(transformList[i].swPoint);
				verticesArray[i*4 + 3] = transform.InverseTransformPoint(transformList[i].sePoint);

				var remainingFrame = Mathf.Max(0f, i - dieFrame);
				verticesArray[i*4 + 0] = new Vector3(verticesArray[i*4 + 0].x * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 0].y * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 0].z);
				verticesArray[i*4 + 1] = new Vector3(verticesArray[i*4 + 1].x * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 1].y * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 1].z);
				verticesArray[i*4 + 2] = new Vector3(verticesArray[i*4 + 2].x * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 2].y * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 2].z);
				verticesArray[i*4 + 3] = new Vector3(verticesArray[i*4 + 3].x * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 3].y * (float)remainingFrame/(numFrames-dieFrame), verticesArray[i*4 + 3].z);

				//if we aren't using lighting, doesn't matter much what the normals are as long as they point toward the outside of the mesh 
				normalsArray[i*4 + 0] = swVector; 
				normalsArray[i*4 + 1] = seVector; 
				normalsArray[i*4 + 2] = nwVector; 
				normalsArray[i*4 + 3] = neVector; 

				//if we want to set the vertex color
				Color c = Color.white;

				colorsArray[i*4 + 0] = c;
				colorsArray[i*4 + 1] = c;
				colorsArray[i*4 + 2] = c;
				colorsArray[i*4 + 3] = c;
				
				// uv
				uvsArray[i*4 + 0] = new Vector2((float)i/numFrames + 0.01f, 0);
				uvsArray[i*4 + 1] = new Vector2((float)i/numFrames + 0.01f, 0.3f);
				uvsArray[i*4 + 2] = new Vector2((float)i/numFrames + 0.01f, 0.6f);
				uvsArray[i*4 + 3] = new Vector2((float)i/numFrames + 0.01f, 0.9f);
			}

			if (numFrames > 1){
				for (int i = 1; i < numFrames; i++){
					//the triangle array wants sets of three indices (corresponding to vertex indices in the vertex array)
					//the triangles have to be defined clockwise or they'll be back-to-front and will be culled.

					//the tube has 4 sides, so we need 8 triangles per segment
					trianglesArray[24*i + 2] = 4*(i-1);
					trianglesArray[24*i + 1] = 4*(i);
					trianglesArray[24*i + 0] = 4*(i-1)+2;

					trianglesArray[24*i + 5] = 4*(i);
					trianglesArray[24*i + 4] = 4*(i)+2;
					trianglesArray[24*i + 3] = 4*(i-1)+2;

					trianglesArray[24*i + 8] = 4*(i-1)+1;
					trianglesArray[24*i + 7] = 4*(i)+1;
					trianglesArray[24*i + 6] = 4*(i-1);

					trianglesArray[24*i + 11] = 4*(i)+1;
					trianglesArray[24*i + 10] = 4*(i);
					trianglesArray[24*i + 9] = 4*(i-1);

					trianglesArray[24*i + 14] = 4*(i-1)+3;
					trianglesArray[24*i + 13] = 4*(i)+3;
					trianglesArray[24*i + 12] = 4*(i-1)+1;

					trianglesArray[24*i + 17] = 4*(i)+3;
					trianglesArray[24*i + 16] = 4*(i)+1;
					trianglesArray[24*i + 15] = 4*(i-1)+1;

					trianglesArray[24*i + 20] = 4*(i-1)+2; //2,6,3
					trianglesArray[24*i + 19] = 4*(i)+2;
					trianglesArray[24*i + 18] = 4*(i-1)+3;

					trianglesArray[24*i + 23] = 4*(i)+2; //6,7,3
					trianglesArray[24*i + 22] = 4*(i)+3;
					trianglesArray[24*i + 21] = 4*(i-1)+3;
				}		
			}

			//pass the arrays to the mesh
			trailMesh.vertices = verticesArray;
			trailMesh.colors32 = colorsArray;
			trailMesh.normals = normalsArray;
			trailMesh.SetTriangles(trianglesArray,0);
			trailMesh.uv = uvsArray;
			trailMesh.bounds =  staticBounds;

			meshCollider.sharedMesh = trailMesh;
			meshFilter.sharedMesh = trailMesh;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (myPlayer.MyState == CubicPlayerController.PlayerState.Dash && other.CompareTag("Enemy"))
		{
			Destroy(other.gameObject);
		}
	}
}