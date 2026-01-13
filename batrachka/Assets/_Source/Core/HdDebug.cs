using UnityEngine;
using HotdogLore;

namespace Core
{
    public class HdDebug : MonoBehaviour
    {
        public void Start()
        {
            Debug.Log("/// Отчет по хотдогам ///");
            
            DebugHotdogVariations(new ClassicHotdog());
            DebugHotdogVariations(new CaesarHotdog());
            DebugHotdogVariations(new MeatHotdog());
            
            Debug.Log("/// Конец отчета ///");
        }
        
        private void DebugHotdogVariations(Hotdog baseHotdog)
        {
            Debug.Log($"{baseHotdog.GetName()} — {baseHotdog.GetCost()}р.");
            Debug.Log("Дополнительная информация:");
            
            Hotdog hotdogWithPickles = new PicklesDecorator(baseHotdog); // Декоратор для огурцов
            Debug.Log($"{hotdogWithPickles.GetName()} — {hotdogWithPickles.GetCost()}р.");
            
            Hotdog hotdogWithSweetOnion = new SweetOnionDecorator(baseHotdog); // Декоратор для сладкого лука
            Debug.Log($"{hotdogWithSweetOnion.GetName()} — {hotdogWithSweetOnion.GetCost()}р.");
        }
    }
}
