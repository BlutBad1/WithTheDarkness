using ScriptableObjectNS.Locking;

public interface ILockingInteractable
{
    public KeyData GetKeyData();
    public void SetKeyData(KeyData keyData);
}
