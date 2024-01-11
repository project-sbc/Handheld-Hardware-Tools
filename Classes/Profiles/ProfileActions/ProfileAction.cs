using Everything_Handhelds_Tool.Classes.Profiles.ProfileActions.ProfileActionClass;

using System.Xml.Serialization;

namespace Everything_Handhelds_Tool.Classes.Profiles.ProfileActions
{
    [XmlInclude(typeof(SetTDP))]
    public class ProfileAction
    {
        public string Value;
        public bool ChangeWithPowerState = false;
        public string OnBatteryValue;
        public virtual void ExecuteAction(string powerStatus) {}
    }
}
