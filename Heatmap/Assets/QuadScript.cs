using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadScript : MonoBehaviour
{
    Material mMaterial;
    MeshRenderer mMeshRenderer;
    public Texture2D brainTexture;

    float[] mPoints;
    int mHitCount;

    float mDelay;


    void Start()
    {
        mDelay = 3;

        mMeshRenderer = GetComponent<MeshRenderer>();
        mMaterial = mMeshRenderer.material;

        mPoints = new float[32 * 3]; //32 point 

    }

    void Update()
    {
        mDelay -= Time.deltaTime;
        if (mDelay <=0)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Projectile"));
            go.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-3f, -1f));

            mDelay = 3f;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var cp in collision.contacts)
        {
            RaycastHit hit;
            var rayLength = 0.1f;
            var ray = new Ray(cp.point + cp.normal * rayLength * 0.5f, -cp.normal);
            var C = Color.white; // default color when the raycast fails for some reason ;)
            if (cp.thisCollider.Raycast(ray, out hit, rayLength))
            {
                Debug.Log("Hit Object " + hit.collider.gameObject.name);
                Debug.Log("Hit Texture coordinates = " + hit.textureCoord);
                addHitPoint( hit.textureCoord.x,  hit.textureCoord.y);
            }
            else
            {
                Debug.Log("No Raycast Hit");
                Destroy(cp.otherCollider.gameObject);
            }
            
            // Instantiate your effect and
            // use the color C
        }
        
    }

    public void addHitPoint(float xp,float yp)
    {
        mPoints[mHitCount * 3] = xp;
        mPoints[mHitCount * 3 + 1] = yp;
        mPoints[mHitCount * 3 + 2] = Random.Range(1f, 3f);

        mHitCount++;
        mHitCount %= 32;

        mMaterial.SetFloatArray("_Hits", mPoints);
        mMaterial.SetInt("_HitCount", mHitCount);

    }

}