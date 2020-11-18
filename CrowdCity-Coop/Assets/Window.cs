using UnityEngine;

public abstract class Window : MonoBehaviour
{
    /*public TransitionHolder[] transitionHolders;*/
        
    public delegate void OpenEventHandler(Window sender);
    public event OpenEventHandler OnOpen;
        
    public virtual bool IsOpen
    {
        get => _isOpen;
        set
        {
            _isOpen = value;
            //TODO:: wrong state
            if (_isOpen)
            {
                OnOpen?.Invoke(this);
            }
            else
            {
                CurrentWindow?.Close();
            }
        }
    }
        
    private Window CurrentWindow { get; set; } = null;
    private bool _isOpen;
        
    private void Awake()
    {
        UIManager.RegisterWindow(this);
    }

    private void OnDestroy()
    {
        UIManager.UnregisterWindow(this);
    }
        
    public void Open()
    {
        IsOpen = true;
        SelfOpen();
    }

    public void Close()
    {
        IsOpen = false;
        SelfClose();
    }
        
    protected virtual void SelfOpen()
    {
        gameObject.SetActive(true);
        /*TransitionsOn();*/
    }
    protected virtual void SelfClose()
    {
        gameObject.SetActive(false);
    }
    
    protected void ChangeCurrentWindow(Window sender)
    {
        if (CurrentWindow != null)
        {
            CurrentWindow.Close();
        }
        
        CurrentWindow = sender;
    }
        
}