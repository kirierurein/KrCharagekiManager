# KrCharagekiManager supports basic functions of ADV (character display, message display, etc.)

## How to use
KrCharagekiManager can play with two data of TextScript for describing ADV processing and csv for managing character's dialogue
By calling the Create function of KrCharagekiManager on the screen, you can generate the manager of the ADV function
e.g.
KrCharagekiManager.Create(["parent"], ["script paths"], ["character parent"], ["character  view  mode"], ["interval in auto mode"], ["asset base path"], ["server base path"]);

When the button is pressed, it can be sent to the next process by calling the TapScreen function of KrCharagekiManager

Get KrCharagekiUIController from KrCharagekiManager and set a callback such as pushing a button
- RegisterFade
- RegisterTitle
- RegisterBackground
- RegisterTextArea
- ToggleAutoMode
- ResetAutoMode

## Important point
- Audio data is supposed to be read from the outside, so it is only wav format
- Scenario data is in csv format only
- Do not enter a comma (,) as the scenario data is csv format

## TextScript Reference
TextScript is divided into four groups, const, resources, initialize, main

### const
const is the group that defines a constant
e.g.
const:
    hoge="hoge"	  // Set the character string "hoge" to the variable hoge
    $hoge        // To use constants, prefix $
end

### resources
resources is a group that declares external resources necessary for playing ADV
※ ※ You must implement download processing of external resources by yourself
e.g.
resources:
    "test/hoge.csv"  // Set the path under the base path
end

### initialize
initialize is a group for performing initialization processing for reproducing ADV
e.g.
initialize:
    load_scenario key="csv" path="test/hoge.csv" // Load csv data
end

#### initialize command
- Load scenario
load_scenario key=["(string)key to register this scenario"] path=["(string)csv path"]
- Load background
load_bg id=["(uint)background id"]
- Load character
load_chara id=["(uint)character id"]

### main
main is a group for processing when you actually play ADV
Each process is divided into units called section
e.g.
main:
    section:
        set_scenario key="csv" // Set csv data
    end
end

#### main command (within section)
- Setting text
set_text id=["(uint)text id of scenario"]
- Show text
show_text
- Setting scenario
set_scenario key=["(string)key of the registered scenario"]
- Setting background
set_bg id=["(uint)background id"]
- Show background
show_bg
- Hide background
hiSet the title associated with the background
set_title
- Show title
show_title
- Hide title
hide_title
- Fade out
fade_out
- Fade in
fade_in
- Toggle input wait setting
wait_input wait=["(boolean)wait for input"]
- Set waiting time
wait_time time=["(float)wait time"]
- Show character
show_chara id=["(uint)character id"]
- Hide character
hide_chara id=["(uint)character id"]
- Set character's motion and pose
chara_action chara=["(uint)character id"] action=["(uint)action id"]
- Setting the character's coordinates
chara_position chara=["(uint)character id"] position=["(string)position name"]
- Play se
play_se path=["(string)se path"]
- Play bgm
play_bgm path=["(string)bgm path"]

