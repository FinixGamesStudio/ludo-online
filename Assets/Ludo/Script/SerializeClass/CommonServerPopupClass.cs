using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonServerPopupClass
{
    [System.Serializable]
    public class CommonServerPopupResponseData
    {
        public bool isPopup;
        public string popupType;
        public string title;
        public string message;
        public int buttonCounts;
        public List<string> button_text;
        public List<string> button_color;
        public List<string> button_methods;
    }
    [System.Serializable]
    public class CommonServerPopupResponse
    {
        public string en;
        public CommonServerPopupResponseData data;
    }
}