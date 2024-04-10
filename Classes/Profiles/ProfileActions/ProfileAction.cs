using Handheld_Hardware_Tools.Classes.Profiles.ProfileActions.ProfileActionClass;

using System.Xml.Serialization;

namespace Handheld_Hardware_Tools.Classes.Profiles.ProfileActions
{
    [XmlInclude(typeof(SetTDP))]
    public class ProfileAction
    {
        public string Value;
        public bool ChangeWithPowerState = false;

        //if change with powerstate is true, onbatteryvalue becomes used
        public string OnBatteryValue;
        public virtual void ExecuteAction(string powerStatus) {}
    }
}
