using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PDollarGestureRecognizer;
using System.IO;

public class TrialStroke : MonoBehaviour
{
    public Text recogniseMessage;
	public Transform lineRenderPrefab;
	public InputField templateName;


	private List<Point> strokePoints = new List<Point>();
	private List<Gesture> classificationGestures = new List<Gesture>();
	private List<LineRenderer> setOfGestures = new List<LineRenderer>();
	private int strokeId = -1;

	private Vector3 currentPosition = Vector2.zero;
	private int pointCount = 0;
	private LineRenderer currentLineRenderer;

	private bool isClassified;

	public string myTemplatePath = "D:\\Files\\Sketch Recognition\\Project\\SketchRecognition\\Assets\\Scripts\\templates";
	private bool newTemplateAdded;

	void Start()
	{
		//Load your templates
		string[] filePaths = Directory.GetFiles(myTemplatePath, "*.xml");
		foreach (string file in filePaths)
			classificationGestures.Add(GestureIO.ReadGestureFromFile(file));
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
			{
				currentPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}

		bool isDrawingArea = checkMousePosition();

		if (isDrawingArea)
		{

			if (Input.GetMouseButtonDown(0))
			{

				if (isClassified || newTemplateAdded)
				{

					isClassified = false;
					newTemplateAdded = false;
					strokeId = -1;

					strokePoints.Clear();
					foreach (LineRenderer line in setOfGestures)
					{

						line.positionCount = 0;
						Destroy(line.gameObject);
					}

					setOfGestures.Clear();
				}

				++strokeId;

				Transform stroke = Instantiate(lineRenderPrefab, transform.position, transform.rotation) as Transform;
				currentLineRenderer = stroke.GetComponent<LineRenderer>();
				setOfGestures.Add(currentLineRenderer);
				pointCount = 0;
			}

			if (Input.GetMouseButton(0))
			{
				strokePoints.Add(new Point(currentPosition.x, -currentPosition.y, strokeId));
				++pointCount;
				currentLineRenderer.positionCount=pointCount;
				currentLineRenderer.SetPosition(pointCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x, currentPosition.y, 6)));
			}
		}
	}

	bool checkMousePosition()
	{
		bool isInside = false;
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.tag == "DrawingArea"){
				isInside = true;
			}
		}
		return isInside;
	}

	public void recogniseStroke()
	{
		isClassified = true;
		Gesture classifyArray = new Gesture(strokePoints.ToArray());
		Result classificationResult = PointCloudRecognizer.Classify(classifyArray, classificationGestures.ToArray());
		recogniseMessage.text = classificationResult.GestureClass;
	}
	public void addStroke()
	{
		newTemplateAdded = true;
		string name = String.Format("{0}/{1}-{2}.xml",myTemplatePath,templateName.text,DateTime.Now.ToFileTime());
		GestureIO.WriteGesture(strokePoints.ToArray(), templateName.text, name);

		classificationGestures.Add(new Gesture(strokePoints.ToArray(), templateName.text));
		templateName.text = " ";

	}
	
}
