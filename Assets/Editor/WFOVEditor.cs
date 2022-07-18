using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Hunting))]
public class WFOV : Editor
{

	void OnSceneGUI()
	{
		//BoidAlignment fow = (BoidAlignment)target;
		//Handles.color = Color.white;
		//Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
		//Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
		//Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

		//Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		//Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		//Handles.color = Color.red;
		//foreach (Transform visibleTarget in fow.visibleTargets)
		//{
		//	Handles.DrawLine(fow.transform.position, visibleTarget.position);
		//}

		Hunting fov = (Hunting)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
		Vector3 viewAngleA2 = fov.DirFromAngle(-fov.viewAngle / 2, false);
		Vector3 viewAngleB2 = fov.DirFromAngle(fov.viewAngle / 2, false);

		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA2 * fov.viewRadius);
		Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB2 * fov.viewRadius);

		Handles.color = Color.red;
		foreach (Transform visibleTarget in fov.visibleTargets)
		{
			if(fov.visibleTargets.Count != 0)
            {
				Handles.DrawLine(fov.transform.position, visibleTarget.position);
			}
			
		}
	}

}
