# Notion Database to Unity Scriptable Object

A flexible system to import Notion databases into a Unity scriptable object for use in Unity game projects. Note: This is experimental. While functional, there may be issues or edge cases that are not covered. Updates will also be slow/infrequent.


<br>


![Unity](https://img.shields.io/badge/Unity-2020.3.x_or_higher-critical?style=for-the-badge&color=8b8b8b)
![Notion API](https://img.shields.io/badge/Notion_API-(V1)_2022/06/28-critical?style=for-the-badge&color=918f48)
![GitHub all releases](https://img.shields.io/github/downloads/CarterGames/NotionToUnity/total?style=for-the-badge&color=8d6ca1)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/CarterGames/NotionToUnity?style=for-the-badge)
![GitHub repo size](https://img.shields.io/github/repo-size/CarterGames/NotionToUnity?style=for-the-badge)
![Static Badge](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)


<br>

### Authors
- <a href="https://github.com/JonathanMCarter">Jonathan Carter</a>


<br><br>

# Table of contents
* [Features](#features)
* [Planned Features](#planned-features)
* [Supported Properties](#supported-properties)
* [Installation](#installation)
   * [Unity Package Manager (Git URL)](#unity-package-manager-git-url-recommended)
   * [Package Import (.unityPackage)](#unity-package-unitypackage)
   * [Manual (Clone)](#manual-clone)
* [Setup Guide](#setup-guide)
   * [Notion Setup](#-notion-setup)
   * [Unity Setup](#-unity-setup)
* [Downloading Data](#downloading-the-data)  
   * [Sorting Properties](#sorting-properties)
   * [Filters](#filters)
   * [Wrapper Classes](#wrapper-classes)
   * [Post Download Logic](#post-download-logic)
   * [Updating all assets](#updating-all-assets)
* [API Access](#scripting-api-info)
 	* [Custom Notion Database Processors](#custom-notion-database-processors)
	* [Accessing Notion Data Assets](#accessing-notion-data-assets)
* [Usage Example](#usage-example)

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


# Setup Guide

<br>

## 💽 Notion setup
You need to make an integration in order for the downloading to work. You can make an intergration <a href="https://www.notion.so/my-integrations">here</a>. The steps to follow are:
- Make a new integration with the ```New integration``` button.
- Select the workspace the integrations can access. This should be the workspace the database(s) you want to download are in.
- Give the integration a name & continue to the next page.
- Navigate to the ```Capabilities``` tab from the sidebar and ensure the integration has read access. You can disable the rest as you only need the readability.
- Navigate to the ```Secrets``` tab and copy the key for use in Unity.

Once done you can then enter Notion and add the integration to one or multiple pages to allow the Notion API to access the data. This is done from: ```... > Manage Connections > "Find and add your integration from the options"```
If you don't see the integration you just made listed, close & open Notion and try again. 

<br><br>

## 🎮 Unity setup
In Unity you use a ```Notion Data Asset``` to store the data. This is just a scriptable object which has a custom inspector to aid with the data download. Each instance you make will consist of the data asset, a scriptable object class and the data class which holds the data structure for the data asset to store. There is a tool to make these for you which can be found under ```Tools > Notion To Unity > Asset Creator```

### Asset creator
The asset creator is an editor window that handles creating the classes for a ```Notion Data Asset```. You just enter a name for the class you want to make and press ```Create``` when ready. You'll be able to see a preview of what the classes will be called before your press the ```Create``` button. Once pressed you'll be able to select where you want to save each newly generated class. It's best to keep them together in the same directory for ease of use. 

![image](https://github.com/CarterGames/NotionToUnity/assets/33253710/50d42e71-c459-4376-8a1e-7213fa03b067)

Once set up you'll just need to write your data class to have the fields you want. An example of a Persona healing skill data class

```
    [Serializable]
    public class DataHealingSkills
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Fields
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */

        [SerializeField] protected string skillName;
        [SerializeField, TextArea] protected string desc;
        [SerializeField] protected NotionDataWrapperSprite icon;
        [SerializeField] protected SkillType type;
        [SerializeField] protected ActionTarget target;
        [SerializeField] protected SkillCost cost;
        [SerializeField] protected NotionDataWrapperPrefab effect;
        [SerializeField] private float power;
        [SerializeField] private StatusAilment cureAilments;
        [SerializeField] private bool canRevive;
    }
```


<br><br>


# Downloading the data

To download your data you will need the link to the database page and the secret key for the integration you made earlier. The Database link can be grabbed from the ... menu on the page the database is on:

![image](https://github.com/CarterGames/NotionToUnity/assets/33253710/875ea852-c437-45c0-94cf-9b1940a88a1e)

Then just fill the fields on the data asset (make one from the ```CreateAssetMenu``` if you haven't already). You will need to assign a processor to parse the data. You can write custom ones to fit your use-cases or use the standard one. it will need to be assigned to download data. Read more on custom processors [here](#custom-notion-database-processors)

![Notion-processor-assign](https://github.com/user-attachments/assets/85da13e1-808c-4da4-a734-f3b5556e0ffa)

Then press the download button. If all goes well you'll see a dialogue stating so. If it fails you should see the error in the console.

![313002443-a78e3d35-e37d-4e12-ba3d-1a9d18109d04](https://github.com/user-attachments/assets/ef095cb6-befa-4287-bfcd-18dbed225df9)

<i>(Image from The Cart implementation of the system, so secret key is in a global field, you'll have to assign it per asset with this setup).</i>


<br>


## Sorting Properties
You can apply sorting properties to your download requests by adding them to the sort properties list in the inspector. The text for each entry is the Notion property name you want to sort by, with the tick box set to if you want to sort ascending for that property. The order of the sort properties in the list defines the order they are used, just like in Notion.

![image](https://github.com/user-attachments/assets/a9768b81-77ea-41a5-be95-42e40b887e9f)
<br>
![image](https://github.com/user-attachments/assets/b69ce981-2375-4c3f-ac71-6fdb175479fc)


<br>


## Filters
As of 0.4.x you can also apply filters to the download requests by using the filters window. This window more or less mimic’s Notion’s filters GUI. This setup supports the property types the system currently supports reading. The only notable difference is with rollup support. It is supported, but you’ll have to use the type the rollup is displaying and then define it as a rollup of that type for it to work.

![image](https://github.com/user-attachments/assets/78950c77-8f81-4aac-8036-7aadf9637a85)
<br><br>
<b>Notion Example</b><br>
![image](https://github.com/user-attachments/assets/cdbaddf2-4446-4df8-a841-d40819ca4602)
<br><br>
<b>Unity Example</b><br>
![image](https://github.com/user-attachments/assets/bba720f5-bb14-41c9-818e-ef776f77c303)


<br>


## Wrapper classes
Some data needs a wrapper class to assign references. This is provided for GameObject prefabs, Sprites & AudioClips should you need it. They are assigned by the name of the asset when downloading the data.


<br>


## Post Download Logic
You can also manipulate the data you download after receiving it by writing an override to the method called ```PostDataDownloaded()``` on the ```Notion Data Asset``` . Note if you need to run editor logic, make sure it is in a #ifdef. An example below:

```
#if UNITY_EDITOR
        protected override void PostDataDownloaded()
        {
            skillLookup = new SerializableDictionary<string, DataHealingSkills>();
            
            foreach (var data in Data)
            {
                // Runs a method called PostDownloadLogic() on the data class instance.
                data.GetType().GetMethod("PostDownloadLogic", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.Invoke(data, null);

                // Adds the data class to a lookup for easier use at runtime.
                skillLookup.Add(data.Name, data);
            }

            // Saves the changes to the scriptable object.
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
```


<br>


## Updating all assets
You can download all data assets in one process through an additional editor window. The window can be found under ```Tools > Carter Games > Standalone > Notion Data > Update Data```. The window has the option to halt the downloading of assets if an error occurs, by default this true. To download all assets in the project, just press the download button and wait for the process to complete.

![image](https://github.com/user-attachments/assets/5f5bfbd0-f805-4e90-be74-570b45ab476f)


<br><br>


# Scripting Api Info
If you are using custom assembly definitions you will need to reference the runtime assembly from this asset in-order to access the API such as the ```DataAccess``` class. If you are not using custom assemblies, you should be able to access all the API by default. The runtime assembly is called ```CarterGames.Standalone.NotionData.Runtime```


<br>

# Custom Notion Database Processors
Processors are what handle converting the data received from Notion into something we can use in Unity. The standard processor is always available for use and will attempt to process each column of the database that was downloaded into fields that match those column names 1-2-1. For simple use-cases, this will be enough. If you need extra functionality or the ability to merge properties into collections etc you’ll need to make your own processor. 

API explanation

```
// Contains the result of the download from Notion
NotionDatabaseQueryResult result

// Is a collection of all the rows on the database that was downloaded.
result.Rows

// In each row there is a data lookup which can be used to search for specific
// columns by their string name as shown in Notion.
result.Rows[0].DataLookup

// From here you'll have a NotionProperty type to use.
// This can be converted to a specific type like Date, Number, Select etc.
// Which formats the json value into a c# equivalent.
// You will need to do extra work to parse them fully though.
// For enums you'll need to parse them into their specific types to read them as an example.

// Converts the property to a rich text type property and surfaces the value as a string for use.
result.Rows[0].DataLookup["myproperty"].RichText().Value

// Other types as an example.
result.Rows[0].DataLookup["myproperty"].Date().Value
result.Rows[0].DataLookup["myproperty"].Number().Value

// Attempts to convert to the entered type (used in the standard processor, can be used in any custom ones as well).
result.Rows[0].DataLookup["myproperty"].TryConvertValueToType<T>(out var result)
```

An example of a custom processor:
```
// Converts data into a list of localization entries
// which are a key string value string class.
[Serializable]
public sealed class NotionDatabaseProcessorLocalization : NotionDatabaseProcessor
{
	/// <summary>
	/// Parses the data when called.
	/// </summary>
	/// <param name="result">The result of the download to use.</param>
	/// <returns>The parsed data to set on the asset.</returns>
	public override List<object> Process<T>(NotionDatabaseQueryResult result)
	{
		//The resulting process has to be a generic object.
		// Data will be cast to the type T when processed on the data asset for use.
		var list = new List<object>();

		foreach (var row in result.Rows)
		{
			var entries = new List<LocalizationEntry>();

			// Gets all values that are not the id and iterates through them.
			foreach (var k in row.DataLookup.Keys.Where(t => t != "id"))
			{
				// For each value it makes a new LocalizationEntry with the column string as the key and the value as the richtext value.
				var valueData = row.DataLookup[k];
				entries.Add(new LocalizationEntry(k, valueData.RichText().Value));
			}
                
			// Adds a LocalizationData class which contains a string and a list of LocalizationEntry  as the value.
			// With the column id that was ignored earlier used as the string for the key.
			list.Add(new LocalizationData(row.DataLookup["id"].RichText().Value, entries));
		}

		return list;
	}
}
```

The database it reads from:
<br>
![fdfdfdfdfimage](https://github.com/user-attachments/assets/66a41c6c-ffc0-46fa-94b3-ee0d7b2b08a2)

<br>

# Accessing Notion Data Assets
You can reference the assets as you would a normal scriptable object in the inspector. Or you can use the ```DataAccess``` class in the project to get them via code. Each Notion Data Asset has a variant id in the inspector. By default the variant id is a new GUID on creation. You can change this to help you identify a single instance of assets of the same type as another. Some example usage below:

```
private void OnEnable()
{
	// Gets the first asset of the type found.
	var asset = DataAccess.GetAsset<NotionDataAssetLevels>();
	
	// Gets the asset of the matching variant id.
	asset = DataAccess.GetAsset<NotionDataAssetLevels>("MyAssetVariantId");
	
	// Gets all of the assets of the type found.
	var assets = DataAccess.GetAssets<NotionDataAssetLevels>();
}
```



<br><br>

# Usage Example
Below is an example using the system to store data for Persona 5 healing skills for persona's

<b>Notion</b>
A database of all the skills for healing:

![image](https://github.com/CarterGames/NotionToUnity/assets/33253710/d8077e61-102a-424e-a6f9-f831c979c9cb)


<b>Unity</b>
The downloaded data in Unity:

![313001350-184013f5-6c60-4331-a64c-fec8e16d09b0](https://github.com/user-attachments/assets/ceaa7564-7680-4d2a-b4c3-4fd9859f39c5)

<br>
