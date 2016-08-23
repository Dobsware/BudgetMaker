using UnityEngine;
using System.Collections;

public class ButtonAux : MonoBehaviour {

    [HideInInspector]
    public int ChoiseNumber;

    public void NextFase()
    {
        GameObject.Find("Controller").GetComponent<Controller>().DoNextFase(ChoiseNumber);
    }

    public void BackFase()
    {
        GameObject.Find("Controller").GetComponent<Controller>().DoBackFase();
    }

    public void Reset()
    {
        GameObject.Find("Controller").GetComponent<Controller>().Reset();
    }
}
