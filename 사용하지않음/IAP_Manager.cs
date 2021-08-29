using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP_Manager : MonoBehaviour, IStoreListener
{
    public static iap_manager instance;

    private static istorecontroller storecontroller;
    private static iextensionprovider extensionprovider;

    //앱 결제
    public const string productid1 = market_manager.sale_soul_stone_10;
    public const string productid2 = market_manager.sale_soul_stone_50;
    public const string productid3 = market_manager.sale_soul_stone_100;

    // use this for initialization
    void start ()
    {
        if (instance == null)
            instance = this;
        initializepurchasing();
    }

    private bool isinitialized()
    {
        return (storecontroller != null && extensionprovider != null);
    }

    public void initializepurchasing()
    {
        if (isinitialized())
            return;

        var module = standardpurchasingmodule.instance();

        configurationbuilder builder = configurationbuilder.instance(module);

        builder.addproduct(productid1, producttype.consumable, new ids
        {
            { productid1, appleappstore.name },
            { productid1, googleplay.name },
        });

        builder.addproduct(productid2, producttype.consumable, new ids
        {
            { productid2, appleappstore.name },
            { productid2, googleplay.name }, }
        );

        builder.addproduct(productid3, producttype.consumable, new ids
        {
            { productid3, appleappstore.name },
            { productid3, googleplay.name },
        });

        unitypurchasing.initialize(this, builder);
    }

    public void buyproductid(string productid)
    {
        try
        {
            if (isinitialized())
            {
                product p = storecontroller.products.withid(productid);

                if (p != null && p.availabletopurchase)
                {
                    debug.log(string.format("purchasing product asychronously: '{0}'", p.definition.id));
                    storecontroller.initiatepurchase(p);
                }
                else
                {
                    debug.log("buyproductid: fail. not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                debug.log("buyproductid fail. not initialized.");
            }
        }
        catch (exception e)
        {
            debug.log("buyproductid: fail. exception during purchase. " + e);
        }
    }

    public void restorepurchase()
    {
        if (!isinitialized())
        {
            debug.log("restorepurchases fail. not initialized.");
            return;
        }

        if (application.platform == runtimeplatform.iphoneplayer || application.platform == runtimeplatform.osxplayer)
        {
            debug.log("restorepurchases started ...");

            var apple = extensionprovider.getextension<iappleextensions>();

            apple.restoretransactions
                (
                    (result) => { debug.log("restorepurchases continuing: " + result + ". if no further messages, no purchases available to restore."); }
                );
        }
        else
        {
            debug.log("restorepurchases fail. not supported on this platform. current = " + application.platform);
        }
    }

    public void oninitialized(istorecontroller sc, iextensionprovider ep)
    {
        debug.log("oninitialized : pass");

        storecontroller = sc;
        extensionprovider = ep;
    }

    public void oninitializefailed(initializationfailurereason reason)
    {
        debug.log("oninitializefailed initializationfailurereason:" + reason);
    }

    public purchaseprocessingresult processpurchase(purchaseeventargs args)
    {
        debug.log(string.format("processpurchase: pass. product: '{0}'", args.purchasedproduct.definition.id));

        switch (args.purchasedproduct.definition.id)
        {
            case productid1:
                startcoroutine(db_manager.instance.add_stone_db(10, "구입전 소울스톤 개수 :" + manager.instance.user.soul_stone, "상점 결제 - 10 소울 스톤 "));
                startcoroutine(db_manager.instance.load_data_db()); //db로부터 정보를 새로고침
                break;

            case productid2:
                startcoroutine(db_manager.instance.add_stone_db(50, "구입전 소울스톤 개수 :" + manager.instance.user.soul_stone, "상점 결제 - 50 소울 스톤 "));
                startcoroutine(db_manager.instance.load_data_db()); //db로부터 정보를 새로고침
                break;

            case productid3:
                startcoroutine(db_manager.instance.add_stone_db(100, "구입전 소울스톤 개수 :" + manager.instance.user.soul_stone, "상점 결제 - 100 소울 스톤 "));
                startcoroutine(db_manager.instance.load_data_db()); //db로부터 정보를 새로고침
                break;
        }

        return purchaseprocessingresult.complete;
    }

    public void onpurchasefailed(product product, purchasefailurereason failurereason)
    {
        debug.log(string.format("onpurchasefailed: fail. product: '{0}', purchasefailurereason: {1}", product.definition.storespecificid, failurereason));
    }
}
