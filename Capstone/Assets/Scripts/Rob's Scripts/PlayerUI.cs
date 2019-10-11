using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject charge;
    [SerializeField] private GameObject focus;

    private float width;
    private Vector2 fullPosition;
    private Vector2 emptyPosition;    
    private Vector2 pastPosition;

    private bool aboveFocusCap;

    // Start is called before the first frame update
    void Start()
    {        
        width = background.GetComponent<RectTransform>().rect.width;
        Debug.Log("<color=red>Width: </color>" + width);

        fullPosition = background.transform.position;
        Debug.Log("<color=red>Full FULL: </color>" + fullPosition);
        
        emptyPosition = new Vector2(fullPosition.x - width, fullPosition.y);
        Debug.Log("<color=red>Empty EMPTY: </color>" + emptyPosition);

        pastPosition = new Vector2(fullPosition.x + width, fullPosition.y);
        Debug.Log("<color=red>Empty PAST: </color>" + pastPosition);

        //set health, charge, focus
        health.transform.position = fullPosition;
        StopCharge();
    }


    public void AdjustHealth(float percentage)
    {
        //check for mistakes
        if (percentage > 1)
        {
            percentage = 1;
        }
        else if (percentage < 0) 
        {
            percentage = 0;
        }       

        //Debug.Log(percentage);

        health.transform.position = new Vector2(emptyPosition.x + (width * percentage), emptyPosition.y);
    }

    public void AdjustCharge(float percentage)
    {
        //scripts calling this are already checking that its below health level
        Debug.Log(percentage);

        //move to the position percentage between full and empty
        charge.transform.position = new Vector2(emptyPosition.x + (width * percentage), emptyPosition.y);
    }

    public void AdjustFocus(float percentage)
    {
        //Attack Charge checks cap        
        Debug.Log(percentage);

        focus.transform.position = new Vector2(charge.transform.position.x + (width * percentage), pastPosition.y);
    }

    //setting objects on or not
    public void StartCharge()
    {
        charge.SetActive(true);
        focus.SetActive(true);
    }
    public void StopCharge()
    {
        //reset stuff
        charge.transform.position = emptyPosition;
        focus.transform.position = pastPosition;

        charge.SetActive(false);
        focus.SetActive(false);
    }
}
