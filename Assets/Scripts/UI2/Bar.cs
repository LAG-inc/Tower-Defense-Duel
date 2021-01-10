using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public RectTransform bar;
    public GameObject wholeWidget;

    private float originalHP;
    private float currentHP;
    private Transform transformToFollow;
    private bool isHidden = true;

    private Color red = new Color32(252, 35, 13, 255);
	private Color blue = new Color32(31, 132, 255, 255);

    public void Initialise(GameObject obj)
    {
        if (obj.GetComponent<ThinkingGenerable>())
        {
            originalHP = currentHP = obj.GetComponent<ThinkingGenerable>().hitPoints;
            bar.GetComponent<Image>().color = (obj.GetComponent<ThinkingGenerable>().faction == Generable.Faction.Player) ? blue : red;

            wholeWidget.transform.localPosition = new Vector3(0f,
                                                        (.4f),
                                                        0f);
        }
        else if (obj.GetComponent<GenerableButton>())
        {
            originalHP = currentHP = obj.GetComponent<GenerableButton>().GetButtonData().generablesData[0].creationTime;
            bar.GetComponent<Image>().color = red;

            wholeWidget.transform.localPosition = new Vector3(0f,
                                                        (-0.45f),
                                                        0f);

        }

        transformToFollow = obj.transform;
        
        wholeWidget.SetActive(true);

    }

    public void SetTimer(float newTimer)
    {
        bar.localScale = new Vector3(newTimer, 1f, 1f);

    }

    public void SetHealth(float newHP)
    {
        if (isHidden)
        {
            wholeWidget.SetActive(true);
            isHidden = false;
        }
        float ratio = 0f;
        if(newHP > 0f)
        {
            ratio = newHP/originalHP;
        }

        bar.localScale = new Vector3(ratio, 1f, 1f);
    }

    public void Move()
    {
        if(transformToFollow != null)
            transform.position = transformToFollow.position;
    }
}
