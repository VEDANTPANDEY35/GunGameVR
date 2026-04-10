using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    public SimpleShoot gun;
    public TextMeshProUGUI ammoText;

    void Update()
    {
        ammoText.text = gun.currentAmmo + " / " + gun.magazineSize;
    }
}// PR test change
