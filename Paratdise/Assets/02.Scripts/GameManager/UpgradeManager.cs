using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public Button[] buttons;
    public Image targetToolImage;
    public Sprite[] toolImages;
    public UpgradeToolRegion targetToolRegion;//enum κΈΈμ΄ ?? ?±

    private CharacterType characterType;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        
    }

    //Ύΐ ½ΗΗΰ½ΓΏ‘ ΄λ»σ ΅΅±Έ ΐΜΉΜΑφ ΊΈΏ©Αά
    private void OnEnable()
    {
        ViewTargetToolImage();
    }

    void Update()
    {
        
    }

    //? ?­λ³??λ‘? ?κ·Έλ ?΄? ?
    public void ToolUpgradeNow(int _targetRegion)
    {
        // 0-->Length,
        // 1-->Width,
        // 2-->Luck,
        // 3-->Strength,
        characterType = GameInfoController.GetCharacterTypeByIndex(ChooseCharacterUI.currentCharacterIndex);
        GetCharacterTypeByIndex();
        UpgradeCheck(characterType, (UpgradeToolRegion)_targetRegion);
    }

    private void ViewTargetToolImage()
    {
        targetToolImage.sprite = toolImages[ChooseCharacterUI.currentCharacterIndex];
    }

    private void GetCharacterTypeByIndex()
    {
        switch (ChooseCharacterUI.currentCharacterIndex)
        {
            case 0:
                characterType = CharacterType.Mice;
                break;
            case 1:
                characterType = CharacterType.Laila;
                break;
            case 2:
                characterType = CharacterType.DrillGgabijo;
                break;
            case 3:
                characterType = CharacterType.Eily;
                break;
        }
    }

    private void UpgradeCheck(CharacterType _character, UpgradeToolRegion _region)
    {
        CharacterData characterData = PlayerDataManager.data.GetCharacterData(_character);

        List<Ingredients>[] targetRegion = GetUpgradeToolType(_region);//region?Όλ‘? ?΄ ???? ?? €μ€?.
        List<ItemData> tempItemDatas = new List<ItemData>();//??? ??΄??€? λ¦¬μ€?Έλ‘? ?? ????₯.
        for (int ingredientType = 0; ingredientType < targetRegion.Length; ingredientType++)
        {
            int toolLevel = GetUpgradeLevel(characterData, _region);//λͺλ¨κ³? κ°ν?Έμ§? ??.
            ItemData targetItem = InventoryDataManager.data.itemsData.Find(x => x.itemName == targetRegion[toolLevel][ingredientType].name);
            //itemData κ°? ??΄??€? ? λ³΄λ?? κ°κ³  ??€λ©?, ?΄?Ή ??΄?? ? λ³΄λ?? InventoryDataManager.data.itemsData?? μ°Ύλ?€.
            
            if (!InventoryDataManager.data.itemsData.Contains(targetItem))
            {
                if (InventoryDataManager.data.itemsData.Find(x => x.itemName == targetItem.itemName).num < targetRegion[toolLevel][ingredientType].count)
                    return; //λ§μ½ ???κ²μ?΄??΄ ?Έλ²€ν λ¦¬μ ??€λ©? ?¨? μ’λ£.
            }//μΊλ¦­?°? enum? ?£?Όλ©? ?΄?Ή ?Έλ²€ν λ¦? λ°ν -> ??΄? μ²΄ν¬

            tempItemDatas.Add(targetItem);//λ§μ½ ??€λ©? ??? ??΄? ?? λ¦¬μ€?Έ? ????₯.
        }

        foreach (var item in tempItemDatas)//κ°ν? ??? ??΄??€? ???© ?λͺ?.
        {
            InventoryDataManager.data.RemoveData(item);//removeDataλ‘? ??΄? ?λͺ?.
        }

        ToolLevelUp(_region, characterData);
        SetCharacterIdentitiy(_character, characterData);
        //GameInfoController.toolInfos[ChooseCharacterUI.currentCharacterIndex].reinforceCount += 1; // ??΄? κ°ν ??λ₯? 1 μ¦κ??.
    }

    private void SetCharacterIdentitiy(CharacterType type, CharacterData data)
    {
        ToolInfo target = GameInfoController.GetToolByCharacter(type, GameInfoController.usingTools);
        target.toolHeight = GameInfoController.GetToolByCharacter(type).toolHeight + 0.05f * data.toolsLevel.heightLevel;
        target.toolWidth = GameInfoController.GetToolByCharacter(type).toolWidth + 0.05f * data.toolsLevel.widthLevel;
        target.luck = GameInfoController.GetToolByCharacter(type).luck + 5 * data.toolsLevel.luckLevel;
        target.power = (float)(GameInfoController.GetToolByCharacter(type).power + 0.25 * data.toolsLevel.strengthLevel);
    }

    private int GetUpgradeLevel(CharacterData data, UpgradeToolRegion targetRegion)
    {
        int _upgradeLevel = -1;
        int targetLevel = 0;

        if (targetRegion == UpgradeToolRegion.Length)
            targetLevel = data.toolsLevel.heightLevel;
        else if (targetRegion == UpgradeToolRegion.Width)
            targetLevel = data.toolsLevel.widthLevel;
        else if (targetRegion == UpgradeToolRegion.Luck)
            targetLevel = data.toolsLevel.luckLevel;
        else if (targetRegion == UpgradeToolRegion.Strength)
            targetLevel = data.toolsLevel.strengthLevel;

        if (targetLevel >= 0 && targetLevel < 4)
            _upgradeLevel = 0;
        else if (targetLevel >= 4 && targetLevel < 7)
            _upgradeLevel = 1;
        else if (targetLevel >= 7 && targetLevel < 9)
            _upgradeLevel = 2;
        else if (targetLevel >= 9 && targetLevel < 11)
            _upgradeLevel = 3;

        return _upgradeLevel;
    }

    private void ToolLevelUp(UpgradeToolRegion targetRegion, CharacterData data)
    {
        ToolsLevel temp = data.toolsLevel;
        if (targetRegion == UpgradeToolRegion.Length)
            temp.heightLevel += 1;
        else if (targetRegion == UpgradeToolRegion.Width)
            temp.widthLevel += 1;
        else if (targetRegion == UpgradeToolRegion.Luck)
            temp.luckLevel += 1;
        else if (targetRegion == UpgradeToolRegion.Strength)
            temp.strengthLevel += 1;
        data.toolsLevel = temp;
    }

    private List<Ingredients>[] GetUpgradeToolType(UpgradeToolRegion targetRegion)
    {
        //enum type? upgradetoolregion λ°μΌλ©? ?΄?Ή λΆ??? upgradeinfo ?΄??€? ingredients λ¦¬μ€?Έ λ°ν. 
        List<Ingredients>[] _region = null;
        if (targetRegion == UpgradeToolRegion.Length)
            _region = GameInfoController.upgradeInfo.length;
        else if (targetRegion == UpgradeToolRegion.Width)
            _region = GameInfoController.upgradeInfo.width;
        else if (targetRegion == UpgradeToolRegion.Luck)
            _region = GameInfoController.upgradeInfo.luck;
        else if (targetRegion == UpgradeToolRegion.Strength)
            _region = GameInfoController.upgradeInfo.strength;

        return _region;
    }

    #region ChracterTypeFind
    private CharacterType GetCharacterType(int byCount)
    {
        CharacterType _type = CharacterType.None;
        switch (byCount)
        {
            case 1:
                _type = CharacterType.Mice;
                break;
            case 2:
                _type = CharacterType.Laila;
                break;
            case 3:
                _type = CharacterType.DrillGgabijo;
                break;
            case 4:
                _type = CharacterType.Eily;
                break;
        }
        return _type;
    }

    private CharacterType GetCharacterType(string byName)
    {
        CharacterType _type = CharacterType.None;
        switch (byName)
        {
            case "λ§μ΄?€":
                _type = CharacterType.Mice;
                break;
            case "?Ό?Ό?Ό":
                _type = CharacterType.Laila;
                break;
            case "κΉ¨λΉ?λ¦΄μ‘°":
                _type = CharacterType.DrillGgabijo;
                break;
            case "??Όλ¦?":
                _type = CharacterType.Eily;
                break;
        }
        return _type;
    }
    #endregion

    public enum UpgradeToolRegion
    {
        Length,
        Width,
        Luck,
        Strength,
    }
}
