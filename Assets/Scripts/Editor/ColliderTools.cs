using UnityEditor;
using UnityEngine;

namespace Editor {
    public static class ColliderTools {

        private const int MaxVertices = 16;

        [MenuItem("Tools/Create Moon Collider")]
        public static void CreateMoonCollider() {
            var currentGameObject = Selection.activeGameObject;
            if (currentGameObject == null || !currentGameObject.CompareTag("Controllable Moon")) {
                return;
            }
            var circleCollider = currentGameObject.GetComponent<CircleCollider2D>();
            if (circleCollider == null) {
                Debug.Log("A CircleCollider2D is required in order to determine collider radius");
                return;
            }
            var collider = currentGameObject.AddComponent<PolygonCollider2D>();

            var points = new Vector2[MaxVertices];
            for (var i = 0; i < MaxVertices; i++) {
                var angle = Mathf.PI * 2f / MaxVertices * i;

                points[i] = new Vector2(
                    Mathf.Cos(angle) * circleCollider.radius,
                    Mathf.Sin(angle) * circleCollider.radius
                );
                Debug.Log($"Angle: {angle}, point: {points[i]}");
            }
            collider.SetPath(0, points);
        }
    }
}
