using Everything_Handhelds_Tool.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Models
{
    public class NumericSpecialKeyboard : Keyboard
    {
        
        public NumericSpecialKeyboard() 
        {
            leftKeyboard = leftKeyboardLowerAlpha;
            rightKeyboard = rightKeyboardLowerAlpha;
        }

        private Dictionary<int, VirtualKeyCodeDisplayCharacter> leftKeyboardLowerAlpha = new Dictionary<int, VirtualKeyCodeDisplayCharacter>()
        {
            {0, new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_G, DisplayCharacter="g"}}, 
            {1,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_T, DisplayCharacter="t"}},
            {2,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_R, DisplayCharacter="r"}},
            {3,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_E, DisplayCharacter="e"}},
            {4,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_W, DisplayCharacter="w"}},
            {5,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_Q, DisplayCharacter="q"}},
            {6,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_A, DisplayCharacter="a"}},
            {7,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_Z, DisplayCharacter="z"}},
            {8,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_X, DisplayCharacter="x"}},
            {9,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_C, DisplayCharacter="c"}},
            {10,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_V, DisplayCharacter="v"}},
            {11,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_B, DisplayCharacter="b"}},
            {12,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_D, DisplayCharacter="d"}},
            {13,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_S, DisplayCharacter="s"}},
            {14,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_F, DisplayCharacter="f"}},
        
        };
        private Dictionary<int, VirtualKeyCodeDisplayCharacter> rightKeyboardLowerAlpha = new Dictionary<int, VirtualKeyCodeDisplayCharacter>()
        {
            {0, new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_G, DisplayCharacter=";"}},
            {1,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_P, DisplayCharacter="p"}},
            {2,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_O, DisplayCharacter="o"}},
            {3,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_I, DisplayCharacter="i"}},
            {4,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_U, DisplayCharacter="u"}},
            {5,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_Y, DisplayCharacter="y"}},
            {6,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_H, DisplayCharacter="h"}},
            {7,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_N, DisplayCharacter="n"}},
            {8,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_M, DisplayCharacter="m"}},
            {9,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.OEM_COMMA, DisplayCharacter=","}},
            {10,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.OEM_PERIOD, DisplayCharacter="."}},
            {11,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.SPACE, DisplayCharacter="?"}},
            {12,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_K, DisplayCharacter="k"}},
            {13,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_J, DisplayCharacter="j"}},
            {14,new VirtualKeyCodeDisplayCharacter{vkc = WindowsInput.Native.VirtualKeyCode.VK_L, DisplayCharacter="l"}},

        };
      

    }
}
