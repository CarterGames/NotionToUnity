# Notion Database To Unity Scriptable Object
A system to import Notion database data into a Unity scriptable object for use in game. 

<br><br>

## Supported Data Types
### Notion
| Property Type | Conversion Types Supported |
| --- | --- |
| Title | ```string``` |
| Text | ```string``` ```NotionDataWrapper(GameObject(Prefab)/Sprite/AudioClip)``` |
| Number | ```int``` ```float``` ```double``` etc |
| Toggle | ```bool``` |
| Single-Select | ```string``` ```enum``` |
| Multi-Select | ```string[]``` ```List<string>``` ```enum flags``` |

Any ```string``` convertable should also support JSON for custom classes, but the mileage may vary. Best to just store raw data in these assets and convert the data with an override to the ```PostDownloadLogic()``` method in the ```Notion Data Asset``` 

## Limitations
- Your Notion database can only have 100 entries max (Notion API limit).
- Downloaded data is in the order of creation (Notion API limit, its how the HTTP request returns the data). You can self order these after download should you wish with an override to the ```PostDownloadLogic()``` method in the ```Notion Data Asset``` classes.
- Limited Notion property support (May be improved in the future).


## Installation
### Package
Import the package found in the releases section through the "Import custom package" right-click menu option in Unity's project tab. 

### Manual 
Download the repo/clone it and import the files into your project manually.

<br>

## 💽 Notion Setup
You need to make an intergration in order for the downloading to work. You can make an intergration <a href="https://www.notion.so/my-integrations">here</a>. The steps to follow are:
- Make a new intergration with the ```New intergration``` button.
- Select the workspace the intergrations can access. This should be the workspace the database(s) you want to download are in.
- Give the intergration a name & continnue to the next page.
- Navigate to the ```Capabilities``` tab from the side bar and ensure the intergration has read access. You can disable the rest as you only need the read ability.
- Navigate to the ```Secrets``` tab and copy the key for use in Unity.

Once done you can then enter Notion and add the intergration to one or multiple pages to allow the Notion API to access the data. This is done from: ```... > Manage Connections > "Find and add your intergration from the options"```
If you don't see the intergration you just made listed, close & open Notion and follow the steps again. 

<br>

## 🎮 Unity Setup
In Unity you use a ```Notion Data Asset``` to store the data. This is just a scriptable object which has a custom inspect or to aid with the data download. Each instance you wake will consist of the data asset, a scriptable object class and the data class which holds the data structure for the data asset to store. There is a tool to make these for you which can be found under ```Tools > Notion To Unity > Asset Creator```

### Asset Creator
The asset creator is an editor window that handles creating the classes for a ```Notion Data Asset```. You just enter a name for the class you want to make and press ```Create``` when ready. You'll be able to see a preview of what the classes will be called before your press the ```Create``` button. Once pressed you'll be able to select where you want to save each newly generated class. Its best to keep them together in the same directory for ease of use. 

![image](https://github.com/CarterGames/NotionToUnity/assets/33253710/50d42e71-c459-4376-8a1e-7213fa03b067)

Once setup you'll just need to write your data class to have the fields you want. 
