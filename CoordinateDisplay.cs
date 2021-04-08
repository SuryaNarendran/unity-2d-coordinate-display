using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utility;

//a simple editor script that lets you see the grid coordinates
//that your mouse is hovering on in the scene window in 2D mode
//https://github.com/SuryaNarendran/unity-2d-coordinate-display

[InitializeOnLoad]
public class CoordinateDisplay : Editor
{
    static bool isEnabled = false;
    static CoordinateDisplay instance;
    static GUIStyle style;

    //this should be more accessible to the user - maybe in an editor window or settings file
    static float gridSize = 0.5f;


    static CoordinateDisplay()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        style = new GUIStyle();
        style.normal.textColor = Color.white;
    }

    [MenuItem("Tools/Toggle Grid Coordinates")]
    public static void ToggleGridCoordinates()
    {
        isEnabled = !isEnabled;
    }


    static void OnSceneGUI(SceneView view)
    {
        if (!isEnabled) return;

        Vector2 mousePosition = MouseHelper.Position;
        if (OutsideSceneView(view, mousePosition)) return;

        //finds the relative position of the mouse in the scene viewport
        float positionXInViewport = (mousePosition.x - view.position.x) / view.position.width;
        //the y coordinate has to be inverted and have the y position of the scene view window subtracted from it
        //since the scene view coordinate system is vertically inverted compared to the global mouse position
        float positionYInViewport = -1 * (mousePosition.y - view.position.y - view.position.height) / view.position.height;
        Vector3 positionInViewport = new Vector3(positionXInViewport, positionYInViewport, 0);

        //finds the world coordinates that the mouse is hovering over in the scene view (works only in 2D mode of course)
        Vector3 worldPosition = view.camera.ViewportToWorldPoint(positionInViewport);
        worldPosition.z = 0;

        //calculates the grid coordinates to display
        Vector2Int gridPosition = new Vector2Int(
            Mathf.FloorToInt(worldPosition.x/gridSize),
            Mathf.FloorToInt(worldPosition.y/gridSize));

        //draws the label next to the mouse
        Vector3 labelDisplayPosition = TransformByPixel(worldPosition, 5, 5);
        Handles.Label(labelDisplayPosition, gridPosition.x.ToString() + "," + gridPosition.y.ToString(), style);

        SceneView.RepaintAll();
    }

    static Vector3 TransformByPixel(Vector3 position, float x, float y)
    {
        return TransformByPixel(position, new Vector3(x, y));
    }

    static Vector3 TransformByPixel(Vector3 position, Vector3 translateBy)
    {
        Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
        if (cam)
            return cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translateBy);
        else
            return position;
    }

    static bool OutsideSceneView(SceneView view, Vector2 position)
    {
        if (position.x < view.position.x || position.x > view.position.x + view.position.width)
        {
            return true;
        }

        if (position.y < view.position.y || position.y > view.position.y + view.position.height)
        {
            return true;
        }

        return false;
    }

}
