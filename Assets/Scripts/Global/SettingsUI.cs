using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Global
{
    public class SettingsUI : MonoBehaviour
    {
        [Header("Button Objects")]
        [SerializeField] Button bgmMute;
        [SerializeField] Button sfxMute;
        [Header("Sprites")]
        [SerializeField] Texture2D bgmON;
        [SerializeField] Texture2D bgmOFF;
        [SerializeField] Texture2D sfxON;
        [SerializeField] Texture2D sfxOFF;

        bool bgm;
        bool sfx;

        private void Start()
        {
            bgm = GameManager.Instance.GetBGMMuted();
            sfx = GameManager.Instance.GetSFXMuted();
        }

        private void Update()
        {
            //update ui sprite based off of if muted or not
            if (bgm == false) { }
            else if (bgm == true) { }
            
            if (sfx == false) { }
            else if (sfx  == true) { }
        }

        public void BGM_Click()
        { 
            bgm = !bgm; //GameManager.Instance.GetBGMMuted();
            GameManager.Instance.SetBGMMuted(bgm);
        }

        public void SFX_Click()
        {
            sfx = !sfx;
            GameManager.Instance.SetSFXMuted(sfx);
        }

        public void Save_Click()
        {
            Debug.Log("Save Game");
        }

        public void Close_Click()
        {
            //GameObject.FindGameObjectWithTag("SettingsUI").SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
