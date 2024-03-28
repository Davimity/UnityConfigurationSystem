using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Configurator
{
    private readonly static Dictionary<string, string> config = new Dictionary<string, string>();
    private static string path = $"{Application.persistentDataPath}/config/conf.config";

    /// <returns>The actual path of the Configurator</returns>
    public static string getPath() => path;

    /// <summary> Create a new config file on the specified path. If the directory doesn't exist, it will be created. </summary>
    public static void CreateConfig(){
        if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, "");
    }

    /// <summary> Set the path of the Configurator. It must be rooted and can't be null. </summary>
    /// <exception cref="System.Exception"></exception>
    public static void setPath(string _path){
        if (_path == null) throw new System.Exception("Configurator: Path can't be null.");
        IsPathValid(_path);
        path = _path;
    }

    /// <summary> Check if the path is valid and if the .config file exists. THE METHOD DOESN'T CHECK IF THE CONFIGURATION TEXT IS OK. </summary>
    /// <exception cref="System.Exception"></exception>
    public static void CheckConfigIntegrity(){
        if (path == null) throw new System.Exception("Configurator: Path is null.");
        IsPathValid(path);
        if(!File.Exists(path)) throw new System.Exception("Configurator: Configuration file does not exist. Use CreateConfig method to create one or modify the path.");
    }

    /// <summary> Load the configuration on the Dictionary. Checks the .config file integrity (if exists or not, not text integrity)</summary>
    /// <exception cref="System.Exception"></exception>
    public static void LoadConfig(){
        CheckConfigIntegrity();
        try{
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines) {
                string[] parts = line.Split('=');

                if (config.ContainsKey(parts[0])) config[parts[0]] = parts[1];
                else config.Add(parts[0], parts[1]);
            }
        }catch(System.Exception e){
            throw new System.Exception("Configurator: Error loading configuration: " + e.Message);
        }
    }

    /// <summary> Save the actual configurations saved on the dictionary on the .config file. Checks the .config file integrity (if exists or not, not text integrity)</summary>
    /// <exception cref="System.Exception"></exception>
    public static void SaveConfig()
    {
        CheckConfigIntegrity();
        try{
            string[] lines = new string[config.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> entry in config){
                lines[i] = $"{entry.Key}={entry.Value}";
                i++;
            }

            File.WriteAllLines(path, lines);
        }
        catch (Exception e)
        {
            throw new Exception("Configurator: Error saving configuration: " + e.Message);
        }
    }

    /// <summary> Get a configuration from the dictionary. If the key is not found, it throws an exception. </summary>
    /// <returns>The value of the requested configuration key</returns>
    /// <exception cref="System.Exception"></exception>
    public static string getConfig(string key){
        if (config.ContainsKey(key)) return config[key];
        else throw new System.Exception("Configuration '" + key + "' not found.");
    }

    /// <summary>Set a new value of a configuration</summary>
    /// <param name="key">Key (name) of the configuration you want to modify</param>
    /// <param name="value">New value of the configuration (in string, everything is stored in strings)</param>
    /// <param name="createNewIfDoesntExist">If set to 'true' and the specified key doesn't exist, it will be created. Otherwise a exception will be thrown</param>
    /// <exception cref="System.Exception"></exception>
    public static void SetConfig(string key, string value, bool createNewIfDoesntExist = false){
        if (config.ContainsKey(key)) config[key] = value;
        else if(createNewIfDoesntExist) config.Add(key, value);
        else throw new System.Exception("Configuration '" + key + "' not found.");
    }

    private static void IsPathValid(string path){
        if (!Path.IsPathRooted(path)) throw new System.Exception("Configurator: Path is not valid, it's not rooted.");
        if(!Directory.Exists(Path.GetDirectoryName(path))) throw new System.Exception("Configurator: configuration directory does not exist. The path is: " + path);
    }
}
