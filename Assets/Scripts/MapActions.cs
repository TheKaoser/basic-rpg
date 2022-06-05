using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapActions : MonoBehaviour
{
    public GameManager gameManager;

    private Vector2 touchPosition;
    private Vector3 direction;
    Vector2 firstPosition;
    public bool isDragging;

    public Image right;
    public Image left;
    public Image up;
    public Image down;

    bool hasTravelled;
    public bool canDrag = true;

    private void Update()
    {
        if (Input.touchCount > 0 && canDrag)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (!isDragging)
            {
                firstPosition = touchPosition;
                isDragging = true;
            }

            direction = (touchPosition - firstPosition);


            if (!hasTravelled)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    right.color = new Color(right.color.r, right.color.g, right.color.b, Mathf.Clamp(-(direction.x * 0.2f), -0.75f, 0.5f));
                    left.color = new Color(left.color.r, left.color.g, left.color.b, Mathf.Clamp((direction.x * 0.2f), -0.75f, 0.5f));
                }
                else
                {
                    if (gameManager.level < 25 || (!(gameManager.place == "volcano") && !(gameManager.place == "forest") && !(gameManager.place == "dungeon")))
                    {
                        up.color = new Color(up.color.r, up.color.g, up.color.b, Mathf.Clamp(-(direction.y * 0.2f), -0.75f, 0.5f));
                    }
                    if (gameManager.level > 1 || (!(gameManager.place == "volcano") && !(gameManager.place == "forest") && !(gameManager.place == "dungeon")))
                    {
                        down.color = new Color(down.color.r, down.color.g, down.color.b, Mathf.Clamp((direction.y * 0.2f), -0.75f, 0.5f));
                    }
                }

                if (right.color.a == 0.5f)
                {
                    StartCoroutine(gameManager.GoTo("right"));
                    hasTravelled = true;
                    firstPosition = touchPosition;
                    canDrag = false;
                }
                else if (left.color.a == 0.5f)
                {
                    StartCoroutine(gameManager.GoTo("left"));
                    hasTravelled = true;
                    firstPosition = touchPosition;
                    canDrag = false;
                }
                else if (up.color.a == 0.5f && (gameManager.level < 25 || (!(gameManager.place == "volcano") && !(gameManager.place == "forest") && !(gameManager.place == "dungeon"))))
                {
                    StartCoroutine(gameManager.GoTo("up"));
                    hasTravelled = true;
                    firstPosition = touchPosition;
                    if (!(gameManager.place == "volcano") && !(gameManager.place == "forest") && !(gameManager.place == "dungeon"))
                    {
                        canDrag = false;
                    }
                }
                else if (down.color.a == 0.5f && (gameManager.level > 1 || (!(gameManager.place == "volcano") && !(gameManager.place == "forest") && !(gameManager.place == "dungeon"))))
                {
                    StartCoroutine(gameManager.GoTo("down"));
                    hasTravelled = true;
                    firstPosition = touchPosition;
                    if (!(gameManager.place == "volcano") && !(gameManager.place == "forest") && !(gameManager.place == "dungeon"))
                    {
                        canDrag = false;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (!hasTravelled)
                {
                    right.color = new Color(right.color.r, right.color.g, right.color.b, 0);
                    left.color = new Color(left.color.r, left.color.g, left.color.b, 0);
                    up.color = new Color(up.color.r, up.color.g, up.color.b, 0);
                    down.color = new Color(down.color.r, down.color.g, down.color.b, 0);
                }
                isDragging = false;
                hasTravelled = false;
            }
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isDragging = false;
            hasTravelled = false;
        }
    }
}

