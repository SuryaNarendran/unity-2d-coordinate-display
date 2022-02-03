using UnityEngine;
using UnityEditor;

namespace Utility
{
    /// <summary>
    /// This is used to find the mouse position when it's over a SceneView.
    /// Used by tools that are menu invoked.
    /// </summary>
    [InitializeOnLoad]
    public class MouseHelper : Editor
    {
        private static Vector2 position;

        public static Vector2 Position
        {
            get { return position; }
        }

        static MouseHelper()
        {
            SceneView.duringSceneGui += UpdateView;
        }

        private static void UpdateView(SceneView sceneView)
        {
            if (Event.current != null)
                position = new Vector2(Event.current.mousePosition.x + sceneView.position.x, Event.current.mousePosition.y + sceneView.position.y);
        }
    }
}