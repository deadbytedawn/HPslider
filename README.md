# HPslider

![Farmers Market Finder Demo](https://media.giphy.com/media/hsgOYIc1mC0D7PNMdb/giphy.gif)

*created in Unity 2019.2.5f1*

I needed a clean and simple HP slide for mobile that showed DMG &amp; HEALING. 

The trick is to create two Sliders with different fill colors (a FRONT and BACK slider) and give them the same rectTransform.
For Debugging in unity drop this script into an empty gameobject, make a UI button and attach the following methods to each TEST Button:
1.  BTN_STRING (in the string field add "damage" or "heal" without the quotes)
2. BTN_FLOAT (this is the AMOUNT you want to )
3. WrapperButton (combines 1 and 2 then starts the Switch statement "MyState")
