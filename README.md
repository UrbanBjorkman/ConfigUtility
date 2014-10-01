ConfigUtility
=============

MSBuild script utilities to clear and merge appsetting configs


Usage
-----

Include a compiled dll in your project and reference it in your build MSBuild Script

```
<UsingTask TaskName="ClearConfig" AssemblyFile="Libs\ConfigUtility.dll" />
<UsingTask TaskName="MergeConfig" AssemblyFile="Libs\ConfigUtility.dll" />
```

Then you can use merge and clear in your buildsteps

```
<ClearConfig ConfigFilename="appSettings.config" />
<MergeConfig SourceConfigFilename="enviroment.config" TargetConfigFilename="appSettings.config" />
```


Example script
--------------

The script first clears all settings in the appsettings.config file 
then it merges client.config into appsettings.config and after that it merges enviroment.config into appsettings.config

```
<?xml version="1.0" encoding="utf-8" ?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003" DefaultTargets="TestTarget">

  <UsingTask TaskName="MergeConfig"
						 AssemblyFile="Libs\ConfigUtility.dll" />

  <UsingTask TaskName="ClearConfig"
           AssemblyFile="Libs\ConfigUtility.dll" />
  
  <Target Name="TestTarget">
    <ClearConfig ConfigFilename="appSettings.config" />

	<MergeConfig SourceConfigFilename="client.config" TargetConfigFilename="appSettings.config" />
    <MergeConfig SourceConfigFilename="enviroment.config" TargetConfigFilename="appSettings.config" />
  </Target>

</Project>
```

Config file example
-------------------

The config file should be formatted in this manner

filename: appSettings.config
```
<?xml version="1.0" encoding="utf-8"?>
  <appSettings>
    <add key="Namekey1" value="thevalue" />
    <add key="Namekey2" value="othervalue" />
  </appSettings>
```

wich then can be referenced from your app.config or web.config

 ```
 <appSettings configSource="appSettings.config" />
 ```