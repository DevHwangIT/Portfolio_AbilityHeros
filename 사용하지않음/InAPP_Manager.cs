using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAPP_Manager : MonoBehaviour, IStoreListener
{
    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {

    }

    void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {

    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {

    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs e)
    {
        return PurchaseProcessingResult.Complete;
    }
}
