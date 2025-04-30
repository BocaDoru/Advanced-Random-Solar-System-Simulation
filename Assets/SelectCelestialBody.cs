using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SelectCelestialBody : MonoBehaviour
{
    public MoveCamera mainCamera;
    public Planet Sun;
    public Planet curentCBody;
    public GameObject content;
    public Sprite sprite;
    public List<Button> celestialBodyButtons = new List<Button>();
    public List<Planet> satelites = new List<Planet>();
    public List<Planet> back = new List<Planet>();
    void Start()
    {
        back.Add(Sun);
        mainCamera.curentCBody = Sun;
        GenerateButtonListForLastBody(back[back.Count - 1]);
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            back.Remove(curentCBody);
            curentCBody = back[back.Count - 1];
            GenerateButtonListForLastBody(curentCBody);
        }
        back.RemoveAll(p => p == null);
    }
    public void GenerateButtonListForLastBody(Planet cBody)
    {
        for (int i = 0; i < celestialBodyButtons.Count; i++)
            Destroy(celestialBodyButtons[i].gameObject);
        celestialBodyButtons.Clear();
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, cBody.satelits.Count * 30);
        for (int i = 0; i < cBody.satelits.Count; i++)
        {
            if (cBody.satelits[i] != null)
            {
                DefaultControls.Resources resources = new DefaultControls.Resources();
                resources.background = sprite;
                resources.standard = sprite;
                GameObject buttonObj = DefaultControls.CreateButton(resources);
                buttonObj.name = cBody.satelits[i].name + "Button";
                buttonObj.transform.SetParent(content.transform);
                buttonObj.transform.localPosition = new Vector3(150, 15 - 30 * (i + 1), 0);
                buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 30);
                TextSettings(buttonObj.GetComponentInChildren<Text>(), "  " + cBody.satelits[i].name, cBody.satelits[i]);
                ButtonSettings(buttonObj.GetComponent<Button>(), cBody.satelits[i], buttonObj.GetComponentInChildren<Text>());
                celestialBodyButtons.Add(buttonObj.GetComponent<Button>());
            }
        }
    }
    void TextSettings(Text textObj,string text,Planet p)
    {
        textObj.alignment = TextAnchor.MiddleLeft;
        textObj.fontSize = 20;
        text += "   satelites:" + p.satelits.Count;
        textObj.text = text;
    }
    void ButtonSettings(Button button,Planet p,Text text)
    {
        button.gameObject.AddComponent<SwitchCelestialBody>().cBody = p;
        button.gameObject.GetComponent<SwitchCelestialBody>().select = this;
        button.onClick.AddListener(button.GetComponent<SwitchCelestialBody>().OnClick);
        button.GetComponent<SwitchCelestialBody>().text = text;
    }
}
