using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


///TODO: shit name
public abstract class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    

    [SerializeField]
    private GameObject DraggingIcon;
    [SerializeField]
    private bool dragOnSurfaces = true;




    private string iconLetter = "?";
    private Text iconText;
    private RectTransform DraggingPlane;
    
    // -- properties -- //
    protected string IconLetter { get => iconLetter; set => iconLetter = value; }
    // -- events -- // 
    // -- public -- //
    // -- private -- // 
    // -- unity -- //


    protected abstract bool CanStartDrag();

    protected abstract void OnDragCompletion(PointerEventData eventData);

    public void OnBeginDrag(PointerEventData eventData){
        if (this.CanStartDrag()){
            DraggingIcon.SetActive(true);
            DraggingIcon.transform.SetAsLastSibling();
            DraggingIcon.GetComponentInChildren<Text>().text = IconLetter;
            
            var canvas = this.gameObject.GetComponentInParent<Canvas>();

            if (dragOnSurfaces)
                DraggingPlane = transform as RectTransform;
            else
                DraggingPlane = canvas.transform as RectTransform;

            SetDraggedPosition(eventData);
        }
        
    }

    public void OnDrag(PointerEventData data){
        if (DraggingIcon != null){
            if(DraggingIcon.gameObject.activeSelf){
                SetDraggedPosition(data);
            }
        }
    }

    private void SetDraggedPosition(PointerEventData data){
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = DraggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        if (DraggingIcon != null)
            DraggingIcon.SetActive(false);
            OnDragCompletion(eventData);
        
    }

    


    // Start is called before the first frame update
    void Awake(){
        GameObject newIcon = GameObject.Instantiate(DraggingIcon);
        newIcon.name = "DragIcon";
        newIcon.transform.SetParent(DraggingIcon.transform.parent);
        this.DraggingIcon = newIcon;
        DraggingIcon.SetActive(false);
        iconText = DraggingIcon.GetComponentInChildren<Text>();
        
    }
}
