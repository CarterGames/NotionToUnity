# Notion Database to Unity Scriptable Object

A tool to download Notion databases into a Unity scriptable object for use in Unity projects. Handy for game data such as items, localization, skills and more! 


<br>


![Unity](https://img.shields.io/badge/Unity-2020.3.x_or_higher-critical?style=for-the-badge&color=8b8b8b)
<br>
![Notion API](https://img.shields.io/badge/Notion_API-2025/09/03-critical?style=for-the-badge&color=4f9148)
<br>
![Notion API 2](https://img.shields.io/badge/Notion_API-2022/06/28-critical?style=for-the-badge&color=918f48)
<br>
![GitHub all releases](https://img.shields.io/github/downloads/CarterGames/NotionToUnity/total?style=for-the-badge&color=8d6ca1)
<br>
![GitHub release (latest by date)](https://img.shields.io/github/v/release/CarterGames/NotionToUnity?style=for-the-badge)
<br>
![GitHub repo size](https://img.shields.io/github/repo-size/CarterGames/NotionToUnity?style=for-the-badge)
<br>
![Static Badge](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)


<br>

### Authors
- <a href="https://github.com/JonathanMCarter">Jonathan Carter</a>
<br>


### Dependencies
- Newtonsoft Json Unity Package which can be added via the Unity package manager with the following:
```
com.unity.nuget.newtonsoft-json
```
<br>

### Documentation
Full documentation can be found here: <a href="https://carter.games/docs/notiondata/documentation.pdf">https://carter.games/docs/notiondata/documentation.pdf</a>


<br><br>

# Table of contents
* [Features](#features)
* [Planned Features](#planned-features)
* [Supported Properties](#supported-properties)
* [Installation](#installation)
   * [Unity Package Manager (Git URL)](#unity-package-manager-git-url-recommended)
   * [Package Import (.unityPackage)](#unity-package-unitypackage)
   * [Manual (Clone)](#manual-clone)

<br><br>


# Features
- Download databases of any size.
- Apply sorting and filters to data to order it just as it is in a Notion database view.
- Automatic parsing of data into their field types.
- Support for most useful Notion data properties.
- Automatic API key removal on build creation for security.
- System to reference assets in code without a direct inspector reference.


<br><br>


# Planned Features
- None at the moment other than general support where needed.


<br><br>


# Supported Properties

Any ```string``` convertible type should also support JSON for custom classes, but the mileage may vary. Best to just store raw data in these assets and convert the data with an override to the ```PostDataDownloaded()``` method in the ```Notion Data Asset```. 

Note that rollups are supported only when they show a property that is otherwise supported below:

| Property type | Conversion types supported (Unity) |
| --- | --- |
| Title | ```string``` |
| Text | ```string``` ```NotionDataWrapper(GameObject(Prefab)/Sprite/AudioClip)```, List/Array of ```string```, ```int```, ```float```, ```double```, ```bool``` |
| Number | ```int``` ```float``` ```double``` etc |
| Toggle | ```bool``` |
| Single-Select | ```string``` ```enum``` |
| Multi-Select | ```string[]``` ```List<string>``` ```enum flags``` |
| Rollup | ```Any supported in this table.``` |
| Date | ```SerializableDateTime``` |
| Url | ```string```  |


<br><br>


# Installation
## Unity Package Manager (Git URL) [Recommended]
<b>Release:</b>
<br>
<i>The most up-to-date version of the repo that is considered stable enough for public use.</i>
```
https://github.com/CarterGames/NotionToUnity.git
```

<b>Pre-release:</b>
<br>
<i>Used to prepare future releases before releasing them, checking for bugs mainly. Will have the latest features but may not be stable just yet. Use at your own risk.</i>
```
https://github.com/CarterGames/NotionToUnity.git#pre-release
```

<br>


## Unity Package (.unitypackage)
Import the package found in the releases section through the "Import custom package" right-click menu option in Unity's project tab. 


<br>


## Manual (Clone)
Download the repo/clone it and import the files into your project manually.


<br><br>


Please refer to <a href="https://carter.games/docs/notiondata/documentation.pdf">documentation</a> for details on setup in your project & usage.
