public interface IModifyPPEffect
{
    void Init();
    void StartModify();
    void Finish();
    void SetNextAction(System.Action iAction);
    void ResetData();
}