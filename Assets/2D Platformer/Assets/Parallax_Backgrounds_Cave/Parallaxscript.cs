using UnityEngine;

public class Parallaxscript : MonoBehaviour
{
    Material mat;
    float distance;

    [Range(0f, 0.5f)]
    public float speed = 0.2f;

    void start() {
        mat = GetComponent<Renderer>().material;
    }

    void update() {
        distance += Time.deltaTime * speed;
        mat.SetTextureOffset("_MainTex", Vector2.right*distance);
    }
}
