using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WFX_BulletHoleDecal : MonoBehaviour
{
    static private Vector2[] quadUVs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };
    public float lifetime = 10f;
    public float fadeoutpercent = 80;
    public Vector2 frames;
    public bool randomRotation = false;
    private float life;
    private float fadeout;
    private Color color;
    private float orgAlpha;
    private void Awake()
    {
        Renderer renderer = this.GetComponent<Renderer>();
        if (renderer.material.HasProperty("_TintColor"))
            color = renderer.material.GetColor("_TintColor");
        else if (renderer.material.HasProperty("_Color"))
            color = renderer.material.GetColor("_Color");
        orgAlpha = color.a;
    }
    private void OnEnable()
    {
        if (randomRotation)
        {
            //Random UVs
            int random = Random.Range(0, (int)(frames.x * frames.y));
            int fx = (int)(random % frames.x);
            int fy = (int)(random / frames.y);
            //Set new UVs
            Vector2[] meshUvs = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                meshUvs[i].x = (quadUVs[i].x + fx) * (1.0f / frames.x);
                meshUvs[i].y = (quadUVs[i].y + fy) * (1.0f / frames.y);
            }
            this.GetComponent<MeshFilter>().mesh.uv = meshUvs;
            //Random rotate
            this.transform.Rotate(0f, 0f, Random.Range(0f, 360f), Space.Self);
        }
        //Start lifetime coroutine
        life = lifetime;
        fadeout = life * (fadeoutpercent / 100f);
        color.a = orgAlpha;
        Renderer renderer = this.GetComponent<Renderer>();
        if (renderer.material.HasProperty("_TintColor"))
            renderer.material.SetColor("_TintColor", color);
        else if (renderer.material.HasProperty("_Color"))
            renderer.material.SetColor("_Color", color);
        StopAllCoroutines();
        StartCoroutine("holeUpdate");
    }
    private IEnumerator holeUpdate()
    {
        while (life > 0f)
        {
            life -= Time.deltaTime;
            if (life <= fadeout)
            {
                color.a = Mathf.Lerp(0f, orgAlpha, life / fadeout);
                Renderer renderer = this.GetComponent<Renderer>();
                if (renderer.material.HasProperty("_TintColor"))
                    renderer.material.SetColor("_TintColor", color);
                else if (renderer.material.HasProperty("_Color"))
                    renderer.material.SetColor("_Color", color);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
