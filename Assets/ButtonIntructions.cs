using UnityEngine;
using TMPro;
using Oculus.Interaction;

namespace Oculus.Interaction
{
    public class ButtonInstructions : MonoBehaviour
    {
        [Header("Interactable")]
        [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;  // Assign in Inspector

        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _normalColor = Color.red;
        [SerializeField] private Color _hoverColor = Color.blue;
        [SerializeField] private Color _selectColor = Color.green;
        [SerializeField] private Color _disabledColor = Color.black;

        [Header("Instruction UI")]
        public TextMeshProUGUI instructionText;
        [TextArea] public string[] instructions;

        [Header("Feedback UI")]
        public TextMeshProUGUI feedbackText;
        [TextArea] public string[] feedback;

        private IInteractableView InteractableView;
        private Material _material;
        private int currentStep = 0;
        private bool _started = false;

        public GetWebResponse gwb;
        private int step = 1;
        private int recipeNum = 1;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
            if (InteractableView == null)
            {
                Debug.LogError("Assigned object does not implement IInteractableView!");
            }
        }

        protected virtual void Start()
        {
            _started = true;
            AssertField(InteractableView, nameof(InteractableView));
            AssertField(_renderer, nameof(_renderer));

            _material = _renderer.material;

            // Show first instruction
            if (instructions.Length > 0 && instructionText != null)
                instructionText.text = instructions[currentStep];

            InteractableView.WhenStateChanged += OnStateChanged;
        }

        protected virtual void OnDestroy()
        {
            if (_material != null)
                Destroy(_material);

            if (InteractableView != null)
                InteractableView.WhenStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(InteractableStateChangeArgs args)
        {
            UpdateVisual();

            // Advance instruction if selected
            if (args.NewState == InteractableState.Select)
            {
                NextInstruction();
                // gwb.GetResponse();
                // string feedback = gwb.responseText;
                // feedbackText.text = feedback;
                NextFeedback();
                step++;

            }
        }

        private void UpdateVisual()
        {
            switch (InteractableView.State)
            {
                case InteractableState.Normal:
                    _material.color = _normalColor;
                    break;
                case InteractableState.Hover:
                    _material.color = _hoverColor;
                    break;
                case InteractableState.Select:
                    _material.color = _selectColor;
                    break;
                case InteractableState.Disabled:
                    _material.color = _disabledColor;
                    break;
            }
        }

        private void NextFeedback()
        {
            if (feedback.Length == 0 || feedbackText == null) return;

            // currentStep++;
            if (currentStep < feedback.Length)
            {
                feedbackText.text = feedback[currentStep];
            }
            else
            {
                feedbackText.text = "All steps completed!";
                gameObject.SetActive(false); // hide button when done
            }
        }

        private void NextInstruction()
        {
            if (instructions.Length == 0 || instructionText == null) return;

            currentStep++;
            if (currentStep < instructions.Length)
            {
                instructionText.text = instructions[currentStep];
            }
            else
            {
                instructionText.text = "All steps completed!";
                gameObject.SetActive(false); // hide button when done
            }
        }

        // Assertion helper that works for both Unity objects and interfaces
        private void AssertField(object obj, string name)
        {
            if (obj == null)
                Debug.LogError($"[{name}] is not assigned in {gameObject.name}");
        }
    }
}
