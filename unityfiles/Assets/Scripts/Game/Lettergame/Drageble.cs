using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

///TODO: shit name
public abstract class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [SerializeField]
    private GameObject DraggingIcon;
    [SerializeField]
    private bool dragOnSurfaces = true;

    private string iconLetter = "?";
    private Text iconText;
    private RectTransform DraggingPlane;

    protected string IconLetter { get => iconLetter; set => iconLetter = value; }

    /// <summary>
    /// Tels whether or not the Ui element should start the drag operation 
    /// </summary>
    /// <returns>true if the drag operation can start false if not</returns>   
    protected abstract bool CanStartDrag ();

    /// <summary>
    /// What the Ui element should do when the drag operation is complete
    /// </summary>
    /// <param name="eventData">event args</param>
    protected abstract void OnDragCompletion (PointerEventData eventData);

    public void OnBeginDrag (PointerEventData eventData) {
        if (this.CanStartDrag ()) {
            DraggingIcon.SetActive (true);
            DraggingIcon.GetComponentInChildren<TMP_Text> ().text = IconLetter;

            var canvas = this.gameObject.GetComponentInParent<Canvas> ();

            if (dragOnSurfaces) {
                DraggingPlane = transform as RectTransform;
            } else {
                DraggingPlane = canvas.transform as RectTransform;
            }
            SetDraggedPosition (eventData);
        }

    }

    public void OnDrag (PointerEventData data) {
        if (DraggingIcon != null) {
            if (DraggingIcon.gameObject.activeSelf) {
                SetDraggedPosition (data);
            }
        }
    }

    public void OnEndDrag (PointerEventData eventData) {
        if (DraggingIcon != null) {
            DraggingIcon.SetActive (false);
            OnDragCompletion (eventData);
        }

    }

    private void SetDraggedPosition (PointerEventData data) {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null) {
            DraggingPlane = data.pointerEnter.transform as RectTransform;
        }
        var rt = DraggingIcon.GetComponent<RectTransform> ();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle (DraggingPlane, data.position, data.pressEventCamera, out globalMousePos)) {
            rt.position = globalMousePos;
            rt.rotation = DraggingPlane.rotation;
        }
    }

    // Start is called before the first frame update
    void Start () {
        GameObject newIcon = Instantiate (DraggingIcon);
        newIcon.transform.SetParent (this.transform.root);
        newIcon.name = "LetterDragIcon";
        this.DraggingIcon = newIcon;
        DraggingIcon.SetActive (false);
        iconText = DraggingIcon.GetComponentInChildren<Text> ();

    }
}