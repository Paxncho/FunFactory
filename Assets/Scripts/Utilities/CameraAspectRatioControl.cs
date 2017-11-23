using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraAspectRatioControl : MonoBehaviour {

    public CanvasScaler[] canvas;
    public Vector2 TargetAspect = new Vector2(16, 9);

    int swidth;
    int sheight;
    Camera m_camera;

    // Use this for initialization
    void Start() {
        m_camera = GetComponent<Camera>();
        RefreshAspect();
        swidth = Screen.width;
        sheight = Screen.height;
    }

    // Update is called once per frame
    void Update () {
		if (swidth != Screen.width || sheight != Screen.height) {
            RefreshAspect();
            swidth = Screen.width;
            sheight = Screen.height;
        }
	}

    void RefreshAspect() {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = TargetAspect.x / (TargetAspect.y * 1.0f);

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f) {
            for (int i = 0; i < canvas.Length; i++) {
                canvas[i].matchWidthOrHeight = 0;
            }
            Rect rect = m_camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            m_camera.rect = rect;
        } else { // add pillarbox
            for (int i = 0; i < canvas.Length; i++) {
                canvas[i].matchWidthOrHeight = 1;
            }
            float scalewidth = 1.0f / scaleheight;

            Rect rect = m_camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            m_camera.rect = rect;
        }
    }
}
