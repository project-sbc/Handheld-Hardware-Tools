using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace Handheld_Hardware_Tools.Classes.Models
{
    public class Keyboard
    {
        public Dictionary<int, VirtualKeyCodeDisplayCharacter> leftKeyboard;
        public Dictionary<int, VirtualKeyCodeDisplayCharacter> rightKeyboard;
        public string keyboardMode;
    }
}
