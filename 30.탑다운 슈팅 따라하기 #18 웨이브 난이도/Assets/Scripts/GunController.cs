using UnityEngine;

// 무기를 장비하는 기능을 관리
public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;

    void Start()
    {
        if (startingGun != null)
            EquipGun(startingGun);
    }
    // 무기 생성 후 장착
    public void EquipGun(Gun gunToEquip)
    {
        if(equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGun.transform.parent = weaponHold;
    }
    // 마우스 버튼을 누르고 있을 때
    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }
    // 마우스 버튼을 땠을 때
    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }
}
