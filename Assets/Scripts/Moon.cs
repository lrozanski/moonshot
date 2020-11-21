using UnityEngine;

public class Moon : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Controllable Moon")) {
            return;
        }
        transform.localScale += (Vector3) (Vector2.one * 0.1f);

        Destroy(other.gameObject);
    }
}
