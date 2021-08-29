using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Error_Manager{

    //Error code List

        public enum Error_Code
    {
        System_Error=1000,
        Server_Error=1001,
        Login_Error=1002,
        InApp_Error=1003,
        Inventory_Load_Error=1004
    }

    //시스템 에러 코드
    public const string System_Error = "Error_Code_1000";

    //서버 연결 에러
    public const string Server_Error = "Error_Code_1001";

    //로그인 에러
    public const string Login_Error = "Error_Code_1002";

    //인앱 결제 에러
    public const string InApp_Error = "Error_Code_1003";

    //인벤 로드 에러
    public const string Inventory_Load_Error = "Error_Code_1004";
    

    public static string Error_string_invert(Error_Code code)
    {
        switch (code)
        {
            case Error_Code.System_Error:
                return System_Error;

            case Error_Code.Server_Error:
                return Server_Error;

            case Error_Code.Login_Error:
                return Login_Error;

            case Error_Code.InApp_Error:
                return InApp_Error;

            case Error_Code.Inventory_Load_Error:
                return Inventory_Load_Error;

            default:
                return null;
        }
    }
}
