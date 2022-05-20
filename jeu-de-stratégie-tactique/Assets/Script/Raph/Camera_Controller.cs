using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{

    private bool lerping;

    private Vector3 vect3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            transform.position = vect3;
        }
    }

    public void MenuCenter() { StartCoroutine(CameraLerp(Vector3.zero, 1.5f));
    }
    public void MenuLeft() { StartCoroutine(CameraLerp(Vector3.left * 18, 1.5f));
    }
    public void MenuRight() { StartCoroutine(CameraLerp(Vector3.right * 18, 1.5f));
    }
    public void MenuUp() { StartCoroutine(CameraLerp(Vector3.up * 10, 1.5f));
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        ;
    }
    

    public IEnumerator CameraLerp(Vector3 position, float speed)
    {
        position = new Vector3(position.x, position.y, -15);
        vect3 = transform.position;
        lerping = true;
        while (vect3 != position)
        {
            if (transform.position.x < position.x - (speed * .5f + .05f))
            {
                vect3 += new Vector3(speed, 0);
            }
            else if (transform.position.x > position.x + (speed * .5f + .05f))
            {
                vect3 -= new Vector3(speed, 0);
            }
            else if (transform.position.x != position.x)
            {
                vect3 = new Vector3(position.x, vect3.y, -15);
            }

            if (transform.position.y < position.y - (speed * .5f + .05f))
            {
                vect3 += new Vector3(0, speed);
            }
            else if (transform.position.y > position.y + (speed * .5f + .05f))
            {
                vect3 -= new Vector3(0, speed);
            }
            else if (transform.position.y != position.y)
            {
                vect3 = new Vector3(vect3.x, position.y, -15);
            }

            yield return new WaitForSeconds(Time.deltaTime);

        }
        transform.position = vect3;
        lerping = false;
        yield break;
    }
}
