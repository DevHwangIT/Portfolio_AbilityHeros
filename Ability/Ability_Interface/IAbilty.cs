interface IAbilty
{
    //스킬 목차 인덱스번호
    int Index { get; set; }

    //스킬 이름
    string Name_Key { get; set; }

    //스킬 설명 정보
    string Info_Key { get; set; }

    //사용을 위해 필요한 소울 개수
    int Spend_Soul { get; set; }

    //스킬 사용 활성화 여부
    bool Skill_Activation { get; set; }

    //스킬 쿨타임
    int Skill_Delay { get; set; }
}
