using deVoid.Utils;
using UnityEngine;

namespace SignalSystem
{
    public class ButtonSignalDispatcher : MonoBehaviour
    {
        public void DispatchShowMainMenuSignal()
        {
            Debug.Log("Сигнал отправлен: ShowMainMenuSignal");
            Signals.Get<ShowMainMenuSignal>().Dispatch();
        }

        public void DispatchShowAddMenuSignal()
        {
            Debug.Log("Сигнал отправлен: ShowAddMenuSignal");
            Signals.Get<ShowAddMenuSignal>().Dispatch();
        }

        public void DispatchShowRemoveMenuSignal()
        {
            Debug.Log("Сигнал отправлен: ShowRemoveMenuSignal");
            Signals.Get<ShowRemoveMenuSignal>().Dispatch();
        }

        public void DispatchAddResourceSignal()
        {
            Debug.Log("Сигнал отправлен: AddResourceSignal");
            Signals.Get<AddResourceSignal>().Dispatch();
        }

        public void DispatchRemoveResourceSignal()
        {
            Debug.Log("Сигнал отправлен: RemoveResourceSignal");
            Signals.Get<RemoveResourceSignal>().Dispatch();
        }

        public void DispatchResetResourcesSignal()
        {
            Debug.Log("Сигнал отправлен: ResetResourcesSignal");
            Signals.Get<ResetResourcesSignal>().Dispatch();
        }
    }
}