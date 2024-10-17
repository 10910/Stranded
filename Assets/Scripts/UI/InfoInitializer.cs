using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoInitializer : MonoBehaviour
{
    public CreatureInfoSO infoSO;
    public TextMeshProUGUI tmProUI;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(CreatureInfoSO infoSO){
        tmProUI.text = infoSO.creatureName;
        Texture2D tex = infoSO.imageTex;
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
    }

}
