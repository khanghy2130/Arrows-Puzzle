using UnityEngine;

public class SkinButtonScript : MonoBehaviour  // each skin button has this. Only holds ID value.
{
    public int idNumber;
    public StoreMasterScript sms;


    public void skinClicked()
    {
        sms.skinClickedHandler(idNumber);
    }


}
