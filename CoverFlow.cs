using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoverFlow : MonoBehaviour
{
    [SerializeField] private List<Sprite> IndexSprite = new List<Sprite>();
    [SerializeField] private GameObject ImageObject;
    private List<RectTransform> IndexRect = new List<RectTransform>();

    [SerializeField]private float Radius = 0;
    private int TargetIndex = 0;

    private float MovedPower = 0;//Goal Y
    private float originValue = 0;//Current Y

    void Start()
    {
        CoverflowInit();
    }

    public void CoverflowInit()
    {
        //Instancing UI Object
        for (int i = 0; i < IndexSprite.Count; i++)
        {
            GameObject tempObj = Instantiate(ImageObject, transform);
            tempObj.name = string.Format("Cover{0}", i);
            IndexRect.Add(tempObj.GetComponent<RectTransform>());
            tempObj.GetComponent<Image>().sprite = IndexSprite[i];
        }
    }

    public void CoverflowReset()
    {
        for (int i = IndexRect.Count-1; i > 0 ; i--)
        {
            Destroy(IndexRect[i].gameObject);
        }

        IndexRect.Clear();
        TargetIndex = 0;
        MovedPower = 0;
        originValue = 0;
    }

    void Update()
    {
        CoverflowInput();
        CoverflowUpdate();
    }

    void CoverflowInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (TargetIndex == 0)
                    TargetIndex = IndexSprite.Count - 1;
                else
                    TargetIndex--;
                MovedPower += 360 / IndexSprite.Count;
            }
            else
            {
                if (TargetIndex == IndexSprite.Count - 1)
                    TargetIndex = 0;
                else
                    TargetIndex++;
                MovedPower -= 360 / IndexSprite.Count;
            }
        }

        for (int i = 0; i <= IndexSprite.Count / 2; i++)
        {
            IndexRect[(TargetIndex + i) % IndexSprite.Count].localScale = Vector3.one * ((float)(IndexSprite.Count - i) / (float)IndexSprite.Count);
            IndexRect[(TargetIndex + i) % IndexSprite.Count].transform.SetAsFirstSibling();
            IndexRect[(TargetIndex - i) < 0 ? (TargetIndex - i) + IndexSprite.Count : (TargetIndex - i)].localScale = Vector3.one * ((float)(IndexSprite.Count - i) / (float)IndexSprite.Count);
            IndexRect[(TargetIndex - i) < 0 ? (TargetIndex - i) + IndexSprite.Count : (TargetIndex - i)].transform.SetAsFirstSibling();
        }
    }

    void CoverflowUpdate()
    {

        originValue = Mathf.Lerp(originValue, MovedPower, 5.6f * Time.deltaTime);

        for (int i = 0; i < IndexSprite.Count; i++)
        {
            IndexRect[i].anchoredPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * ((360 / IndexSprite.Count) * i + originValue)) * Radius * 0.2f, Mathf.Sin(Mathf.Deg2Rad * ((360 / IndexSprite.Count) * i + originValue)) * Radius, 0);
        }
    }

    public void OnClick()
    {
        
    }
}
