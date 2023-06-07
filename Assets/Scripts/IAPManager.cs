using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public enum coinPurchaseLevels { twoThousandCoins, fiveThousandCoins};
public class IAPManager : MonoBehaviour, IStoreListener
{
	public static IAPManager instance;
	
	private static IStoreController storeController;
	private static IExtensionProvider storeExtensionProvider;

	private string fiveThousandCoins = "com.bluesphere.tanghulu.5kCoins";
	private string twoThousandCoins = "com.bluesphere.tanghulu.2kCoins";

	public StoreScreenController storeScreenController;

    // Start is called before the first frame update
    void Start()
    {
        if(storeController == null) {
			InitializePurchasing();
		}
		instance = this;
    }

	public void InitializePurchasing() {
		if(IsInitialized()) return;
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		builder.AddProduct(fiveThousandCoins, ProductType.Consumable); 
		builder.AddProduct(twoThousandCoins, ProductType.Consumable); 

		UnityPurchasing.Initialize(this, builder);
	}

	private bool IsInitialized() {
		return storeController != null && storeExtensionProvider != null;
	}

	public void BuyProductID(coinPurchaseLevels purchaseLevel) {
		string productId = GetPurchaseString(purchaseLevel);
		if(IsInitialized()) {
			Product product = storeController.products.WithID(productId);
			if(product != null && product.availableToPurchase) {
				Debug.Log(string.Format("Purchasing product asynchronously: '{0}'", product.definition.id));

				storeController.InitiatePurchase(product);
			}
			else {
				storeScreenController.ShowPurchaseError("Not purchasing product, either is not found or not available");
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or not available");
			}
		} else {
			storeScreenController.ShowPurchaseError("Manager not initalized");
			Debug.Log("BuyProductID FAIL, Not initialized");
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
		if(String.Equals(args.purchasedProduct.definition.id, GetPurchaseString(coinPurchaseLevels.fiveThousandCoins), StringComparison.Ordinal)) {
			Debug.Log("Bought five k");
			SaveDataManager.instance.UpdatePlayerCoins(5000);
			storeScreenController.CoinsUpdated();
		}
		else if(String.Equals(args.purchasedProduct.definition.id, GetPurchaseString(coinPurchaseLevels.twoThousandCoins), StringComparison.Ordinal)) {
			Debug.Log("Bought two k");
			SaveDataManager.instance.UpdatePlayerCoins(2000);
			storeScreenController.CoinsUpdated();
		}
		else {
			Debug.LogError("Purchase failed");
		}
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		storeScreenController.ShowPurchaseError(failureReason.ToString());
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
		Debug.Log("Initialized IStoreController");
		
		storeController = controller;
		storeExtensionProvider = extensions;
	}

	public void RestorePurchases() {
		if(!IsInitialized()) {
			Debug.Log("Restore Purchases Failed. Purchaser not initialized");
		}

		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			Debug.Log("Restore Purchases Started");

			var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();

			apple.RestoreTransactions((result) => {
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
	}

	public void OnInitializeFailed(InitializationFailureReason error) {
		Debug.Log("IStoreController initialization failed: " + error);
	}

	public string GetPurchaseString(coinPurchaseLevels purchaseLevel) {
		if (purchaseLevel == coinPurchaseLevels.twoThousandCoins) return twoThousandCoins;
		else return fiveThousandCoins;
	}
}
