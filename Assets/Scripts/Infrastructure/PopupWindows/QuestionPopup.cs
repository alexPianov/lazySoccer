using Cysharp.Threading.Tasks;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.Popup
{
    public class QuestionPopup : MonoBehaviour
    {
        [Title("Popup")]
        [SerializeField] private GameObject questionPopup;
        [SerializeField] private Image questionImage;
        [SerializeField] private Sprite[] questionImageSprites;
        
        [Title("Text")]
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text button;
        
        [Title("Status")]
        [SerializeField] private QuestionStatus questionStatus; 
        
        public enum QuestionStatus
        {
            None, Confirm, Cancel
        }
        
        public enum QuestionImage
        {
            None, Success, Error
        }
        
        public async UniTask<bool> OpenQuestion(string descriptionText = "", string titleText = "", 
            string buttonText = "Confirm", QuestionImage image = QuestionImage.None, 
            string Param1 = null, string Param2 = null)
        {
            ActivePopup(true);
            //SetQuestionImage(image);
            
            //title.text = titleText;
            title.GetComponent<Localize>().SetTerm("Question-" + titleText);
            //description.text = descriptionText;
            description.GetComponent<Localize>().SetTerm("Question-" + descriptionText);
            
            if (!string.IsNullOrEmpty(Param1))
            {
                description.GetComponent<LocalizationParamsManager>().SetParameterValue("param1", Param1);
            }
            
            if (!string.IsNullOrEmpty(Param2))
            {
                description.GetComponent<LocalizationParamsManager>().SetParameterValue("param2", Param2);
            }
            
            //button.text = buttonText;
            button.GetComponent<Localize>().SetTerm("Question-" + buttonText);

            await UniTask.WaitUntil(() => questionStatus != QuestionStatus.None);

            if (questionStatus == QuestionStatus.Confirm)
            {
                ActivePopup(false);
                return true;
            }
            
            ActivePopup(false);
            questionStatus = QuestionStatus.None;
            
            return false;
        }

        private void ActivePopup(bool state)
        {
            questionPopup.SetActive(state);
            
            if(!state) questionStatus = QuestionStatus.None;
        }

        public void Confirm()
        {
            questionStatus = QuestionStatus.Confirm;
        }

        public void Cancel()
        {
            questionStatus = QuestionStatus.Cancel;
        }
        
        private void SetQuestionImage(QuestionImage image)
        {
            switch (image)
            {
                case QuestionImage.None: 
                    questionImage.sprite = null;
                    questionImage.enabled = false;
                    break;
                
                case QuestionImage.Success: 
                    questionImage.sprite = questionImageSprites[0];
                    questionImage.enabled = true;
                    break;
                
                case QuestionImage.Error: 
                    questionImage.sprite = questionImageSprites[1];
                    questionImage.enabled = true;
                    break;
            }
        }
    }
}