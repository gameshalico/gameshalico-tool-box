using ShalicoSoundKit.Runtime;
using ShalicoToolBox;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.SoundKit
{
    public class SoundKitSample : MonoBehaviour
    {
        [SerializeField] private AudioClip audioClip;

        private UIDocument _uiDocument;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            ISoundHandler soundHandler = null;
            _uiDocument.rootVisualElement.Q<Button>("play-sound-button").clicked += () =>
            {
                soundHandler = SoundManager.GetPlayer(audioClip).PlayWithFadeIn(2f);
            };
            _uiDocument.rootVisualElement.Q<Button>("random-pitch-button").clicked += () =>
            {
                soundHandler = SoundManager.GetPlayer(audioClip).SetRandomPitchByJustIntonation(-3, 3).Play();
            };
            _uiDocument.rootVisualElement.Q<Button>("fadeout-button").clicked += () =>
            {
                SoundManager.GetAllHandlers().ForEachAsync(async handler =>
                {
                    await handler.FadeOutAndStopAsync(0.5f);
                    handler.Release();
                });
            };
        }
    }
}